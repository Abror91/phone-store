using Phonix.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonix.DAL.Interfaces
{
    public interface IOrderRepository : IDisposable
    {
        Task<ICollection<Order>> GetOrdersWithUsers();
        Task<Order> GetOrder(int? id);
        Task Add(Order order);
        Task Update(Order order);
        Task Delete(Order order);
        Task DeleteUserOrders(string userId);
    }
}
