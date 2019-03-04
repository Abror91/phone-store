using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Phonix.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonix.DAL.Repositories
{
    public class ApplicationRoleManager : RoleManager<ApplicationRole>
    {
        public ApplicationRoleManager(RoleStore<ApplicationRole> store) : base(store)
        {

        }
    }
}
