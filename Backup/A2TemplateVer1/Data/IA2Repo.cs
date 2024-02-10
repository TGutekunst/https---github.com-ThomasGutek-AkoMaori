using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using A2.Models;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace A2.Data
{
    public interface IA2Repo
    {
        public IEnumerable<Product> GetAllProducts();
        //public Product GetProductByID(int id);
        //public ActionResult<String> Register(User user);
        public User UserInDbbool(User user);
        public User AddUser(User user);


    }
}
