namespace SupplierOrdersApp;

partial class RatioReportForm
{
    private System.ComponentModel.IContainer components = null;
    private SplitContainer splitContainer;
    private TableLayoutPanel filterLayout;
    private TableLayoutPanel resultLayout;
    private DateTimePicker fromPicker;
    private DateTimePicker toPicker;
    private CheckedListBox supplierList;
    private Button buildButton;
    private Button backButton;
    private PieChartPanel pieChart;
    private DataGridView ratiosGrid;
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
        splitContainer = new SplitContainer();
        filterLayout = new TableLayoutPanel();
        fromPicker = new DateTimePicker();
        toPicker = new DateTimePicker();
        supplierList = new CheckedListBox();
        buildButton = new Button();
        backButton = new Button();
        resultLayout = new TableLayoutPanel();
        pieChart = new PieChartPanel();
        ratiosGrid = new DataGridView();
        statusLabel = new Label();
        ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
        splitContainer.Panel1.SuspendLayout();
        splitContainer.Panel2.SuspendLayout();
        splitContainer.SuspendLayout();
        filterLayout.SuspendLayout();
        resultLayout.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)ratiosGrid).BeginInit();
        SuspendLayout();
        // 
        // splitContainer
        // 
        splitContainer.Dock = DockStyle.Fill;
        splitContainer.Location = new Point(0, 0);
        splitContainer.Name = "splitContainer";
        // 
        // splitContainer.Panel1
        // 
        splitContainer.Panel1.Controls.Add(filterLayout);
        // 
        // splitContainer.Panel2
        // 
        splitContainer.Panel2.Controls.Add(resultLayout);
        splitContainer.Size = new Size(1050, 650);
        splitContainer.SplitterDistance = 846;
        splitContainer.TabIndex = 0;
        // 
        // filterLayout
        // 
        filterLayout.ColumnCount = 1;
        filterLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        filterLayout.Controls.Add(fromPicker, 0, 1);
        filterLayout.Controls.Add(toPicker, 0, 3);
        filterLayout.Controls.Add(supplierList, 0, 5);
        filterLayout.Controls.Add(buildButton, 0, 6);
        filterLayout.Controls.Add(backButton, 0, 7);
        filterLayout.Dock = DockStyle.Fill;
        filterLayout.Location = new Point(0, 0);
        filterLayout.Name = "filterLayout";
        filterLayout.Padding = new Padding(10);
        filterLayout.RowCount = 8;
        filterLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        filterLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        filterLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        filterLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        filterLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
        filterLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        filterLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
        filterLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
        filterLayout.Size = new Size(846, 650);
        filterLayout.TabIndex = 0;
        // 
        // fromPicker
        // 
        fromPicker.Dock = DockStyle.Fill;
        fromPicker.Format = DateTimePickerFormat.Short;
        fromPicker.Location = new Point(13, 45);
        fromPicker.Name = "fromPicker";
        fromPicker.Size = new Size(820, 25);
        fromPicker.TabIndex = 1;
        fromPicker.Value = new DateTime(2026, 3, 24, 0, 0, 0, 0);
        // 
        // toPicker
        // 
        toPicker.Dock = DockStyle.Fill;
        toPicker.Format = DateTimePickerFormat.Short;
        toPicker.Location = new Point(13, 115);
        toPicker.Name = "toPicker";
        toPicker.Size = new Size(820, 25);
        toPicker.TabIndex = 3;
        toPicker.Value = new DateTime(2026, 5, 24, 0, 0, 0, 0);
        // 
        // supplierList
        // 
        supplierList.CheckOnClick = true;
        supplierList.Dock = DockStyle.Fill;
        supplierList.Location = new Point(13, 187);
        supplierList.Name = "supplierList";
        supplierList.Size = new Size(820, 358);
        supplierList.TabIndex = 5;
        // 
        // buildButton
        // 
        buildButton.Dock = DockStyle.Fill;
        buildButton.Location = new Point(13, 551);
        buildButton.Name = "buildButton";
        buildButton.Size = new Size(820, 40);
        buildButton.TabIndex = 6;
        buildButton.Text = "Сформировать";
        buildButton.Click += BuildButton_Click;
        // 
        // backButton
        // 
        backButton.Dock = DockStyle.Fill;
        backButton.Location = new Point(13, 597);
        backButton.Name = "backButton";
        backButton.Size = new Size(820, 40);
        backButton.TabIndex = 7;
        backButton.Text = "Назад";
        backButton.Click += BackButton_Click;
        // 
        // resultLayout
        // 
        resultLayout.ColumnCount = 1;
        resultLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        resultLayout.Controls.Add(pieChart, 0, 0);
        resultLayout.Controls.Add(ratiosGrid, 0, 1);
        resultLayout.Controls.Add(statusLabel, 0, 2);
        resultLayout.Dock = DockStyle.Fill;
        resultLayout.Location = new Point(0, 0);
        resultLayout.Name = "resultLayout";
        resultLayout.Padding = new Padding(10);
        resultLayout.RowCount = 3;
        resultLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 58F));
        resultLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 42F));
        resultLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
        resultLayout.Size = new Size(200, 650);
        resultLayout.TabIndex = 0;
        // 
        // pieChart
        // 
        pieChart.BackColor = Color.White;
        pieChart.Dock = DockStyle.Fill;
        pieChart.Location = new Point(13, 13);
        pieChart.Name = "pieChart";
        pieChart.Size = new Size(174, 339);
        pieChart.TabIndex = 0;
        // 
        // ratiosGrid
        // 
        ratiosGrid.AllowUserToAddRows = false;
        ratiosGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        ratiosGrid.Dock = DockStyle.Fill;
        ratiosGrid.Location = new Point(13, 358);
        ratiosGrid.Name = "ratiosGrid";
        ratiosGrid.ReadOnly = true;
        ratiosGrid.Size = new Size(174, 244);
        ratiosGrid.TabIndex = 1;
        // 
        // statusLabel
        // 
        statusLabel.Dock = DockStyle.Fill;
        statusLabel.ForeColor = Color.DimGray;
        statusLabel.Location = new Point(13, 605);
        statusLabel.Name = "statusLabel";
        statusLabel.Size = new Size(174, 35);
        statusLabel.TabIndex = 2;
        statusLabel.Text = "Готово";
        statusLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // RatioReportForm
        // 
        AutoScaleDimensions = new SizeF(7F, 17F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1050, 650);
        Controls.Add(splitContainer);
        Font = new Font("Segoe UI", 10F);
        MinimumSize = new Size(920, 560);
        Name = "RatioReportForm";
        Text = "Заказы и отказы";
        Load += RatioReportForm_Load;
        splitContainer.Panel1.ResumeLayout(false);
        splitContainer.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
        splitContainer.ResumeLayout(false);
        filterLayout.ResumeLayout(false);
        resultLayout.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)ratiosGrid).EndInit();
        ResumeLayout(false);
    }
}
