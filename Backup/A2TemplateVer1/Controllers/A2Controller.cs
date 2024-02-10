
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A2.Dtos;
using A2.Models;
using A2.Data;
using Microsoft.AspNetCore.Mvc;



namespace A1.Controllers
{
    [Route("webapi")]
    [ApiController]
    public class A2Controller : Controller
    {
        private readonly IA2Repo _repository;


        public A2Controller(IA2Repo repository)
        {
            _repository = repository;
        }
        [HttpGet("GetVersion")]
        public ActionResult<string> GetVersion()
        {
            return Ok("1.0.0(Ngongotahā) by zjes252");

        }
        [HttpPost("Register")]
        public ActionResult<String> Register(User user)
        {
            User userInDb = _repository.UserInDbbool(user);

            if (userInDb == null)
            {
                User u = new User { UserName = user.UserName, Password = user.Password, Address = user.Address };
                //c.IP = ipAddress;
                //c.Time = formattedDateTime;
                User addedUser = _repository.AddUser(u);
                //CreatedAtAction(nameof(u), new { userName = addedUser.UserName }, addedUser);
                return Ok("User successfully registered.");
            }
            return Ok("UserName " + user.UserName + " is not available.");


            //DateTime dt = DateTime.ParseExact(inputString, formatString, System.Globalization.CultureInfo.InvariantCulture);
        }
        [HttpGet("PurchaseItem")]
        public PurchaseItem(int id)
        {
            User userInDb = _repository.UserInDbbool(user);

            if (userInDb == null)
            {
                User u = new User { UserName = user.UserName, Password = user.Password, Address = user.Address };
                //c.IP = ipAddress;
                //c.Time = formattedDateTime;
                User addedUser = _repository.AddUser(u);
                //CreatedAtAction(nameof(u), new { userName = addedUser.UserName }, addedUser);
                return Ok("User successfully registered.");
            }
            return Ok("UserName " + user.UserName + " is not available.");


            //DateTime dt = DateTime.ParseExact(inputString, formatString, System.Globalization.CultureInfo.InvariantCulture);
        }


    }
}