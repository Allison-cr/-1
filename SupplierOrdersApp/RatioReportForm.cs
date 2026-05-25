namespace SupplierOrdersApp;

public partial class RatioReportForm : Form
{
    private readonly DatabaseRepository _repository = AppConfig.CreateRepository();
    private readonly BindingSource _ratiosSource = new();

    public RatioReportForm()
    {
        InitializeComponent();
        AppTheme.ApplyBackground(this);
        ratiosGrid.DataSource = _ratiosSource;
        pieChart.SetValues(0, 0);
    }

    private async void RatioReportForm_Load(object sender, EventArgs e) =>
        await RunSafelyAsync(LoadSuppliersAsync);

    private async Task LoadSuppliersAsync()
    {
        supplierList.Items.Clear();
        supplierList.DisplayMember = nameof(SupplierInfo.Name);
        foreach (var supplier in await _repository.GetSuppliersAsync())
        {
            supplierList.Items.Add(supplier, true);
        }

        statusLabel.Text = "Поставщики загружены";
    }

    private async void BuildButton_Click(object sender, EventArgs e) =>
        await RunSafelyAsync(async () =>
        {
            var selectedSuppliers = supplierList.CheckedItems.Cast<SupplierInfo>().Select(s => s.Id).ToArray();
            var rows = await _repository.GetSupplierRatiosAsync(fromPicker.Value, toPicker.Value, selectedSuppliers);
            _ratiosSource.DataSource = rows;
            pieChart.SetValues(rows.Sum(r => r.OrdersAmount), rows.Sum(r => r.RefusalsAmount));
            statusLabel.Text = $"Сформировано поставщиков: {rows.Count}";
        });

    private void BackButton_Click(object sender, EventArgs e) => Close();

    private async Task RunSafelyAsync(Func<Task> action)
    {
        try
        {
            buildButton.Enabled = false;
            statusLabel.ForeColor = Color.DimGray;
            statusLabel.Text = "Выполняется...";
            await action();
            statusLabel.ForeColor = Color.ForestGreen;
        }
        catch (Exception ex)
        {
            statusLabel.Text = "Ошибка";
            statusLabel.ForeColor = Color.Firebrick;
            MessageBox.Show(this, ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            buildButton.Enabled = true;
        }
    }
}
