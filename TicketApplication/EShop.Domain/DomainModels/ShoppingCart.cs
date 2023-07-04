using EShop.Domain.Identity;

namespace EShop.Domain.DomainModels
{
    public class ShoppingCart : BaseEntity
    {
        public string OwnerId { get; set; }

        public virtual EShopApplicationUser Owner { get; set; }

        public virtual ICollection<TicketInShoppingCart> TicketInShoppingCart { get; set; }
    }
}
