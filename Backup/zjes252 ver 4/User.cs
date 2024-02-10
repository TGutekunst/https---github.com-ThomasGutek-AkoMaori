using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace A2.Models
{
    public class User
    {
       
        [Required, Key]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Address { get; set; }
        
    }
}
