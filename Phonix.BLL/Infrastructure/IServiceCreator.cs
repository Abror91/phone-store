using Phonix.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonix.BLL.Infrastructure
{
    public interface IServiceCreator
    {
        IUserService CreateUserService();
    }
}
