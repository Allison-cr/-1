using System.Globalization;
using System.Drawing.Drawing2D;

namespace SupplierOrdersApp;

public sealed class PieChartPanel : Panel
{
    private decimal _ordersAmount;
    private decimal _refusalsAmount;

    public PieChartPanel()
    {
        DoubleBuffered = true;
        BackColor = Color.White;
    }

    public void SetValues(decimal ordersAmount, decimal refusalsAmount)
    {
        _ordersAmount = Math.Max(0, ordersAmount);
        _refusalsAmount = Math.Max(0, refusalsAmount);
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        e.Graphics.Clear(Color.White);

        var total = _ordersAmount + _refusalsAmount;
        using var textBrush = new SolidBrush(Color.FromArgb(45, 45, 45));
        using var font = new Font(Font.FontFamily, 10f);

        if (total <= 0)
        {
            TextRenderer.DrawText(e.Graphics, "Нет данных за выбранный период", Font, ClientRectangle, Color.DimGray, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            return;
        }

        var diameter = Math.Min(Width - 220, Height - 40);
        diameter = Math.Max(120, diameter);
        var pieRect = new Rectangle(24, Math.Max(20, (Height - diameter) / 2), diameter, diameter);

        var refusalAngle = (float)(_refusalsAmount / total * 360m);
        var orderAngle = 360f - refusalAngle;

        using var orderBrush = new SolidBrush(Color.FromArgb(58, 134, 255));
        using var refusalBrush = new SolidBrush(Color.FromArgb(230, 57, 70));
        e.Graphics.FillPie(orderBrush, pieRect, -90, orderAngle);
        e.Graphics.FillPie(refusalBrush, pieRect, -90 + orderAngle, refusalAngle);
        using var outlinePen = new Pen(Color.White, 2f);
        e.Graphics.DrawEllipse(outlinePen, pieRect);

        var legendX = pieRect.Right + 28;
        var legendY = pieRect.Top + 20;
        DrawLegend(e.Graphics, orderBrush, "Суммы заказов", _ordersAmount, total, legendX, legendY);
        DrawLegend(e.Graphics, refusalBrush, "Отказы / просрочка", _refusalsAmount, total, legendX, legendY + 56);
    }

    private void DrawLegend(Graphics graphics, Brush brush, string title, decimal value, decimal total, int x, int y)
    {
        graphics.FillRectangle(brush, x, y + 4, 16, 16);
        using var titleFont = new Font(Font.FontFamily, 9.5f, FontStyle.Bold);
        using var valueFont = new Font(Font.FontFamily, 9f);
        graphics.DrawString(title, titleFont, Brushes.Black, x + 24, y);

        var percent = total == 0 ? 0 : value / total;
        var text = $"{value:N2} руб. ({percent.ToString("P1", CultureInfo.CurrentCulture)})";
        graphics.DrawString(text, valueFont, Brushes.DimGray, x + 24, y + 24);
    }
}
