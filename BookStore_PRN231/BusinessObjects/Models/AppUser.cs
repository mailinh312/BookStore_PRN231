using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public class AppUser : IdentityUser
    {
        public string? Address
        {
            get;
            set;
        }
        public string? Fullname
        {
            get;
            set;
        }
    }
}
