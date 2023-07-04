using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using EShop.Domain.DomainModels;
using EShop.Domain.DTO;
using EShop.Repository;
using EShop.Service.Interface;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;

namespace EShopWeb.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ITicketService _ticketService;
        private readonly ILogger<TicketsController> _logger;

        public TicketsController(ILogger<TicketsController> logger, ITicketService ticketService)
        {
            _logger = logger;
            _ticketService = ticketService;
        }

        // GET: Tickets
        public IActionResult Index()
        {
            _logger.LogInformation("User Request -> Get All tickets!");

            return View(this._ticketService.GetAllTickets());
        }

        // GET: Tickets/Details/5
        public IActionResult Details(Guid? id)
        {
            _logger.LogInformation("User Request -> Get Details For Ticket");

            if (id == null)
            {
                return NotFound();
            }

            var ticket = this._ticketService.GetDetailsForTicket(id);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            _logger.LogInformation("User Request -> Get create form for Ticket!");

            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,MovieName,MoviePoster,MovieDescription,MovieGenre,TicketPrice,DateTime")] Ticket ticket)
        {
            _logger.LogInformation("User Request -> Insert Ticket in DataBase!");

            if (ModelState.IsValid)
            {
                ticket.Id = Guid.NewGuid();
                this._ticketService.CreateNewTicket(ticket);
                return RedirectToAction(nameof(Index));
            }

            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(Guid? id)
        {
            _logger.LogInformation("User Request -> Get edit form for Ticket!");
            if (id == null)
            {
                return NotFound();
            }

            var ticket = this._ticketService.GetDetailsForTicket(id);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,MovieName,MoviePoster,MovieDescription,MovieGenre,TicketPrice,DateTime")] Ticket ticket)
        {
            _logger.LogInformation("User Request -> Update Ticket in DataBase!");

            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    this._ticketService.UpdateExistingTicket(ticket);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(Guid? id)
        {
            _logger.LogInformation("User Request -> Get delete form for Ticket!");

            if (id == null)
            {
                return NotFound();
            }

            var ticket = this._ticketService.GetDetailsForTicket(id);

            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _logger.LogInformation("User Request -> Delete Ticket in DataBase!");

            this._ticketService.DeleteTicket(id);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult AddTicketToCart(Guid id)
        {
            var result = this._ticketService.GetShoppingCartInfo(id);

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddTicketToCart(AddToShoppingCartDto model)
        {
            _logger.LogInformation("User Request -> Add Ticket in ShoppingCart and save changes in DataBase!");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = this._ticketService.AddToShoppingCart(model, userId);

            if (result)
            {
                return RedirectToAction("Index", "Tickets");
            }

            return View(model);
        }

        private bool TicketExists(Guid id)
        {
            return this._ticketService.GetDetailsForTicket(id) != null;
        }

        [Authorize(Roles ="Administrator")]
        public FileContentResult ExportTickets(string? genre)
        {
            HttpClient client = new HttpClient();

            string URL = "https://localhost:7299/api/Admin/GetTickets";

            HttpResponseMessage response = client.GetAsync(URL).Result;

            var result = response.Content.ReadAsAsync<List<Ticket>>().Result;

            List<Ticket> filtered = result.Where(ticket => ticket.MovieGenre.Equals(genre)).ToList();

            string fileName = "Tickets.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workBook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workBook.Worksheets.Add("All Tickets");

                worksheet.Cell(1, 1).Value = "Ticket Date";
                worksheet.Cell(1, 2).Value = "Movie Name";
                worksheet.Cell(1, 3).Value = "Movie Genre";
                worksheet.Cell(1, 4).Value = "Ticket Price";

                for (int i = 1; i <= filtered.Count(); i++)
                {

                    var item = filtered[i - 1];


                    worksheet.Cell(i + 1, 1).Value = item.DateTime.ToString("dd-MM-yyyy");
                    worksheet.Cell(i + 1, 2).Value = item.MovieName;
                    worksheet.Cell(i + 1, 3).Value = item.MovieGenre;
                    worksheet.Cell(i + 1, 4).Value = item.TicketPrice;

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
