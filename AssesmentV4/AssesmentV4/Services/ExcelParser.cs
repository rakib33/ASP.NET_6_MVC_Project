using AssesmentV4.Models;
using OfficeOpenXml;


namespace AssesmentV4.Services
{
    public class ExcelParser
    {
        public List<Product> ParseExcel(string filePath)
        {
            var products = new List<Product>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                if (package.Workbook.Worksheets.Count > 0)
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++) // Start from row 2 to skip header
                    {
                        var product = new Product
                        {
                            Id = int.Parse(worksheet.Cells[row, 1].Text),
                            Image = worksheet.Cells[row, 2].Text,
                            Name = worksheet.Cells[row, 3].Text,
                            //OrderDate = DateTime.Parse(worksheet.Cells[row, 4].Text),

                            // Parse as DateTime with Kind=Unspecified
                           OrderDate = DateTime.SpecifyKind(
                                DateTime.Parse(worksheet.Cells[row, 4].Text),
                                DateTimeKind.Unspecified
                            ),
                            Price = worksheet.Cells[row, 5].Text,
                            DiscountedPrice = worksheet.Cells[row, 6].Text
                        };

                        products.Add(product);
                    }
                }

            }

            return products;
        }
    }
}
