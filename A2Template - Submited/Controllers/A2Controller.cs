
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A2.Dtos;
using A2.Models;
using A2.Data;
using A2.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using System.Collections;


namespace A2.Controllers
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

        [Authorize(AuthenticationSchemes = "Authentication")]
        [Authorize(Policy = "UserOnly")]
        [HttpGet("PurchaseItem/{id}")]
        public ActionResult<PurchaseOutput> PurchaseItem(int id)
        {
            //PurchaseOutput p = new PurchaseOutput { }
            //return Ok(PurchaseOutput)
            Product product = _repository.GetProductByID(id);

            ClaimsIdentity ci = HttpContext.User.Identities.FirstOrDefault();
            Claim c = ci.FindFirst("user");
            string username = c.Value;

            //User user = _repository.GetUserByInputID();
            if (product == null)
                return BadRequest("Product " + id + " not found");
                //return BadRequest("Product " + id + " not found");
            else
            {

                PurchaseOutput p = new PurchaseOutput { UserName = username, ProductID = product.Id };
                return Ok(p);


                //DateTime dt = DateTime.ParseExact(inputString, formatString, System.Globalization.CultureInfo.InvariantCulture);
            }
        }


        [Authorize(AuthenticationSchemes = "Authentication")]
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("AddEvent")]
        public ActionResult<string> AddEvent(EventInput newEvent)
        {
            DateTime parsedDate;
            string format = "yyyyMMddTHHmmssZ";
            bool validStart = DateTime.TryParseExact(newEvent.Start, format, null, System.Globalization.DateTimeStyles.None, out parsedDate);
            bool validEnd = DateTime.TryParseExact(newEvent.End, format, null, System.Globalization.DateTimeStyles.None, out parsedDate);
            var myData = new
            {
                message = "Bad request",
                errorCode = 400,
                detail = "The format of Start and End should be yyyyMMddTHHmmssZ.",
            };
            if (validStart == false)
            {
                return BadRequest(myData);
            }
            if (validEnd == false)
            {
                return BadRequest(myData);
            }
            if (validStart && validEnd == false)
            {
                return BadRequest(myData);
            }
            Event eventInput = new Event { Start = newEvent.Start, End = newEvent.End, Summary = newEvent.Summary, Description = newEvent.Description, Location = newEvent.Location };
            Event newEventInput = _repository.AddEvent(eventInput);
            return Ok("Success");

        }
        [Authorize(AuthenticationSchemes = "Authentication")]
        [Authorize(Policy = "AdminAndUser")]
        [HttpGet("EventCount")]
        public ActionResult<int> EventCount()
        {
            int eventCount = _repository.numEvent();
            return eventCount;

        }
        [Authorize(AuthenticationSchemes = "Authentication")]
        [Authorize(Policy = "AdminAndUser")]
        [HttpGet("Event/{id}")]
        public ActionResult Event(int id)
        {
            Event selectedEvent = _repository.GetEvent(id);
            if (selectedEvent == null)
            {
                //return BadRequest("The value '" + id + "' is not valid.");
                return BadRequest("Event " + id + " does not exist.");
            }
            Response.Headers.Add("Content-Type", "text/calendar");
            return Ok(selectedEvent);

        }


    }

}
    










//response.header.add("Content-Type", "text/calender"):