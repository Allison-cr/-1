namespace SupplierOrdersApp;

public sealed record SupplierInfo(int Id, string Name);

public sealed record ProductInfo(int Id, string Name, string Unit);

public sealed record OrderLineInfo(int ProductId, string ProductName, decimal UnitPrice, int Quantity, decimal LineAmount);

public sealed record OrderInfo(
    int Id,
    int SupplierId,
    string SupplierName,
    DateTime OrderDate,
    DateTime? ConfirmedAt,
    decimal TotalAmount,
    decimal PaidAmount,
    DateTime? DueDate,
    bool IsOverdue);

public sealed record MaterialReportRow(
    string Supplier,
    string Product,
    string Unit,
    int Quantity,
    decimal TotalAmount,
    decimal PaidAmount);

public sealed record SupplierRatioRow(
    string Supplier,
    decimal OrdersAmount,
    decimal RefusalsAmount);
