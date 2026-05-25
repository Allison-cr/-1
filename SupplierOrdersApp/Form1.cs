namespace SupplierOrdersApp;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
        AppTheme.ApplyBackground(this);
    }

    private void OpenChild(Form child)
    {
        Hide();
        child.StartPosition = FormStartPosition.CenterScreen;
        child.FormClosed += (_, _) => Show();
        child.Show();
    }

    private void OrdersButton_Click(object sender, EventArgs e) =>
        OpenChild(new OrdersForm());

    private void DataButton_Click(object sender, EventArgs e) =>
        OpenChild(new DataAdminForm());

    private void MaterialsButton_Click(object sender, EventArgs e) =>
        OpenChild(new MaterialReportForm());

    private void RatiosButton_Click(object sender, EventArgs e) =>
        OpenChild(new RatioReportForm());

    private void DatabaseButton_Click(object sender, EventArgs e) =>
        OpenChild(new DatabaseSetupForm());
}
