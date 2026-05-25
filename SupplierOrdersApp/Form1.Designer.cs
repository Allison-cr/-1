namespace SupplierOrdersApp;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;
    private Label titleLabel;
    private Label subtitleLabel;
    private Button databaseButton;
    private Button ordersButton;
    private Button dataButton;
    private Button materialsButton;
    private Button ratiosButton;
    private TableLayoutPanel mainLayout;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        titleLabel = new Label();
        subtitleLabel = new Label();
        databaseButton = new Button();
        ordersButton = new Button();
        dataButton = new Button();
        materialsButton = new Button();
        ratiosButton = new Button();
        mainLayout = new TableLayoutPanel();
        mainLayout.SuspendLayout();
        SuspendLayout();

        mainLayout.ColumnCount = 1;
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        mainLayout.Controls.Add(titleLabel, 0, 0);
        mainLayout.Controls.Add(subtitleLabel, 0, 1);
        mainLayout.Controls.Add(databaseButton, 0, 2);
        mainLayout.Controls.Add(ordersButton, 0, 3);
        mainLayout.Controls.Add(dataButton, 0, 4);
        mainLayout.Controls.Add(materialsButton, 0, 5);
        mainLayout.Controls.Add(ratiosButton, 0, 6);
        mainLayout.Dock = DockStyle.Fill;
        mainLayout.Margin = new Padding(24);
        mainLayout.Name = "mainLayout";
        mainLayout.Padding = new Padding(32);
        mainLayout.RowCount = 8;
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 58F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        titleLabel.Dock = DockStyle.Fill;
        titleLabel.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
        titleLabel.Name = "titleLabel";
        titleLabel.TabIndex = 0;
        titleLabel.Text = "Магазин: заказы поставщикам";
        titleLabel.TextAlign = ContentAlignment.MiddleCenter;

        subtitleLabel.Dock = DockStyle.Fill;
        subtitleLabel.ForeColor = Color.DimGray;
        subtitleLabel.Name = "subtitleLabel";
        subtitleLabel.TabIndex = 1;
        subtitleLabel.Text = "Выберите раздел для работы";
        subtitleLabel.TextAlign = ContentAlignment.MiddleCenter;

        ConfigureMenuButton(databaseButton, "База данных", 2);
        databaseButton.Click += DatabaseButton_Click;

        ConfigureMenuButton(ordersButton, "Заказы и оплаты", 3);
        ordersButton.Click += OrdersButton_Click;

        ConfigureMenuButton(dataButton, "Управление таблицами", 4);
        dataButton.Click += DataButton_Click;

        ConfigureMenuButton(materialsButton, "Материальный отчет", 5);
        materialsButton.Click += MaterialsButton_Click;

        ConfigureMenuButton(ratiosButton, "Диаграмма заказов и отказов", 6);
        ratiosButton.Click += RatiosButton_Click;

        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(824, 600);
        Controls.Add(mainLayout);
        Font = new Font("Segoe UI", 10F);
        MinimumSize = new Size(700, 560);
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Главное меню";
        mainLayout.ResumeLayout(false);
        ResumeLayout(false);
    }

    private static void ConfigureMenuButton(Button button, string text, int tabIndex)
    {
        button.Dock = DockStyle.Fill;
        button.Font = new Font("Segoe UI", 12F);
        button.Margin = new Padding(3);
        button.Name = text.Replace(" ", "", StringComparison.Ordinal);
        button.TabIndex = tabIndex;
        button.Text = text;
        button.UseVisualStyleBackColor = true;
    }
}
