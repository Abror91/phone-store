using Phonix.BLL.DTO;
using Phonix.BLL.Infrastructure;
using Phonix.BLL.Interfaces;
using Phonix.DAL.Entities;
using Phonix.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonix.BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _db;
        public OrderService(IUnitOfWork db)
        {
            _db = db;
        }

        public async Task<IEnumerable<OrderDTO>> GetOrders()
        {
            var orders = await _db.Orders.GetOrdersWithUsers();
            var ordersToReturn = new List<OrderDTO>();
            foreach(var o in orders)
            {
                var order = new OrderDTO
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    PhoneId = o.Phone.Id,
                    PhoneName = o.Phone.Model + " " + o.Phone.CompanyName,
                    UserEmail = o.ApplicationUser.Email
                };
                ordersToReturn.Add(order);
            }
            return ordersToReturn;
        }

        public async Task<OrderDTO> GetOrderById(int? id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));
            var order = await _db.Orders.GetOrder(id);
            if (order == null)
                throw new NullReferenceException();
            var o = new OrderDTO
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                PhoneId = order.Phone.Id,
                PhoneName = order.Phone.Model,
                UserEmail = order.ApplicationUser.Email
            };
            return o;
        }

        public async Task<OperationDetails> MakeOrder(OrderDTO order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));
            if (string.IsNullOrEmpty(order.OrderDate.ToShortDateString()))
                throw new ArgumentNullException(nameof(order.OrderDate));
            if (string.IsNullOrEmpty(order.UserEmail))
                throw new ArgumentNullException(nameof(order.UserEmail));
            if (string.IsNullOrEmpty(order.PhoneId.ToString()))
                throw new ArgumentNullException(nameof(order.PhoneId));
            var user = await _db.UserManager.FindByEmailAsync(order.UserEmail);
            var phone = await _db.Phones.GetPhone(order.PhoneId);
            if(user != null)
            {
                if(phone != null)
                {
                    var orderToSave = new Order
                    {
                        OrderDate = DateTime.Now.Date,
                        PhoneId = phone.Id,
                        ApplicationUserId = user.Id
                    };
                    await _db.Orders.Add(orderToSave);
                    return new OperationDetails(true, "Your order has been successfully saved!", "");
                }
                return new OperationDetails(false, "Error. The phone has not been found!", "");
            }
            return new OperationDetails(false, "Error. IUser was not found!", "");
        }

        public async Task<OperationDetails> UpdateOrder(OrderDTO order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));
            if (string.IsNullOrEmpty(order.OrderDate.ToShortDateString()))
                throw new ArgumentNullException(nameof(order.OrderDate));
            if (string.IsNullOrEmpty(order.UserEmail))
                throw new ArgumentNullException(nameof(order.UserEmail));
            if (string.IsNullOrEmpty(order.PhoneId.ToString()))
                throw new ArgumentNullException(nameof(order.PhoneId));
            var orderToUpdate = await _db.Orders.GetOrder(order.Id);
            if(orderToUpdate != null)
            {
                orderToUpdate.OrderDate = order.OrderDate;
                await _db.Orders.Update(orderToUpdate);
                return new OperationDetails(true, "Successfully updated!", "");
            }
            return new OperationDetails(false, "Error. Unable to find order!", "");
        }

        public async Task<OperationDetails> DeleteOrder(int? orderId)
        {
            if (orderId == null)
                throw new ArgumentNullException(nameof(orderId));
            var order = await _db.Orders.GetOrder(orderId);
            if(order != null)
            {
                await _db.Orders.Delete(order);
                return new OperationDetails(true, "Order has been successfully deleted!", "");
            }
            return new OperationDetails(false, "Order has not been found!", "");
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
