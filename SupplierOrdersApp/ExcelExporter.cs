using System.Globalization;
using System.IO.Compression;
using System.Text;

namespace SupplierOrdersApp;

public static class ExcelExporter
{
    public static void ExportMaterialReport(string path, IEnumerable<MaterialReportRow> rows, DateTime from, DateTime to)
    {
        var table = new List<IReadOnlyList<object?>>
        {
            new object?[] { $"Материальный отчет за {from:dd.MM.yyyy} - {to:dd.MM.yyyy}" },
            new object?[] { "Поставщик", "Товар", "Ед.", "Количество", "Общая сумма", "Оплаченная сумма" }
        };

        table.AddRange(rows.Select(row => new object?[]
        {
            row.Supplier,
            row.Product,
            row.Unit,
            row.Quantity,
            row.TotalAmount,
            row.PaidAmount
        }));

        SaveWorkbook(path, "Материальный отчет", table);
    }

    private static void SaveWorkbook(string path, string sheetName, IReadOnlyList<IReadOnlyList<object?>> rows)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        using var archive = ZipFile.Open(path, ZipArchiveMode.Create);
        AddEntry(archive, "[Content_Types].xml", """
            <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
            <Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types">
              <Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml"/>
              <Default Extension="xml" ContentType="application/xml"/>
              <Override PartName="/xl/workbook.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml"/>
              <Override PartName="/xl/worksheets/sheet1.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml"/>
              <Override PartName="/xl/styles.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml"/>
            </Types>
            """);
        AddEntry(archive, "_rels/.rels", """
            <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
            <Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">
              <Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument" Target="xl/workbook.xml"/>
            </Relationships>
            """);
        AddEntry(archive, "xl/_rels/workbook.xml.rels", """
            <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
            <Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">
              <Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet" Target="worksheets/sheet1.xml"/>
              <Relationship Id="rId2" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles" Target="styles.xml"/>
            </Relationships>
            """);
        AddEntry(archive, "xl/workbook.xml", $$"""
            <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
            <workbook xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main" xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships">
              <sheets>
                <sheet name="{{Escape(sheetName)}}" sheetId="1" r:id="rId1"/>
              </sheets>
            </workbook>
            """);
        AddEntry(archive, "xl/styles.xml", """
            <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
            <styleSheet xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main">
              <fonts count="2"><font/><font><b/></font></fonts>
              <fills count="1"><fill><patternFill patternType="none"/></fill></fills>
              <borders count="1"><border/></borders>
              <cellStyleXfs count="1"><xf numFmtId="0" fontId="0" fillId="0" borderId="0"/></cellStyleXfs>
              <cellXfs count="2"><xf numFmtId="0" fontId="0" fillId="0" borderId="0"/><xf numFmtId="0" fontId="1" fillId="0" borderId="0" applyFont="1"/></cellXfs>
            </styleSheet>
            """);
        AddEntry(archive, "xl/worksheets/sheet1.xml", BuildSheet(rows));
    }

    private static string BuildSheet(IReadOnlyList<IReadOnlyList<object?>> rows)
    {
        var xml = new StringBuilder();
        xml.AppendLine("""<?xml version="1.0" encoding="UTF-8" standalone="yes"?>""");
        xml.AppendLine("""<worksheet xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main">""");
        xml.AppendLine("""<cols><col min="1" max="6" width="18" customWidth="1"/></cols>""");
        xml.AppendLine("<sheetData>");

        for (var rowIndex = 0; rowIndex < rows.Count; rowIndex++)
        {
            xml.Append(CultureInfo.InvariantCulture, $"<row r=\"{rowIndex + 1}\">");
            var row = rows[rowIndex];
            for (var columnIndex = 0; columnIndex < row.Count; columnIndex++)
            {
                var reference = GetCellReference(columnIndex, rowIndex + 1);
                var style = rowIndex <= 1 ? " s=\"1\"" : string.Empty;
                AppendCell(xml, reference, row[columnIndex], style);
            }
            xml.AppendLine("</row>");
        }

        xml.AppendLine("</sheetData>");
        xml.AppendLine("</worksheet>");
        return xml.ToString();
    }

    private static void AppendCell(StringBuilder xml, string reference, object? value, string style)
    {
        if (value is null)
        {
            xml.Append(CultureInfo.InvariantCulture, $"<c r=\"{reference}\"{style}/>");
            return;
        }

        if (value is int or decimal or double)
        {
            xml.Append(CultureInfo.InvariantCulture, $"<c r=\"{reference}\"{style}><v>{Convert.ToString(value, CultureInfo.InvariantCulture)}</v></c>");
            return;
        }

        xml.Append(CultureInfo.InvariantCulture, $"<c r=\"{reference}\" t=\"inlineStr\"{style}><is><t>{Escape(Convert.ToString(value) ?? string.Empty)}</t></is></c>");
    }

    private static string GetCellReference(int zeroBasedColumn, int row)
    {
        var dividend = zeroBasedColumn + 1;
        var columnName = string.Empty;
        while (dividend > 0)
        {
            var modulo = (dividend - 1) % 26;
            columnName = Convert.ToChar('A' + modulo) + columnName;
            dividend = (dividend - modulo) / 26;
        }

        return columnName + row.ToString(CultureInfo.InvariantCulture);
    }

    private static void AddEntry(ZipArchive archive, string name, string content)
    {
        var entry = archive.CreateEntry(name, CompressionLevel.Optimal);
        using var stream = entry.Open();
        using var writer = new StreamWriter(stream, new UTF8Encoding(false));
        writer.Write(content);
    }

    private static string Escape(string value) =>
        value.Replace("&", "&amp;", StringComparison.Ordinal)
            .Replace("<", "&lt;", StringComparison.Ordinal)
            .Replace(">", "&gt;", StringComparison.Ordinal)
            .Replace("\"", "&quot;", StringComparison.Ordinal);
}
