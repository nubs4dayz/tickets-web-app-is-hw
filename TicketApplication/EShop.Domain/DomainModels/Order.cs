using EShop.Domain.Identity;

namespace EShop.Domain.DomainModels
{
    public class Order : BaseEntity
    {

        public string UserId { get; set; }

        public EShopApplicationUser User { get; set; }

        public virtual ICollection<TicketInOrder> TicketInOrders { get; set; }
    }
}
