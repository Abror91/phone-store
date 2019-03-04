using Ninject.Modules;
using Phonix.BLL.Interfaces;
using Phonix.BLL.Services;
using Phonix.DAL;
using Phonix.DAL.Interfaces;
using Phonix.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonix.BLL.Ninject
{
    public class NinjectInitializer : NinjectModule
    {
        public override void Load()
        {
            Bind<IApplicationDbContext>().To<ApplicationDbContext>();
            Bind<IUnitOfWork>().To<UnitOfWork>();
            Bind<IOrderRepository>().To<OrderRepository>();
            Bind<IPhoneRepository>().To<PhoneRepository>();
            Bind<IOrderService>().To<OrderService>();
            Bind<IPhoneService>().To<PhoneService>();
            Bind<IUserService>().To<UserService>();
            Bind<IRoleService>().To<RoleService>();
        }
    }
}
