using EShop.Domain.DomainModels;
using EShop.Domain.DTO;
using EShop.Repository.Interface;
using EShop.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Service.Implementation
{
    public class TicketService : ITicketService
    {
        private readonly IRepository<Ticket> _ticketRepository;
        private readonly IRepository<TicketInShoppingCart> _ticketInShoppingCartRepository;
        private readonly IUserRepository _userRepository;

        public TicketService(IRepository<Ticket> ticketRepository, IRepository<TicketInShoppingCart> ticketInShoppingCartRepository, IUserRepository userRepository)
        {
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
            _ticketInShoppingCartRepository = ticketInShoppingCartRepository;
        }


        public bool AddToShoppingCart(AddToShoppingCartDto item, string userID)
        {
            var user = this._userRepository.Get(userID);

            var userShoppingCart = user.UserCart;

            if (item.SelectedTicketId != null && userShoppingCart != null)
            {
                var ticket = this.GetDetailsForTicket(item.SelectedTicketId);

                if (ticket != null)
                {
                    TicketInShoppingCart itemToAdd = new TicketInShoppingCart
                    {
                        Id = Guid.NewGuid(),
                        CurrentTicket = ticket,
                        TicketId = ticket.Id,
                        UserCart = userShoppingCart,
                        ShoppingCartId = userShoppingCart.Id,
                        Quantity = item.Quantity
                    };

                    var existing = userShoppingCart.TicketInShoppingCart.Where(z => z.ShoppingCartId == userShoppingCart.Id && z.TicketId == itemToAdd.TicketId).FirstOrDefault();

                    if (existing != null)
                    {
                        existing.Quantity += itemToAdd.Quantity;
                        this._ticketInShoppingCartRepository.Update(existing);
                    }
                    else
                    {
                        this._ticketInShoppingCartRepository.Insert(itemToAdd);
                    }
                    return true;
                }
                return false;
            }
            return false;
        }

        public void CreateNewTicket(Ticket t)
        {
            this._ticketRepository.Insert(t);
        }

        public void DeleteTicket(Guid id)
        {
            var ticket = this.GetDetailsForTicket(id);
            this._ticketRepository.Delete(ticket);
        }

        public List<Ticket> GetAllTickets()
        {
            return this._ticketRepository.GetAll().ToList();
        }

        public Ticket GetDetailsForTicket(Guid? id)
        {
            return this._ticketRepository.Get(id);
        }

        public AddToShoppingCartDto GetShoppingCartInfo(Guid? id)
        {
            var ticket = this.GetDetailsForTicket(id);
            AddToShoppingCartDto model = new AddToShoppingCartDto
            {
                SelectedTicket = ticket,
                SelectedTicketId = ticket.Id,
                Quantity = 1
            };

            return model;
        }

        public void UpdateExistingTicket(Ticket t)
        {
            this._ticketRepository.Update(t);
        }
    }
}
