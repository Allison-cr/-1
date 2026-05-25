using Npgsql;

namespace SupplierOrdersApp;

public sealed class DatabaseRepository(string connectionString)
{
    private readonly string _connectionString = connectionString;

    public async Task TestConnectionAsync()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
    }

    public async Task InitializeAsync()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var sql = """
            create table if not exists suppliers (
                id integer generated always as identity primary key,
                name text not null unique,
                contact_name text,
                phone text
            );

            create table if not exists products (
                id integer generated always as identity primary key,
                name text not null unique,
                unit text not null default 'шт'
            );

            create table if not exists purchase_orders (
                id integer generated always as identity primary key,
                supplier_id integer not null references suppliers(id),
                order_date date not null,
                confirmed_at date,
                note text
            );

            create table if not exists purchase_order_items (
                id integer generated always as identity primary key,
                order_id integer not null references purchase_orders(id) on delete cascade,
                product_id integer not null references products(id),
                quantity integer not null check (quantity > 0),
                unit_price numeric(12,2) not null check (unit_price >= 0)
            );

            create table if not exists payments (
                id integer generated always as identity primary key,
                order_id integer not null references purchase_orders(id) on delete cascade,
                payment_date date not null,
                amount numeric(12,2) not null check (amount > 0)
            );

            create or replace view v_order_totals as
            select
                po.id,
                po.supplier_id,
                s.name as supplier_name,
                po.order_date,
                po.confirmed_at,
                coalesce(sum(i.quantity * i.unit_price), 0)::numeric(12,2) as total_amount,
                coalesce((select sum(p.amount) from payments p where p.order_id = po.id), 0)::numeric(12,2) as paid_amount
            from purchase_orders po
            join suppliers s on s.id = po.supplier_id
            left join purchase_order_items i on i.order_id = po.id
            group by po.id, s.name;
            """;

        await using var command = new NpgsqlCommand(sql, connection);
        await command.ExecuteNonQueryAsync();
    }

    public async Task SeedAsync()
    {
        await InitializeAsync();

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var transaction = await connection.BeginTransactionAsync();

        var hasData = await ScalarAsync<long>(connection, transaction, "select count(*) from suppliers");
        if (hasData > 0)
        {
            return;
        }

        await ExecuteAsync(connection, transaction, """
            insert into suppliers (name, contact_name, phone) values
            ('ООО Продукт-Сервис', 'Иван Петров', '+7 900 100-10-10'),
            ('ТК Север', 'Мария Смирнова', '+7 900 200-20-20'),
            ('Фермерский двор', 'Анна Кузнецова', '+7 900 300-30-30');

            insert into products (name, unit) values
            ('Молоко 3.2%', 'л'),
            ('Хлеб пшеничный', 'шт'),
            ('Сыр Российский', 'кг'),
            ('Крупа гречневая', 'кг'),
            ('Чай черный', 'уп');

            insert into purchase_orders (supplier_id, order_date, confirmed_at, note) values
            (1, current_date - interval '55 days', current_date - interval '53 days', 'Плановая поставка'),
            (2, current_date - interval '40 days', current_date - interval '39 days', 'Сезонный запас'),
            (3, current_date - interval '20 days', current_date - interval '18 days', 'Пополнение витрины'),
            (1, current_date - interval '10 days', current_date - interval '9 days', 'Срочный заказ'),
            (2, current_date - interval '5 days', null, 'Ожидает подтверждения');

            insert into purchase_order_items (order_id, product_id, quantity, unit_price) values
            (1, 1, 120, 62.50), (1, 2, 200, 31.00), (1, 3, 25, 520.00),
            (2, 4, 300, 88.00), (2, 5, 80, 145.00),
            (3, 1, 90, 64.00), (3, 3, 18, 540.00),
            (4, 2, 150, 32.00), (4, 5, 60, 150.00),
            (5, 4, 120, 91.00);

            insert into payments (order_id, payment_date, amount) values
            (1, current_date - interval '30 days', 26700.00),
            (2, current_date - interval '7 days', 10000.00),
            (3, current_date - interval '3 days', 15480.00);
            """);

        await transaction.CommitAsync();
    }

    public async Task<IReadOnlyList<SupplierInfo>> GetSuppliersAsync()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var result = new List<SupplierInfo>();
        await using var command = new NpgsqlCommand("select id, name from suppliers order by name", connection);
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            result.Add(new SupplierInfo(reader.GetInt32(0), reader.GetString(1)));
        }

        return result;
    }

    public async Task<IReadOnlyList<ProductInfo>> GetProductsAsync()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var result = new List<ProductInfo>();
        await using var command = new NpgsqlCommand("select id, name, unit from products order by name", connection);
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            result.Add(new ProductInfo(reader.GetInt32(0), reader.GetString(1), reader.GetString(2)));
        }

        return result;
    }

    public async Task<IReadOnlyList<OrderInfo>> GetOrdersAsync(DateTime from, DateTime to)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var result = new List<OrderInfo>();
        await using var command = new NpgsqlCommand("""
            select id, supplier_id, supplier_name, order_date, confirmed_at, total_amount, paid_amount
            from v_order_totals
            where order_date between @from and @to
            order by order_date desc, id desc
            """, connection);
        command.Parameters.AddWithValue("from", from.Date);
        command.Parameters.AddWithValue("to", to.Date);

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var confirmedAt = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4);
            var dueDate = confirmedAt?.AddDays(30);
            var total = reader.GetDecimal(5);
            var paid = reader.GetDecimal(6);
            result.Add(new OrderInfo(
                reader.GetInt32(0),
                reader.GetInt32(1),
                reader.GetString(2),
                reader.GetDateTime(3),
                confirmedAt,
                total,
                paid,
                dueDate,
                dueDate.HasValue && dueDate.Value.Date < DateTime.Today && paid < total));
        }

        return result;
    }

    public async Task<int> CreateOrderAsync(int supplierId, DateTime orderDate, bool confirmed, IEnumerable<OrderLineInfo> lines)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var transaction = await connection.BeginTransactionAsync();

        await using var orderCommand = new NpgsqlCommand("""
            insert into purchase_orders (supplier_id, order_date, confirmed_at)
            values (@supplier_id, @order_date, @confirmed_at)
            returning id
            """, connection, transaction);
        orderCommand.Parameters.AddWithValue("supplier_id", supplierId);
        orderCommand.Parameters.AddWithValue("order_date", orderDate.Date);
        orderCommand.Parameters.AddWithValue("confirmed_at", confirmed ? orderDate.Date : (object)DBNull.Value);
        var orderId = (int)(await orderCommand.ExecuteScalarAsync() ?? 0);

        foreach (var line in lines)
        {
            await using var itemCommand = new NpgsqlCommand("""
                insert into purchase_order_items (order_id, product_id, quantity, unit_price)
                values (@order_id, @product_id, @quantity, @unit_price)
                """, connection, transaction);
            itemCommand.Parameters.AddWithValue("order_id", orderId);
            itemCommand.Parameters.AddWithValue("product_id", line.ProductId);
            itemCommand.Parameters.AddWithValue("quantity", line.Quantity);
            itemCommand.Parameters.AddWithValue("unit_price", line.UnitPrice);
            await itemCommand.ExecuteNonQueryAsync();
        }

        await transaction.CommitAsync();
        return orderId;
    }

    public async Task ConfirmOrderAsync(int orderId, DateTime confirmedAt)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand("update purchase_orders set confirmed_at = @date where id = @id", connection);
        command.Parameters.AddWithValue("date", confirmedAt.Date);
        command.Parameters.AddWithValue("id", orderId);
        await command.ExecuteNonQueryAsync();
    }

    public async Task AddPaymentAsync(int orderId, DateTime paymentDate, decimal amount)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand("""
            insert into payments (order_id, payment_date, amount)
            values (@order_id, @payment_date, @amount)
            """, connection);
        command.Parameters.AddWithValue("order_id", orderId);
        command.Parameters.AddWithValue("payment_date", paymentDate.Date);
        command.Parameters.AddWithValue("amount", amount);
        await command.ExecuteNonQueryAsync();
    }

    public async Task<IReadOnlyList<MaterialReportRow>> GetMaterialReportAsync(DateTime from, DateTime to)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var result = new List<MaterialReportRow>();
        await using var command = new NpgsqlCommand("""
            select
                s.name,
                pr.name,
                pr.unit,
                sum(i.quantity)::integer as quantity,
                sum(i.quantity * i.unit_price)::numeric(12,2) as total_amount,
                sum(i.quantity * i.unit_price * case when ot.total_amount = 0 then 0 else least(ot.paid_amount, ot.total_amount) / ot.total_amount end)::numeric(12,2) as paid_amount
            from purchase_orders po
            join suppliers s on s.id = po.supplier_id
            join purchase_order_items i on i.order_id = po.id
            join products pr on pr.id = i.product_id
            join v_order_totals ot on ot.id = po.id
            where po.order_date between @from and @to
            group by s.name, pr.name, pr.unit
            order by s.name, pr.name
            """, connection);
        command.Parameters.AddWithValue("from", from.Date);
        command.Parameters.AddWithValue("to", to.Date);

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            result.Add(new MaterialReportRow(
                reader.GetString(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetInt32(3),
                reader.GetDecimal(4),
                reader.GetDecimal(5)));
        }

        return result;
    }

    public async Task<IReadOnlyList<SupplierRatioRow>> GetSupplierRatiosAsync(DateTime from, DateTime to, IReadOnlyCollection<int> supplierIds)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var result = new List<SupplierRatioRow>();
        await using var command = new NpgsqlCommand("""
            select
                supplier_name,
                sum(total_amount)::numeric(12,2) as orders_amount,
                sum(case
                    when confirmed_at is not null
                         and confirmed_at + interval '30 days' < current_date
                         and paid_amount < total_amount
                    then total_amount - paid_amount
                    else 0
                end)::numeric(12,2) as refusals_amount
            from v_order_totals
            where order_date between @from and @to
              and (cardinality(@supplier_ids) = 0 or supplier_id = any(@supplier_ids))
            group by supplier_name
            order by supplier_name
            """, connection);
        command.Parameters.AddWithValue("from", from.Date);
        command.Parameters.AddWithValue("to", to.Date);
        command.Parameters.AddWithValue("supplier_ids", supplierIds.ToArray());

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            result.Add(new SupplierRatioRow(reader.GetString(0), reader.GetDecimal(1), reader.GetDecimal(2)));
        }

        return result;
    }

    private static async Task ExecuteAsync(NpgsqlConnection connection, NpgsqlTransaction transaction, string sql)
    {
        await using var command = new NpgsqlCommand(sql, connection, transaction);
        await command.ExecuteNonQueryAsync();
    }

    private static async Task<T> ScalarAsync<T>(NpgsqlConnection connection, NpgsqlTransaction transaction, string sql)
    {
        await using var command = new NpgsqlCommand(sql, connection, transaction);
        var value = await command.ExecuteScalarAsync();
        if (value is null || value is DBNull)
        {
            throw new InvalidOperationException("SQL-запрос не вернул значение.");
        }

        return (T)Convert.ChangeType(value, typeof(T));
    }
}
