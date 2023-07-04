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
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCart> _shoppingCartRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<EmailMessage> _mailRepository;
        private readonly IRepository<TicketInOrder> _ticketInOrderRepository;
        private readonly IUserRepository _userRepository;

        public ShoppingCartService(IRepository<ShoppingCart> shoppingCartRepository, IUserRepository userRepository, IRepository<EmailMessage> mailRepository, IRepository<Order> orderRepository, IRepository<TicketInOrder> ticketInOrderRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _ticketInOrderRepository = ticketInOrderRepository;
            _mailRepository = mailRepository;
        }


        public bool deleteTicketFromShoppingCart(string userId, Guid ticketId)
        {
            if (!string.IsNullOrEmpty(userId) && ticketId != null)
            {
                var loggedInUser = this._userRepository.Get(userId);

                var userShoppingCart = loggedInUser.UserCart;

                var itemToDelete = userShoppingCart.TicketInShoppingCart.Where(z => z.TicketId.Equals(ticketId)).FirstOrDefault();

                userShoppingCart.TicketInShoppingCart.Remove(itemToDelete);

                this._shoppingCartRepository.Update(userShoppingCart);

                return true;
            }
            return false;
        }

        public ShoppingCartDto getShoppingCartInfo(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var loggedInUser = this._userRepository.Get(userId);

                var userCart = loggedInUser.UserCart;

                var allTickets = userCart.TicketInShoppingCart.ToList();

                var allTicketPrices = allTickets.Select(z => new
                {
                    TicketPrice = z.CurrentTicket.TicketPrice,
                    Quantity = z.Quantity
                }).ToList();

                double totalPrice = 0.0;

                foreach (var item in allTicketPrices)
                {
                    totalPrice += item.Quantity * item.TicketPrice;
                }

                var result = new ShoppingCartDto
                {
                    Tickets = allTickets,
                    TotalPrice = totalPrice
                };

                return result;
            }
            return new ShoppingCartDto();
        }

        public bool order(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var loggedInUser = this._userRepository.Get(userId);
                var userCart = loggedInUser.UserCart;

                EmailMessage mail = new EmailMessage();
                mail.MailTo = loggedInUser.Email;
                mail.Subject = "Sucessfuly created order!";
                mail.Status = false;

                Order order = new Order
                {
                    Id = Guid.NewGuid(),
                    User = loggedInUser,
                    UserId = userId
                };

                this._orderRepository.Insert(order);

                List<TicketInOrder> ticketInOrders = new List<TicketInOrder>();

                var result = userCart.TicketInShoppingCart.Select(z => new TicketInOrder
                {
                    Id = Guid.NewGuid(),
                    TicketId = z.CurrentTicket.Id,
                    Ticket = z.CurrentTicket,
                    OrderId = order.Id,
                    Order = order,
                    Quantity = z.Quantity
                }).ToList();

                StringBuilder sb = new StringBuilder();

                var totalPrice = 0.0;

                sb.AppendLine("Your order is completed. The order conatins: ");

                for (int i = 1; i <= result.Count(); i++)
                {
                    var currentItem = result[i - 1];
                    totalPrice += currentItem.Quantity * currentItem.Ticket.TicketPrice;
                    sb.AppendLine(i.ToString() + ". " + currentItem.Ticket.MovieName + " with quantity of: " + currentItem.Quantity + " and price of: " + currentItem.Ticket.TicketPrice + " ДЕН.");
                }

                sb.AppendLine("Total price for your order: " + totalPrice.ToString());

                mail.Content = sb.ToString();

                ticketInOrders.AddRange(result);

                foreach (var item in ticketInOrders)
                {
                    this._ticketInOrderRepository.Insert(item);
                }

                loggedInUser.UserCart.TicketInShoppingCart.Clear();

                this._userRepository.Update(loggedInUser);

                this._mailRepository.Insert(mail);

                return true;
            }

            return false;
        }
    }
}
