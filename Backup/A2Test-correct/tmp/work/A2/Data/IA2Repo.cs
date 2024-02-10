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

        //public Product GetProductByID(int id);
        //public ActionResult<String> Register(User user);
        public User UserInDbbool(User user);
        public User AddUser(User user);

        public bool ValidLogin(string userName, string password);

        public bool ValidLoginOrganizer(string userName, string password);
        public Product GetProductByID(int id);

        public Event AddEvent(Event newEvent);
        public int numEvent();




        //public IEnumerable<Customer> GetAllCustomers();
        //public bool ValidLogin(string userName, string password);
        //public Customer GetCustomerByEmail(string e);

    }
}
