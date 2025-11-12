using ClosedXML.Excel;

public class ExcelService
{
    public byte[] ExportToExcel(IEnumerable<StaffDto> rows)
    {
        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Staffs");
        ws.Cell(1, 1).Value = "StaffId";
        ws.Cell(1, 2).Value = "FullName";
        ws.Cell(1, 3).Value = "Birthday";
        ws.Cell(1, 4).Value = "Gender";

        var rowNumber = 2;
        foreach (var s in rows)
        {
            ws.Cell(rowNumber, 1).Value = s.StaffId;
            ws.Cell(rowNumber, 2).Value = s.FullName;
            ws.Cell(rowNumber, 3).Value = s.Birthday.ToString("yyyy-MM-dd");
            ws.Cell(rowNumber, 4).Value = s.Gender;
            rowNumber++;
        }

        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        return ms.ToArray();
    }
}