using Phonix.DAL.Entities;
using Phonix.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonix.DAL.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IApplicationDbContext _db;
        private readonly IDbSet<Order> _orders;
        public OrderRepository(IApplicationDbContext db)
        {
            _db = db;
            _orders = db.Set<Order>();
        }

        public async Task<ICollection<Order>> GetOrdersWithUsers()
        {
            return await _orders.Include(s => s.ApplicationUser).ToListAsync();
        }
        public async Task<Order> GetOrder(int? id)
        {
            return await _orders.Where(s => s.Id == id).Include(s => s.ApplicationUser).Include(s => s.Phone).FirstOrDefaultAsync();
        }
        public async Task Add(Order order)
        {
            _orders.Add(order);
            await _db.SaveChangesAsync();
        }
        public async Task Update(Order order)
        {
            ChangeState(order, EntityState.Modified);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(Order order)
        {
            if (!_orders.Local.Contains(order))
                _orders.Attach(order);
            _orders.Remove(order);
            ChangeState(order, EntityState.Deleted);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteUserOrders(string userId)
        {
            var orders = await _orders.Where(s => s.ApplicationUser.Id == userId).ToListAsync();
            foreach(var order in orders)
            {
                _orders.Remove(order);
            }
            await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        private void ChangeState(Order order, EntityState state)
        {
            _db.Entry(order).State = state;
        }
    }
}
