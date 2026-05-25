using System.ComponentModel;

namespace SupplierOrdersApp;

public partial class OrdersForm : Form
{
    private readonly DatabaseRepository _repository = AppConfig.CreateRepository();
    private readonly BindingList<OrderLineInfo> _newOrderLines = [];
    private readonly BindingSource _ordersSource = new();

    public OrdersForm()
    {
        InitializeComponent();
        AppTheme.ApplyBackground(this);
        newLinesGrid.DataSource = _newOrderLines;
        ordersGrid.DataSource = _ordersSource;
    }

    private async void OrdersForm_Load(object sender, EventArgs e) =>
        await RunSafelyAsync(async () =>
        {
            await LoadLookupsAsync();
            await LoadOrdersAsync();
        });

    private async Task LoadLookupsAsync()
    {
        supplierComboBox.DataSource = (await _repository.GetSuppliersAsync()).ToList();
        supplierComboBox.DisplayMember = nameof(SupplierInfo.Name);
        supplierComboBox.ValueMember = nameof(SupplierInfo.Id);

        productComboBox.DataSource = (await _repository.GetProductsAsync()).ToList();
        productComboBox.DisplayMember = nameof(ProductInfo.Name);
        productComboBox.ValueMember = nameof(ProductInfo.Id);
    }

    private void AddLineButton_Click(object sender, EventArgs e)
    {
        if (productComboBox.SelectedItem is not ProductInfo product)
        {
            MessageBox.Show(this, "Сначала создайте таблицы и заполните справочники.", "Товары", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var quantity = (int)quantityInput.Value;
        var price = priceInput.Value;
        _newOrderLines.Add(new OrderLineInfo(product.Id, product.Name, price, quantity, price * quantity));
    }

    private async void SaveOrderButton_Click(object sender, EventArgs e) =>
        await RunSafelyAsync(async () =>
        {
            if (supplierComboBox.SelectedItem is not SupplierInfo supplier)
            {
                throw new InvalidOperationException("Выберите поставщика.");
            }

            if (_newOrderLines.Count == 0)
            {
                throw new InvalidOperationException("Добавьте хотя бы один товар.");
            }

            await _repository.CreateOrderAsync(supplier.Id, orderDatePicker.Value, confirmedCheckBox.Checked, _newOrderLines.ToList());
            _newOrderLines.Clear();
            await LoadOrdersAsync();
            statusLabel.Text = "Заказ сохранен";
        });

    private async void LoadOrdersButton_Click(object sender, EventArgs e) =>
        await RunSafelyAsync(LoadOrdersAsync);

    private async Task LoadOrdersAsync()
    {
        _ordersSource.DataSource = await _repository.GetOrdersAsync(ordersFromPicker.Value, ordersToPicker.Value);
    }

    private async void ConfirmButton_Click(object sender, EventArgs e) =>
        await RunSafelyAsync(async () =>
        {
            var order = SelectedOrder();
            await _repository.ConfirmOrderAsync(order.Id, DateTime.Today);
            await LoadOrdersAsync();
            statusLabel.Text = "Заказ подтвержден";
        });

    private async void PayButton_Click(object sender, EventArgs e) =>
        await RunSafelyAsync(async () =>
        {
            var order = SelectedOrder();
            if (paymentAmountInput.Value <= 0)
            {
                paymentAmountInput.Value = Math.Max(1, order.TotalAmount - order.PaidAmount);
            }

            await _repository.AddPaymentAsync(order.Id, paymentDatePicker.Value, paymentAmountInput.Value);
            await LoadOrdersAsync();
            statusLabel.Text = "Оплата внесена";
        });

    private OrderInfo SelectedOrder()
    {
        if (_ordersSource.Current is not OrderInfo order)
        {
            throw new InvalidOperationException("Выберите заказ в таблице.");
        }

        return order;
    }

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
        addLineButton.Enabled = enabled;
        saveOrderButton.Enabled = enabled;
        loadOrdersButton.Enabled = enabled;
        confirmButton.Enabled = enabled;
        payButton.Enabled = enabled;
        backButton.Enabled = enabled;
    }
}
