using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIMS.ViewModel
{
    public class UserRoleViewModel
    {
        public Models.User User { get; set; }
        public Models.Role Role { get; set; }
        public bool active{ get; set; }


    }
}