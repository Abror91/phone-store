using Phonix.BLL.DTO;
using Phonix.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonix.BLL.Interfaces
{
    public interface IOrderService : IDisposable
    {
        Task<IEnumerable<OrderDTO>> GetOrders();
        Task<OrderDTO> GetOrderById(int? id);
        Task<OperationDetails> MakeOrder(OrderDTO order);
        Task<OperationDetails> UpdateOrder(OrderDTO order);
        Task<OperationDetails> DeleteOrder(int? id);
    }
}
