namespace SupplierOrdersApp;

partial class MaterialReportForm
{
    private System.ComponentModel.IContainer components = null;
    private TableLayoutPanel layout;
    private FlowLayoutPanel toolbar;
    private DateTimePicker fromPicker;
    private DateTimePicker toPicker;
    private Button buildButton;
    private Button exportButton;
    private Button backButton;
    private DataGridView materialsGrid;
    private Label statusLabel;

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
        layout = new TableLayoutPanel();
        toolbar = new FlowLayoutPanel();
        fromPicker = new DateTimePicker();
        toPicker = new DateTimePicker();
        buildButton = new Button();
        exportButton = new Button();
        backButton = new Button();
        materialsGrid = new DataGridView();
        statusLabel = new Label();
        layout.SuspendLayout();
        toolbar.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)materialsGrid).BeginInit();
        SuspendLayout();

        layout.ColumnCount = 1;
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout.Controls.Add(toolbar, 0, 0);
        layout.Controls.Add(materialsGrid, 0, 1);
        layout.Controls.Add(statusLabel, 0, 2);
        layout.Dock = DockStyle.Fill;
        layout.Padding = new Padding(10);
        layout.RowCount = 3;
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));

        toolbar.Dock = DockStyle.Fill;
        toolbar.WrapContents = false;
        toolbar.Controls.Add(new Label { Text = "С", AutoSize = true, Padding = new Padding(0, 8, 0, 0) });
        fromPicker.Format = DateTimePickerFormat.Short;
        fromPicker.Value = DateTime.Today.AddMonths(-2);
        fromPicker.Width = 125;
        toolbar.Controls.Add(fromPicker);
        toolbar.Controls.Add(new Label { Text = "по", AutoSize = true, Padding = new Padding(0, 8, 0, 0) });
        toPicker.Format = DateTimePickerFormat.Short;
        toPicker.Value = DateTime.Today;
        toPicker.Width = 125;
        toolbar.Controls.Add(toPicker);
        buildButton.Text = "Сформировать";
        buildButton.Width = 135;
        buildButton.Click += BuildButton_Click;
        toolbar.Controls.Add(buildButton);
        exportButton.Text = "Экспорт в Excel";
        exportButton.Width = 140;
        exportButton.Click += ExportButton_Click;
        toolbar.Controls.Add(exportButton);
        backButton.Text = "Назад";
        backButton.Width = 100;
        backButton.Click += BackButton_Click;
        toolbar.Controls.Add(backButton);

        materialsGrid.AllowUserToAddRows = false;
        materialsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        materialsGrid.Dock = DockStyle.Fill;
        materialsGrid.ReadOnly = true;

        statusLabel.Dock = DockStyle.Fill;
        statusLabel.ForeColor = Color.DimGray;
        statusLabel.Text = "Готово";
        statusLabel.TextAlign = ContentAlignment.MiddleLeft;

        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1050, 650);
        Controls.Add(layout);
        Font = new Font("Segoe UI", 10F);
        MinimumSize = new Size(900, 560);
        Name = "MaterialReportForm";
        Text = "Материальный отчет";
        layout.ResumeLayout(false);
        toolbar.ResumeLayout(false);
        toolbar.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)materialsGrid).EndInit();
        ResumeLayout(false);
    }
}
