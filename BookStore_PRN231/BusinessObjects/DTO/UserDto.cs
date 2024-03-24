using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessObjects.DTO
{
    public class UserDto
    {
        public string Id {  get; set; }
        public string UserName { get; set; }
        public string Email {  get; set; }
        public List<String> Roles { get; set; }
        public string Fullname { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
