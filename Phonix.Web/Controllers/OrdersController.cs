using AutoMapper;
using Phonix.BLL.DTO;
using Phonix.BLL.Interfaces;
using Phonix.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Phonix.Web.Controllers
{
    public class OrdersController : Controller
    {

        private readonly IOrderService _orders;
        public OrdersController(IOrderService orders)
        {
            _orders = orders;
        }
        public async Task<ActionResult> Index()
        {
            var orders = await _orders.GetOrders();
            var mapper = new MapperConfiguration(c => c.CreateMap<OrderDTO, OrderViewModel>()).CreateMapper();
            var ordersToReturn = mapper.Map<IEnumerable<OrderDTO>, IEnumerable<OrderViewModel>>(orders);
            return View();
        }
    }
}