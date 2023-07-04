using ClosedXML.Excel;
using EShop.Domain.DomainModels;
using GemBox.Document;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace EShopWeb.Controllers
{
    public class OrderController : Controller
    {

        public OrderController()
        {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }

        public IActionResult Index()
        {
            HttpClient client = new HttpClient();

            string URL = "https://localhost:7299/api/Admin/GetOrders";

            HttpResponseMessage response = client.GetAsync(URL).Result;

            var result = response.Content.ReadAsAsync<List<Order>>().Result;

            return View(result);
        }

        public IActionResult Details(Guid id)
        {
            HttpClient client = new HttpClient();

            string URL = "https://localhost:7299/api/Admin/GetDetailsForOrder";

            var model = new
            {
                Id = id
            };


            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");


            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            var result = response.Content.ReadAsAsync<Order>().Result;

            return View(result);
        }

        public IActionResult CreateInvoice(Guid id)
        {
            HttpClient client = new HttpClient();

            string URL = "https://localhost:7299/api/Admin/GetDetailsForOrder";

            var model = new
            {
                Id = id
            };


            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");


            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            var result = response.Content.ReadAsAsync<Order>().Result;

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.docx");

            var document = DocumentModel.Load(templatePath);

            document.Content.Replace("{{OrderNumber}}", result.Id.ToString());
            document.Content.Replace("{{CostumerEmail}}", result.User.Email);
            document.Content.Replace("{{CostumerInfo}}", (result.User.FirstName + " " + result.User.LastName));

            StringBuilder sb = new StringBuilder();

            var total = 0.0;

            foreach (var item in result.TicketInOrders)
            {
                total += item.Quantity * item.Ticket.TicketPrice;
                sb.AppendLine(item.Ticket.MovieName + " with quantity of: " + item.Quantity + " and price of: " + item.Ticket.TicketPrice + " ДЕН.");
            }

            document.Content.Replace("{{AllTickets}}", sb.ToString());
            document.Content.Replace("{{TotalPrice}}", total.ToString() + " ДЕН.");

            var stream = new MemoryStream();

            document.Save(stream, new PdfSaveOptions());


            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "ExportInvoice.pdf");
        }

        public FileContentResult ExportAllOrders(Guid id)
        {
            HttpClient client = new HttpClient();

            string URL = "https://localhost:7299/api/Admin/GetOrders";

            HttpResponseMessage response = client.GetAsync(URL).Result;

            var result = response.Content.ReadAsAsync<List<Order>>().Result;

            string fileName = "Orders.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workBook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workBook.Worksheets.Add("All Orders");

                worksheet.Cell(1, 1).Value = "Order Id";
                worksheet.Cell(1, 2).Value = "Costumer Name";
                worksheet.Cell(1, 3).Value = "Costumer Last Name";
                worksheet.Cell(1, 4).Value = "Costumer Email";

                for (int i = 1; i <= result.Count(); i++)
                {
                    var item = result[i - 1];

                    worksheet.Cell(i + 1, 1).Value = item.Id.ToString();
                    worksheet.Cell(i + 1, 2).Value = item.User.FirstName;
                    worksheet.Cell(i + 1, 3).Value = item.User.LastName;
                    worksheet.Cell(i + 1, 4).Value = item.User.Email;

                    for (int p = 1; p <= item.TicketInOrders.Count(); p++)
                    {
                        worksheet.Cell(1, p + 4).Value = "Ticket-" + (p + 1);
                        worksheet.Cell(i + 1, p + 4).Value = item.TicketInOrders.ElementAt(p - 1).Ticket.MovieName;
                    }

                }

                using (var stream = new MemoryStream())
                {
                    workBook.SaveAs(stream);

                    var content = stream.ToArray();

                    return File(content, contentType, fileName);
                }
            }

        }
    }
}
