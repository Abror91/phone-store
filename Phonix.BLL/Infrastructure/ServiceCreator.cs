using Phonix.BLL.Interfaces;
using Phonix.BLL.Services;
using Phonix.DAL.Interfaces;
using Phonix.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonix.BLL.Infrastructure
{
    public class ServiceCreator : IServiceCreator
    {
        public IUserService CreateUserService()
        {
            return new UserService(new UnitOfWork());
        }
    }
}
