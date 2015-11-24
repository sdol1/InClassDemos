using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region
using Microsoft.AspNet.Identity.EntityFramework;
using eRestaurantSystem.Entities.Security;
using Microsoft.AspNet.Identity;
using eRestaurantSystem.DAL.Security;
#endregion

namespace eRestaurantSystem.BLL.Security
{
   
    public class UserManager : UserManager<ApplicationUser>
    {
        public UserManager()
            : base(new UserStore<ApplicationUser>(new ApplicationDbContext()))
        {
        }
    }
}
