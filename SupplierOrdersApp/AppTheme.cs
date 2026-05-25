namespace SupplierOrdersApp;

public static class AppTheme
{
    public static void ApplyBackground(Form form)
    {
        var imagePath = Path.Combine(AppContext.BaseDirectory, "image.jpg");
        if (!File.Exists(imagePath))
        {
            return;
        }

        form.BackgroundImage = Image.FromFile(imagePath);
        form.BackgroundImageLayout = ImageLayout.Stretch;
        MakeContainersTransparent(form);
    }

    private static void MakeContainersTransparent(Control parent)
    {
        foreach (Control control in parent.Controls)
        {
            if (control is TableLayoutPanel or FlowLayoutPanel or Panel or SplitContainer)
            {
                TrySetTransparent(control);
            }

            if (control is SplitContainer splitContainer)
            {
                TrySetTransparent(splitContainer.Panel1);
                TrySetTransparent(splitContainer.Panel2);
            }

            MakeContainersTransparent(control);
        }
    }

    private static void TrySetTransparent(Control control)
    {
        try
        {
            control.BackColor = Color.Transparent;
        }
        catch (NotSupportedException)
        {
            control.BackColor = Color.FromArgb(245, 245, 245);
        }
    }
}
