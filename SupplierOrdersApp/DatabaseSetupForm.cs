namespace SupplierOrdersApp;

public partial class DatabaseSetupForm : Form
{
    private readonly DatabaseRepository _repository = AppConfig.CreateRepository();

    public DatabaseSetupForm()
    {
        InitializeComponent();
        AppTheme.ApplyBackground(this);
    }

    private async void CheckButton_Click(object sender, EventArgs e) =>
        await RunSafelyAsync(async () =>
        {
            await _repository.TestConnectionAsync();
            statusLabel.Text = "Подключение успешно";
        });

    private async void CreateTablesButton_Click(object sender, EventArgs e) =>
        await RunSafelyAsync(async () =>
        {
            await _repository.InitializeAsync();
            statusLabel.Text = "Таблицы созданы";
        });

    private async void SeedButton_Click(object sender, EventArgs e) =>
        await RunSafelyAsync(async () =>
        {
            await _repository.SeedAsync();
            statusLabel.Text = "Тестовые данные добавлены";
        });

    private void BackButton_Click(object sender, EventArgs e) => Close();

    private async Task RunSafelyAsync(Func<Task> action)
    {
        try
        {
            ToggleButtons(false);
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
            ToggleButtons(true);
        }
    }

    private void ToggleButtons(bool enabled)
    {
        checkButton.Enabled = enabled;
        createTablesButton.Enabled = enabled;
        seedButton.Enabled = enabled;
        backButton.Enabled = enabled;
    }
}
