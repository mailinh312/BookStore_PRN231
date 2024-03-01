using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public class Status
    {
        public Status()
        {
            Orders = new HashSet<Order>();
        }

        [Key]
        [Display(Name = "Mã trạng thái")]
        public int StatusId { get; set; }

        [Display(Name = "Tên trạng thái")]
        public string? StatusName { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
