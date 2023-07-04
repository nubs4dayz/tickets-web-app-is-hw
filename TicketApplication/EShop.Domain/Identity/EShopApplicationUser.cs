using EShop.Domain.DomainModels;
using Microsoft.AspNetCore.Identity;

namespace EShop.Domain.Identity
{
    public class EShopApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public virtual ShoppingCart UserCart { get; set; }
    }
}
