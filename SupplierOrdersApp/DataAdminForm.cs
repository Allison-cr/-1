using System.Data;
using Npgsql;

namespace SupplierOrdersApp;

public sealed class DataAdminForm : Form
{
    private readonly string _connectionString = AppConfig.ConnectionString;
    private readonly BindingSource _suppliersSource = new();
    private readonly BindingSource _productsSource = new();
    private readonly BindingSource _ordersSource = new();
    private readonly BindingSource _itemsSource = new();
    private readonly BindingSource _paymentsSource = new();

    private readonly DataGridView _suppliersGrid = Grid();
    private readonly DataGridView _productsGrid = Grid();
    private readonly DataGridView _ordersGrid = Grid();
    private readonly DataGridView _itemsGrid = Grid();
    private readonly DataGridView _paymentsGrid = Grid();

    private readonly TextBox _supplierName = new() { Dock = DockStyle.Fill };
    private readonly TextBox _supplierContact = new() { Dock = DockStyle.Fill };
    private readonly TextBox _supplierPhone = new() { Dock = DockStyle.Fill };
    private readonly TextBox _productName = new() { Dock = DockStyle.Fill };
    private readonly TextBox _productUnit = new() { Dock = DockStyle.Fill };
    private readonly ComboBox _orderSupplier = Combo();
    private readonly DateTimePicker _orderDate = new() { Dock = DockStyle.Fill, Format = DateTimePickerFormat.Short };
    private readonly CheckBox _orderConfirmed = new() { Text = "Подтвержден", Dock = DockStyle.Fill };
    private readonly TextBox _orderNote = new() { Dock = DockStyle.Fill };
    private readonly ComboBox _itemProduct = Combo();
    private readonly NumericUpDown _itemQuantity = Number(1, 100000, 0);
    private readonly NumericUpDown _itemPrice = Number(0, 10000000, 2);
    private readonly ComboBox _paymentOrder = Combo();
    private readonly DateTimePicker _paymentDate = new() { Dock = DockStyle.Fill, Format = DateTimePickerFormat.Short };
    private readonly NumericUpDown _paymentAmount = Number(1, 10000000, 2);
    private readonly Label _status = new() { Dock = DockStyle.Bottom, Height = 28, Text = "Готово", ForeColor = Color.DimGray };

    public DataAdminForm()
    {
        Text = "Управление данными";
        Width = 1240;
        Height = 780;
        MinimumSize = new Size(1100, 690);
        Font = new Font("Segoe UI", 10f);
        BackColor = Color.WhiteSmoke;
        BuildUi();
        Load += async (_, _) => await ReloadAllAsync();
    }

    private void BuildUi()
    {
        var tabs = new TabControl { Dock = DockStyle.Fill };
        tabs.TabPages.Add(BuildDirectoriesPage());
        tabs.TabPages.Add(BuildOrdersPage());
        tabs.TabPages.Add(BuildPaymentsPage());
        Controls.Add(tabs);
        Controls.Add(_status);
    }

    private TabPage BuildDirectoriesPage()
    {
        var page = new TabPage("Справочники");
        var split = new SplitContainer { Dock = DockStyle.Fill, Orientation = Orientation.Horizontal, SplitterDistance = 330 };
        page.Controls.Add(split);

        split.Panel1.Controls.Add(BuildCrudBlock(
            "Поставщики",
            _suppliersGrid,
            new (string, Control)[] { ("Название", _supplierName), ("Контакт", _supplierContact), ("Телефон", _supplierPhone) },
            AddSupplierAsync,
            UpdateSupplierAsync,
            DeleteSupplierAsync));

        split.Panel2.Controls.Add(BuildCrudBlock(
            "Товары",
            _productsGrid,
            new (string, Control)[] { ("Название", _productName), ("Ед. изм.", _productUnit) },
            AddProductAsync,
            UpdateProductAsync,
            DeleteProductAsync));

        _suppliersGrid.DataSource = _suppliersSource;
        _productsGrid.DataSource = _productsSource;
        _suppliersGrid.SelectionChanged += (_, _) => FillSupplierFields();
        _productsGrid.SelectionChanged += (_, _) => FillProductFields();
        return page;
    }

    private TabPage BuildOrdersPage()
    {
        var page = new TabPage("Заказы и состав");
        var split = new SplitContainer { Dock = DockStyle.Fill, Orientation = Orientation.Horizontal, SplitterDistance = 360 };
        page.Controls.Add(split);

        split.Panel1.Controls.Add(BuildCrudBlock(
            "Заказы поставщикам",
            _ordersGrid,
            new (string, Control)[] { ("Поставщик", _orderSupplier), ("Дата", _orderDate), ("Статус", _orderConfirmed), ("Примечание", _orderNote) },
            AddOrderAsync,
            UpdateOrderAsync,
            DeleteOrderAsync));

        split.Panel2.Controls.Add(BuildCrudBlock(
            "Состав выбранного заказа",
            _itemsGrid,
            new (string, Control)[] { ("Товар", _itemProduct), ("Количество", _itemQuantity), ("Цена", _itemPrice) },
            AddItemAsync,
            UpdateItemAsync,
            DeleteItemAsync));

        _ordersGrid.DataSource = _ordersSource;
        _itemsGrid.DataSource = _itemsSource;
        _ordersGrid.SelectionChanged += async (_, _) =>
        {
            FillOrderFields();
            await LoadOrderItemsAsync(SelectedId(_ordersGrid));
        };
        _itemsGrid.SelectionChanged += (_, _) => FillItemFields();
        return page;
    }

    private TabPage BuildPaymentsPage()
    {
        var page = new TabPage("Оплаты");
        page.Controls.Add(BuildCrudBlock(
            "Оплаты заказов",
            _paymentsGrid,
            new (string, Control)[] { ("Заказ", _paymentOrder), ("Дата оплаты", _paymentDate), ("Сумма", _paymentAmount) },
            AddPaymentAsync,
            UpdatePaymentAsync,
            DeletePaymentAsync));
        _paymentsGrid.DataSource = _paymentsSource;
        _paymentsGrid.SelectionChanged += (_, _) => FillPaymentFields();
        return page;
    }

    private Control BuildCrudBlock(string title, DataGridView grid, (string Label, Control Control)[] fields, Func<Task> add, Func<Task> update, Func<Task> delete)
    {
        var root = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(10), ColumnCount = 2, RowCount = 2 };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 340));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.Controls.Add(new Label { Text = title, Dock = DockStyle.Fill, Font = new Font("Segoe UI", 12f, FontStyle.Bold) }, 0, 0);
        root.Controls.Add(grid, 0, 1);

        var editor = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = fields.Length + 4, Padding = new Padding(8) };
        editor.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
        editor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        foreach (var field in fields.Select((value, index) => new { value, index }))
        {
            editor.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));
            editor.Controls.Add(new Label { Text = field.value.Label, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, field.index);
            editor.Controls.Add(field.value.Control, 1, field.index);
        }

        var addButton = ActionButton("Добавить");
        var updateButton = ActionButton("Изменить");
        var deleteButton = ActionButton("Удалить");
        var refreshButton = ActionButton("Обновить");
        var start = fields.Length;
        editor.Controls.Add(addButton, 0, start);
        editor.SetColumnSpan(addButton, 2);
        editor.Controls.Add(updateButton, 0, start + 1);
        editor.SetColumnSpan(updateButton, 2);
        editor.Controls.Add(deleteButton, 0, start + 2);
        editor.SetColumnSpan(deleteButton, 2);
        editor.Controls.Add(refreshButton, 0, start + 3);
        editor.SetColumnSpan(refreshButton, 2);
        addButton.Click += async (_, _) => await RunAsync(add);
        updateButton.Click += async (_, _) => await RunAsync(update);
        deleteButton.Click += async (_, _) => await RunAsync(delete);
        refreshButton.Click += async (_, _) => await ReloadAllAsync();
        root.Controls.Add(editor, 1, 1);
        return root;
    }

    private async Task ReloadAllAsync()
    {
        await RunAsync(async () =>
        {
            await LoadSuppliersAsync();
            await LoadProductsAsync();
            await LoadOrdersAsync();
            await LoadPaymentsAsync();
            await LoadCombosAsync();
            HideIds(_suppliersGrid, _productsGrid, _ordersGrid, _itemsGrid, _paymentsGrid);
            _status.Text = "Данные обновлены";
        }, showWorking: false);
    }

    private async Task LoadSuppliersAsync() => _suppliersSource.DataSource = await QueryAsync("select id, name as \"Название\", contact_name as \"Контакт\", phone as \"Телефон\" from suppliers order by id");

    private async Task LoadProductsAsync() => _productsSource.DataSource = await QueryAsync("select id, name as \"Название\", unit as \"Ед. изм.\" from products order by id");

    private async Task LoadOrdersAsync()
    {
        _ordersSource.DataSource = await QueryAsync("""
            select po.id,
                   s.name as "Поставщик",
                   po.supplier_id,
                   po.order_date as "Дата заказа",
                   po.confirmed_at as "Дата подтверждения",
                   po.note as "Примечание"
            from purchase_orders po
            join suppliers s on s.id = po.supplier_id
            order by po.id
            """);
        await LoadOrderItemsAsync(SelectedId(_ordersGrid));
    }

    private async Task LoadOrderItemsAsync(int orderId)
    {
        if (orderId == 0)
        {
            _itemsSource.DataSource = new DataTable();
            return;
        }

        _itemsSource.DataSource = await QueryAsync("""
            select i.id,
                   i.order_id,
                   p.name as "Товар",
                   i.product_id,
                   i.quantity as "Количество",
                   i.unit_price as "Цена",
                   (i.quantity * i.unit_price)::numeric(12,2) as "Сумма"
            from purchase_order_items i
            join products p on p.id = i.product_id
            where i.order_id = @order_id
            order by i.id
            """, ("order_id", orderId));
        HideIds(_itemsGrid);
    }

    private async Task LoadPaymentsAsync() => _paymentsSource.DataSource = await QueryAsync("""
        select p.id,
               p.order_id,
               ('Заказ #' || po.id || ' от ' || to_char(po.order_date, 'DD.MM.YYYY') || ', ' || s.name) as "Заказ",
               p.payment_date as "Дата оплаты",
               p.amount as "Сумма"
        from payments p
        join purchase_orders po on po.id = p.order_id
        join suppliers s on s.id = po.supplier_id
        order by p.id
        """);

    private async Task LoadCombosAsync()
    {
        await FillComboAsync(_orderSupplier, "select id, name from suppliers order by name", "name", "id");
        await FillComboAsync(_itemProduct, "select id, name from products order by name", "name", "id");
        await FillComboAsync(_paymentOrder, """
            select po.id, ('Заказ #' || po.id || ' от ' || to_char(po.order_date, 'DD.MM.YYYY') || ', ' || s.name) as title
            from purchase_orders po
            join suppliers s on s.id = po.supplier_id
            order by po.id
            """, "title", "id");
    }

    private async Task AddSupplierAsync()
    {
        await ExecuteAsync("insert into suppliers (name, contact_name, phone) values (@name, @contact, @phone)", ("name", _supplierName.Text), ("contact", _supplierContact.Text), ("phone", _supplierPhone.Text));
        await ReloadAllAsync();
    }

    private async Task UpdateSupplierAsync()
    {
        var id = RequireSelectedId(_suppliersGrid);
        await ExecuteAsync("update suppliers set name=@name, contact_name=@contact, phone=@phone where id=@id", ("id", id), ("name", _supplierName.Text), ("contact", _supplierContact.Text), ("phone", _supplierPhone.Text));
        await ReloadAllAsync();
    }

    private async Task DeleteSupplierAsync()
    {
        var id = RequireSelectedId(_suppliersGrid);
        await ExecuteAsync("delete from suppliers where id=@id", ("id", id));
        await ReloadAllAsync();
    }

    private async Task AddProductAsync()
    {
        await ExecuteAsync("insert into products (name, unit) values (@name, @unit)", ("name", _productName.Text), ("unit", _productUnit.Text));
        await ReloadAllAsync();
    }

    private async Task UpdateProductAsync()
    {
        var id = RequireSelectedId(_productsGrid);
        await ExecuteAsync("update products set name=@name, unit=@unit where id=@id", ("id", id), ("name", _productName.Text), ("unit", _productUnit.Text));
        await ReloadAllAsync();
    }

    private async Task DeleteProductAsync()
    {
        var id = RequireSelectedId(_productsGrid);
        await ExecuteAsync("delete from products where id=@id", ("id", id));
        await ReloadAllAsync();
    }

    private async Task AddOrderAsync()
    {
        await ExecuteAsync("insert into purchase_orders (supplier_id, order_date, confirmed_at, note) values (@supplier, @date, @confirmed, @note)",
            ("supplier", ComboValue(_orderSupplier)),
            ("date", _orderDate.Value.Date),
            ("confirmed", _orderConfirmed.Checked ? _orderDate.Value.Date : (object)DBNull.Value),
            ("note", _orderNote.Text));
        await ReloadAllAsync();
    }

    private async Task UpdateOrderAsync()
    {
        var id = RequireSelectedId(_ordersGrid);
        await ExecuteAsync("update purchase_orders set supplier_id=@supplier, order_date=@date, confirmed_at=@confirmed, note=@note where id=@id",
            ("id", id),
            ("supplier", ComboValue(_orderSupplier)),
            ("date", _orderDate.Value.Date),
            ("confirmed", _orderConfirmed.Checked ? _orderDate.Value.Date : (object)DBNull.Value),
            ("note", _orderNote.Text));
        await ReloadAllAsync();
    }

    private async Task DeleteOrderAsync()
    {
        var id = RequireSelectedId(_ordersGrid);
        await ExecuteAsync("delete from purchase_orders where id=@id", ("id", id));
        await ReloadAllAsync();
    }

    private async Task AddItemAsync()
    {
        var orderId = RequireSelectedId(_ordersGrid);
        await ExecuteAsync("insert into purchase_order_items (order_id, product_id, quantity, unit_price) values (@order, @product, @quantity, @price)",
            ("order", orderId), ("product", ComboValue(_itemProduct)), ("quantity", (int)_itemQuantity.Value), ("price", _itemPrice.Value));
        await LoadOrdersAsync();
    }

    private async Task UpdateItemAsync()
    {
        var id = RequireSelectedId(_itemsGrid);
        await ExecuteAsync("update purchase_order_items set product_id=@product, quantity=@quantity, unit_price=@price where id=@id",
            ("id", id), ("product", ComboValue(_itemProduct)), ("quantity", (int)_itemQuantity.Value), ("price", _itemPrice.Value));
        await LoadOrdersAsync();
    }

    private async Task DeleteItemAsync()
    {
        var id = RequireSelectedId(_itemsGrid);
        await ExecuteAsync("delete from purchase_order_items where id=@id", ("id", id));
        await LoadOrdersAsync();
    }

    private async Task AddPaymentAsync()
    {
        await ExecuteAsync("insert into payments (order_id, payment_date, amount) values (@order, @date, @amount)",
            ("order", ComboValue(_paymentOrder)), ("date", _paymentDate.Value.Date), ("amount", _paymentAmount.Value));
        await ReloadAllAsync();
    }

    private async Task UpdatePaymentAsync()
    {
        var id = RequireSelectedId(_paymentsGrid);
        await ExecuteAsync("update payments set order_id=@order, payment_date=@date, amount=@amount where id=@id",
            ("id", id), ("order", ComboValue(_paymentOrder)), ("date", _paymentDate.Value.Date), ("amount", _paymentAmount.Value));
        await ReloadAllAsync();
    }

    private async Task DeletePaymentAsync()
    {
        var id = RequireSelectedId(_paymentsGrid);
        await ExecuteAsync("delete from payments where id=@id", ("id", id));
        await ReloadAllAsync();
    }

    private void FillSupplierFields()
    {
        if (CurrentRow(_suppliersGrid) is not { } row) return;
        _supplierName.Text = TextValue(row, "Название");
        _supplierContact.Text = TextValue(row, "Контакт");
        _supplierPhone.Text = TextValue(row, "Телефон");
    }

    private void FillProductFields()
    {
        if (CurrentRow(_productsGrid) is not { } row) return;
        _productName.Text = TextValue(row, "Название");
        _productUnit.Text = TextValue(row, "Ед. изм.");
    }

    private void FillOrderFields()
    {
        if (CurrentRow(_ordersGrid) is not { } row) return;
        _orderSupplier.SelectedValue = Convert.ToInt32(row["supplier_id"]);
        _orderDate.Value = Convert.ToDateTime(row["Дата заказа"]);
        _orderConfirmed.Checked = row["Дата подтверждения"] is not DBNull;
        _orderNote.Text = TextValue(row, "Примечание");
    }

    private void FillItemFields()
    {
        if (CurrentRow(_itemsGrid) is not { } row) return;
        _itemProduct.SelectedValue = Convert.ToInt32(row["product_id"]);
        _itemQuantity.Value = Convert.ToDecimal(row["Количество"]);
        _itemPrice.Value = Convert.ToDecimal(row["Цена"]);
    }

    private void FillPaymentFields()
    {
        if (CurrentRow(_paymentsGrid) is not { } row) return;
        _paymentOrder.SelectedValue = Convert.ToInt32(row["order_id"]);
        _paymentDate.Value = Convert.ToDateTime(row["Дата оплаты"]);
        _paymentAmount.Value = Convert.ToDecimal(row["Сумма"]);
    }

    private async Task<DataTable> QueryAsync(string sql, params (string Name, object? Value)[] parameters)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(sql, connection);
        AddParameters(command, parameters);
        var table = new DataTable();
        await using var reader = await command.ExecuteReaderAsync();
        table.Load(reader);
        return table;
    }

    private async Task ExecuteAsync(string sql, params (string Name, object? Value)[] parameters)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(sql, connection);
        AddParameters(command, parameters);
        await command.ExecuteNonQueryAsync();
    }

    private async Task FillComboAsync(ComboBox combo, string sql, string displayMember, string valueMember)
    {
        combo.DataSource = await QueryAsync(sql);
        combo.DisplayMember = displayMember;
        combo.ValueMember = valueMember;
    }

    private async Task RunAsync(Func<Task> action, bool showWorking = true)
    {
        try
        {
            if (showWorking)
            {
                _status.Text = "Выполняется...";
                _status.ForeColor = Color.DimGray;
            }
            await action();
            _status.ForeColor = Color.ForestGreen;
        }
        catch (Exception ex)
        {
            _status.Text = "Ошибка";
            _status.ForeColor = Color.Firebrick;
            MessageBox.Show(this, ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private static void AddParameters(NpgsqlCommand command, params (string Name, object? Value)[] parameters)
    {
        foreach (var (name, value) in parameters)
        {
            command.Parameters.AddWithValue(name, value ?? DBNull.Value);
        }
    }

    private static DataGridView Grid() => new()
    {
        Dock = DockStyle.Fill,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
        ReadOnly = true,
        AllowUserToAddRows = false,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        MultiSelect = false
    };

    private static ComboBox Combo() => new() { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };

    private static NumericUpDown Number(decimal minimum, decimal maximum, int decimals) => new()
    {
        Dock = DockStyle.Fill,
        Minimum = minimum,
        Maximum = maximum,
        DecimalPlaces = decimals
    };

    private static Button ActionButton(string text) => new()
    {
        Text = text,
        Dock = DockStyle.Fill,
        Margin = new Padding(3),
        Height = 34
    };

    private static DataRowView? CurrentRow(DataGridView grid) => grid.CurrentRow?.DataBoundItem as DataRowView;

    private static int SelectedId(DataGridView grid) => CurrentRow(grid) is { } row ? Convert.ToInt32(row["id"]) : 0;

    private static int RequireSelectedId(DataGridView grid)
    {
        var id = SelectedId(grid);
        if (id == 0)
        {
            throw new InvalidOperationException("Выберите строку в таблице.");
        }
        return id;
    }

    private static int ComboValue(ComboBox combo)
    {
        if (combo.SelectedValue is null)
        {
            throw new InvalidOperationException("Выберите значение из списка.");
        }
        return Convert.ToInt32(combo.SelectedValue);
    }

    private static string TextValue(DataRowView row, string column) => row[column] is DBNull ? string.Empty : Convert.ToString(row[column]) ?? string.Empty;

    private static void HideIds(params DataGridView[] grids)
    {
        foreach (var grid in grids)
        {
            foreach (DataGridViewColumn column in grid.Columns)
            {
                if (column.Name.EndsWith("id", StringComparison.OrdinalIgnoreCase) || column.Name.EndsWith("_id", StringComparison.OrdinalIgnoreCase))
                {
                    column.Visible = false;
                }
            }
        }
    }
}
