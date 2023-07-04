using System.ComponentModel.DataAnnotations;

namespace EShop.Domain.DomainModels
{
    public class Ticket : BaseEntity
    {
        [Required]
        public string MovieName { get; set; }

        [Required]
        public string MoviePoster { get; set; }

        [Required]
        public string MovieDescription { get; set; }

        [Required]
        public string MovieGenre { get; set; }

        [Required]
        public double TicketPrice { get; set; }

        public DateTime DateTime { get; set; }

        public virtual ICollection<TicketInShoppingCart>? TicketInShoppingCart { get; set; }

        public virtual ICollection<TicketInOrder>? TicketInOrders { get; set; }
    }
}
