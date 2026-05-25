namespace SupplierOrdersApp;

partial class OrdersForm
{
    private System.ComponentModel.IContainer components = null;
    private SplitContainer splitContainer;
    private TableLayoutPanel leftLayout;
    private TableLayoutPanel rightLayout;
    private ComboBox supplierComboBox;
    private ComboBox productComboBox;
    private DateTimePicker orderDatePicker;
    private NumericUpDown quantityInput;
    private NumericUpDown priceInput;
    private CheckBox confirmedCheckBox;
    private Button addLineButton;
    private Button saveOrderButton;
    private DataGridView newLinesGrid;
    private DateTimePicker ordersFromPicker;
    private DateTimePicker ordersToPicker;
    private Button loadOrdersButton;
    private DataGridView ordersGrid;
    private Button confirmButton;
    private DateTimePicker paymentDatePicker;
    private NumericUpDown paymentAmountInput;
    private Button payButton;
    private Button backButton;
    private Label statusLabel;
    private FlowLayoutPanel filterPanel;
    private FlowLayoutPanel actionsPanel;

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
        leftLayout = new TableLayoutPanel();
        supplierComboBox = new ComboBox();
        orderDatePicker = new DateTimePicker();
        productComboBox = new ComboBox();
        quantityInput = new NumericUpDown();
        priceInput = new NumericUpDown();
        confirmedCheckBox = new CheckBox();
        addLineButton = new Button();
        newLinesGrid = new DataGridView();
        saveOrderButton = new Button();
        backButton = new Button();
        rightLayout = new TableLayoutPanel();
        filterPanel = new FlowLayoutPanel();
        ordersFromPicker = new DateTimePicker();
        ordersToPicker = new DateTimePicker();
        loadOrdersButton = new Button();
        ordersGrid = new DataGridView();
        actionsPanel = new FlowLayoutPanel();
        confirmButton = new Button();
        paymentDatePicker = new DateTimePicker();
        paymentAmountInput = new NumericUpDown();
        payButton = new Button();
        statusLabel = new Label();
        ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
        splitContainer.Panel1.SuspendLayout();
        splitContainer.Panel2.SuspendLayout();
        splitContainer.SuspendLayout();
        leftLayout.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)quantityInput).BeginInit();
        ((System.ComponentModel.ISupportInitialize)priceInput).BeginInit();
        ((System.ComponentModel.ISupportInitialize)newLinesGrid).BeginInit();
        rightLayout.SuspendLayout();
        filterPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)ordersGrid).BeginInit();
        actionsPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)paymentAmountInput).BeginInit();
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
        splitContainer.Panel1.Controls.Add(leftLayout);
        // 
        // splitContainer.Panel2
        // 
        splitContainer.Panel2.Controls.Add(rightLayout);
        splitContainer.Size = new Size(1182, 753);
        splitContainer.SplitterDistance = 953;
        splitContainer.TabIndex = 0;
        // 
        // leftLayout
        // 
        leftLayout.ColumnCount = 2;
        leftLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 145F));
        leftLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        leftLayout.Controls.Add(supplierComboBox, 1, 0);
        leftLayout.Controls.Add(orderDatePicker, 1, 1);
        leftLayout.Controls.Add(productComboBox, 1, 2);
        leftLayout.Controls.Add(quantityInput, 1, 3);
        leftLayout.Controls.Add(priceInput, 1, 4);
        leftLayout.Controls.Add(confirmedCheckBox, 1, 5);
        leftLayout.Controls.Add(addLineButton, 1, 6);
        leftLayout.Controls.Add(newLinesGrid, 0, 7);
        leftLayout.Controls.Add(saveOrderButton, 0, 8);
        leftLayout.Controls.Add(backButton, 0, 9);
        leftLayout.Dock = DockStyle.Fill;
        leftLayout.Location = new Point(0, 0);
        leftLayout.Name = "leftLayout";
        leftLayout.Padding = new Padding(10);
        leftLayout.RowCount = 10;
        leftLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        leftLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        leftLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        leftLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        leftLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        leftLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        leftLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        leftLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        leftLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
        leftLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
        leftLayout.Size = new Size(953, 753);
        leftLayout.TabIndex = 0;
        // 
        // supplierComboBox
        // 
        supplierComboBox.Dock = DockStyle.Fill;
        supplierComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        supplierComboBox.Location = new Point(158, 13);
        supplierComboBox.Name = "supplierComboBox";
        supplierComboBox.Size = new Size(782, 25);
        supplierComboBox.TabIndex = 0;
        // 
        // orderDatePicker
        // 
        orderDatePicker.Dock = DockStyle.Fill;
        orderDatePicker.Format = DateTimePickerFormat.Short;
        orderDatePicker.Location = new Point(158, 49);
        orderDatePicker.Name = "orderDatePicker";
        orderDatePicker.Size = new Size(782, 25);
        orderDatePicker.TabIndex = 1;
        // 
        // productComboBox
        // 
        productComboBox.Dock = DockStyle.Fill;
        productComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        productComboBox.Location = new Point(158, 85);
        productComboBox.Name = "productComboBox";
        productComboBox.Size = new Size(782, 25);
        productComboBox.TabIndex = 2;
        // 
        // quantityInput
        // 
        quantityInput.Dock = DockStyle.Fill;
        quantityInput.Location = new Point(158, 121);
        quantityInput.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
        quantityInput.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        quantityInput.Name = "quantityInput";
        quantityInput.Size = new Size(782, 25);
        quantityInput.TabIndex = 3;
        quantityInput.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // priceInput
        // 
        priceInput.DecimalPlaces = 2;
        priceInput.Dock = DockStyle.Fill;
        priceInput.Increment = new decimal(new int[] { 10, 0, 0, 0 });
        priceInput.Location = new Point(158, 157);
        priceInput.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
        priceInput.Name = "priceInput";
        priceInput.Size = new Size(782, 25);
        priceInput.TabIndex = 4;
        priceInput.Value = new decimal(new int[] { 100, 0, 0, 0 });
        // 
        // confirmedCheckBox
        // 
        confirmedCheckBox.Checked = true;
        confirmedCheckBox.CheckState = CheckState.Checked;
        confirmedCheckBox.Dock = DockStyle.Fill;
        confirmedCheckBox.Location = new Point(158, 193);
        confirmedCheckBox.Name = "confirmedCheckBox";
        confirmedCheckBox.Size = new Size(782, 30);
        confirmedCheckBox.TabIndex = 5;
        confirmedCheckBox.Text = "Подтвержден поставщиком";
        // 
        // addLineButton
        // 
        addLineButton.Dock = DockStyle.Fill;
        addLineButton.Location = new Point(158, 229);
        addLineButton.Name = "addLineButton";
        addLineButton.Size = new Size(782, 36);
        addLineButton.TabIndex = 6;
        addLineButton.Text = "Добавить товар";
        addLineButton.Click += AddLineButton_Click;
        // 
        // newLinesGrid
        // 
        newLinesGrid.AllowUserToAddRows = false;
        newLinesGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        leftLayout.SetColumnSpan(newLinesGrid, 2);
        newLinesGrid.Dock = DockStyle.Fill;
        newLinesGrid.Location = new Point(13, 271);
        newLinesGrid.Name = "newLinesGrid";
        newLinesGrid.ReadOnly = true;
        newLinesGrid.Size = new Size(927, 377);
        newLinesGrid.TabIndex = 7;
        // 
        // saveOrderButton
        // 
        leftLayout.SetColumnSpan(saveOrderButton, 2);
        saveOrderButton.Dock = DockStyle.Fill;
        saveOrderButton.Location = new Point(13, 654);
        saveOrderButton.Name = "saveOrderButton";
        saveOrderButton.Size = new Size(927, 40);
        saveOrderButton.TabIndex = 8;
        saveOrderButton.Text = "Сохранить заказ";
        saveOrderButton.Click += SaveOrderButton_Click;
        // 
        // backButton
        // 
        leftLayout.SetColumnSpan(backButton, 2);
        backButton.Dock = DockStyle.Fill;
        backButton.Location = new Point(13, 700);
        backButton.Name = "backButton";
        backButton.Size = new Size(927, 40);
        backButton.TabIndex = 9;
        backButton.Text = "Назад";
        backButton.Click += BackButton_Click;
        // 
        // rightLayout
        // 
        rightLayout.ColumnCount = 1;
        rightLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        rightLayout.Controls.Add(filterPanel, 0, 0);
        rightLayout.Controls.Add(ordersGrid, 0, 1);
        rightLayout.Controls.Add(actionsPanel, 0, 2);
        rightLayout.Controls.Add(statusLabel, 0, 3);
        rightLayout.Dock = DockStyle.Fill;
        rightLayout.Location = new Point(0, 0);
        rightLayout.Name = "rightLayout";
        rightLayout.Padding = new Padding(10);
        rightLayout.RowCount = 4;
        rightLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
        rightLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        rightLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
        rightLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        rightLayout.Size = new Size(225, 753);
        rightLayout.TabIndex = 0;
        // 
        // filterPanel
        // 
        filterPanel.Controls.Add(ordersFromPicker);
        filterPanel.Controls.Add(ordersToPicker);
        filterPanel.Controls.Add(loadOrdersButton);
        filterPanel.Dock = DockStyle.Fill;
        filterPanel.Location = new Point(13, 13);
        filterPanel.Name = "filterPanel";
        filterPanel.Size = new Size(199, 39);
        filterPanel.TabIndex = 0;
        filterPanel.WrapContents = false;
        // 
        // ordersFromPicker
        // 
        ordersFromPicker.Format = DateTimePickerFormat.Short;
        ordersFromPicker.Location = new Point(3, 3);
        ordersFromPicker.Name = "ordersFromPicker";
        ordersFromPicker.Size = new Size(120, 25);
        ordersFromPicker.TabIndex = 1;
        ordersFromPicker.Value = new DateTime(2026, 3, 24, 0, 0, 0, 0);
        // 
        // ordersToPicker
        // 
        ordersToPicker.Format = DateTimePickerFormat.Short;
        ordersToPicker.Location = new Point(129, 3);
        ordersToPicker.Name = "ordersToPicker";
        ordersToPicker.Size = new Size(120, 25);
        ordersToPicker.TabIndex = 3;
        ordersToPicker.Value = new DateTime(2026, 5, 24, 0, 0, 0, 0);
        // 
        // loadOrdersButton
        // 
        loadOrdersButton.Location = new Point(255, 3);
        loadOrdersButton.Name = "loadOrdersButton";
        loadOrdersButton.Size = new Size(115, 23);
        loadOrdersButton.TabIndex = 4;
        loadOrdersButton.Text = "Показать";
        loadOrdersButton.Click += LoadOrdersButton_Click;
        // 
        // ordersGrid
        // 
        ordersGrid.AllowUserToAddRows = false;
        ordersGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        ordersGrid.Dock = DockStyle.Fill;
        ordersGrid.Location = new Point(13, 58);
        ordersGrid.Name = "ordersGrid";
        ordersGrid.ReadOnly = true;
        ordersGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        ordersGrid.Size = new Size(199, 598);
        ordersGrid.TabIndex = 1;
        // 
        // actionsPanel
        // 
        actionsPanel.Controls.Add(confirmButton);
        actionsPanel.Controls.Add(paymentDatePicker);
        actionsPanel.Controls.Add(paymentAmountInput);
        actionsPanel.Controls.Add(payButton);
        actionsPanel.Dock = DockStyle.Fill;
        actionsPanel.Location = new Point(13, 662);
        actionsPanel.Name = "actionsPanel";
        actionsPanel.Size = new Size(199, 42);
        actionsPanel.TabIndex = 2;
        actionsPanel.WrapContents = false;
        // 
        // confirmButton
        // 
        confirmButton.Location = new Point(3, 3);
        confirmButton.Name = "confirmButton";
        confirmButton.Size = new Size(130, 23);
        confirmButton.TabIndex = 0;
        confirmButton.Text = "Подтвердить";
        confirmButton.Click += ConfirmButton_Click;
        // 
        // paymentDatePicker
        // 
        paymentDatePicker.Format = DateTimePickerFormat.Short;
        paymentDatePicker.Location = new Point(139, 3);
        paymentDatePicker.Name = "paymentDatePicker";
        paymentDatePicker.Size = new Size(120, 25);
        paymentDatePicker.TabIndex = 2;
        // 
        // paymentAmountInput
        // 
        paymentAmountInput.DecimalPlaces = 2;
        paymentAmountInput.Location = new Point(265, 3);
        paymentAmountInput.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
        paymentAmountInput.Name = "paymentAmountInput";
        paymentAmountInput.Size = new Size(130, 25);
        paymentAmountInput.TabIndex = 4;
        // 
        // payButton
        // 
        payButton.Location = new Point(401, 3);
        payButton.Name = "payButton";
        payButton.Size = new Size(100, 23);
        payButton.TabIndex = 5;
        payButton.Text = "Оплатить";
        payButton.Click += PayButton_Click;
        // 
        // statusLabel
        // 
        statusLabel.Dock = DockStyle.Fill;
        statusLabel.ForeColor = Color.DimGray;
        statusLabel.Location = new Point(13, 707);
        statusLabel.Name = "statusLabel";
        statusLabel.Size = new Size(199, 36);
        statusLabel.TabIndex = 3;
        statusLabel.Text = "Готово";
        statusLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // OrdersForm
        // 
        AutoScaleDimensions = new SizeF(7F, 17F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1182, 753);
        Controls.Add(splitContainer);
        Font = new Font("Segoe UI", 10F);
        MinimumSize = new Size(1050, 680);
        Name = "OrdersForm";
        Text = "Заказы и оплаты";
        Load += OrdersForm_Load;
        splitContainer.Panel1.ResumeLayout(false);
        splitContainer.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
        splitContainer.ResumeLayout(false);
        leftLayout.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)quantityInput).EndInit();
        ((System.ComponentModel.ISupportInitialize)priceInput).EndInit();
        ((System.ComponentModel.ISupportInitialize)newLinesGrid).EndInit();
        rightLayout.ResumeLayout(false);
        filterPanel.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)ordersGrid).EndInit();
        actionsPanel.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)paymentAmountInput).EndInit();
        ResumeLayout(false);
    }

    private static void AddLabel(TableLayoutPanel panel, string text, int row)
    {
        panel.Controls.Add(new Label
        {
            Text = text,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft
        }, 0, row);
    }
}
