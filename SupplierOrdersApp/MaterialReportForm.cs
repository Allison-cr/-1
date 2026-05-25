namespace SupplierOrdersApp;

public partial class MaterialReportForm : Form
{
    private readonly DatabaseRepository _repository = AppConfig.CreateRepository();
    private readonly BindingSource _materialsSource = new();

    public MaterialReportForm()
    {
        InitializeComponent();
        AppTheme.ApplyBackground(this);
        materialsGrid.DataSource = _materialsSource;
    }

    private async void BuildButton_Click(object sender, EventArgs e) =>
        await RunSafelyAsync(async () =>
        {
            var rows = await _repository.GetMaterialReportAsync(fromPicker.Value, toPicker.Value);
            _materialsSource.DataSource = rows;
            statusLabel.Text = $"Сформировано строк: {rows.Count}";
        });

    private void ExportButton_Click(object sender, EventArgs e)
    {
        if (_materialsSource.DataSource is not IReadOnlyList<MaterialReportRow> rows || rows.Count == 0)
        {
            MessageBox.Show(this, "Сначала сформируйте отчет.", "Экспорт", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var dialog = new SaveFileDialog
        {
            Filter = "Excel workbook (*.xlsx)|*.xlsx",
            FileName = $"material-report-{DateTime.Now:yyyyMMdd-HHmm}.xlsx"
        };

        if (dialog.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        ExcelExporter.ExportMaterialReport(dialog.FileName, rows, fromPicker.Value, toPicker.Value);
        statusLabel.Text = "Excel-файл сохранен";
        statusLabel.ForeColor = Color.ForestGreen;
    }

    private void BackButton_Click(object sender, EventArgs e) => Close();

    private async Task RunSafelyAsync(Func<Task> action)
    {
        try
        {
            buildButton.Enabled = false;
            exportButton.Enabled = false;
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
            exportButton.Enabled = true;
        }
    }
}
