using EShop.Domain.DomainModels;

namespace EShop.Domain.DTO
{
    public class AddToShoppingCartDto
    {
        public Ticket SelectedTicket { get; set; }
        public Guid SelectedTicketId { get; set; }
        public int Quantity { get; set; }
    }
}
