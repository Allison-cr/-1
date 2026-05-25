namespace SupplierOrdersApp;

partial class DatabaseSetupForm
{
    private System.ComponentModel.IContainer components = null;
    private Label titleLabel;
    private Label connectionLabel;
    private Label statusLabel;
    private Button checkButton;
    private Button createTablesButton;
    private Button seedButton;
    private Button backButton;
    private TableLayoutPanel layout;

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
        connectionLabel = new Label();
        statusLabel = new Label();
        checkButton = new Button();
        createTablesButton = new Button();
        seedButton = new Button();
        backButton = new Button();
        layout = new TableLayoutPanel();
        layout.SuspendLayout();
        SuspendLayout();

        layout.ColumnCount = 1;
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout.Controls.Add(titleLabel, 0, 0);
        layout.Controls.Add(connectionLabel, 0, 1);
        layout.Controls.Add(checkButton, 0, 2);
        layout.Controls.Add(createTablesButton, 0, 3);
        layout.Controls.Add(seedButton, 0, 4);
        layout.Controls.Add(statusLabel, 0, 5);
        layout.Controls.Add(backButton, 0, 6);
        layout.Dock = DockStyle.Fill;
        layout.Padding = new Padding(28);
        layout.RowCount = 8;
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 62F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        titleLabel.Dock = DockStyle.Fill;
        titleLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        titleLabel.Text = "База данных";
        titleLabel.TextAlign = ContentAlignment.MiddleCenter;

        connectionLabel.Dock = DockStyle.Fill;
        connectionLabel.ForeColor = Color.DimGray;
        connectionLabel.Text = "Подключение: localhost, база supplier_orders, пользователь postgres";
        connectionLabel.TextAlign = ContentAlignment.MiddleCenter;

        checkButton.Dock = DockStyle.Fill;
        checkButton.Text = "Проверить подключение";
        checkButton.UseVisualStyleBackColor = true;
        checkButton.Click += CheckButton_Click;

        createTablesButton.Dock = DockStyle.Fill;
        createTablesButton.Text = "Создать таблицы";
        createTablesButton.UseVisualStyleBackColor = true;
        createTablesButton.Click += CreateTablesButton_Click;

        seedButton.Dock = DockStyle.Fill;
        seedButton.Text = "Заполнить примером";
        seedButton.UseVisualStyleBackColor = true;
        seedButton.Click += SeedButton_Click;

        statusLabel.Dock = DockStyle.Fill;
        statusLabel.ForeColor = Color.DimGray;
        statusLabel.Text = "Готово к работе";
        statusLabel.TextAlign = ContentAlignment.MiddleCenter;

        backButton.Dock = DockStyle.Fill;
        backButton.Text = "Назад";
        backButton.UseVisualStyleBackColor = true;
        backButton.Click += BackButton_Click;

        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(640, 470);
        Controls.Add(layout);
        Font = new Font("Segoe UI", 10F);
        MinimumSize = new Size(560, 430);
        Name = "DatabaseSetupForm";
        Text = "База данных";
        layout.ResumeLayout(false);
        ResumeLayout(false);
    }
}
