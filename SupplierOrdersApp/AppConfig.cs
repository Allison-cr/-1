namespace SupplierOrdersApp;

public static class AppConfig
{
    public const string ConnectionString = "Host=localhost;Port=5432;Database=supplier_orders;Username=postgres;Password=1234";

    public static DatabaseRepository CreateRepository() => new(ConnectionString);
}
