using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using A2.Dtos;
using A2.Models;

namespace A2.Helper
{
    public class CalendarOutputFormatter : TextOutputFormatter
    {
        public CalendarOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/calendar"));
            //("text/vcard"));plain
            SupportedEncodings.Add(Encoding.UTF8);
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            Event selectedEvent = (Event)context.Object;
            StringBuilder builder = new StringBuilder();
            DateTime currentDateTime = DateTime.Now;
            string formattedDateTime = currentDateTime.ToString("yyyyMMddTHHmmssZ");


            builder.AppendLine("BEGIN:VCALENDAR");
            builder.AppendLine("VERSION:2.0");
            builder.AppendLine("PRODID:zjes252");
            builder.AppendLine("BEGIN:VEVENT");
            builder.Append("UID:").AppendLine(selectedEvent.Id + "");
            builder.Append("DTSTAMP:").AppendLine(formattedDateTime);
            builder.Append("DTSTART:").AppendLine(selectedEvent.Start);
            builder.Append("DTEND:").AppendLine(selectedEvent.End);
            builder.Append("SUMMARY:").AppendLine(selectedEvent.Summary);
            builder.Append("DESCRIPTION:").AppendLine(selectedEvent.Description);
            builder.Append("LOCATION:").AppendLine(selectedEvent.Location);
            builder.AppendLine("END:VEVENT"+"");
            builder.AppendLine("END:VCALENDAR");

            //builder.Append("END:VCALENDAR" + "");


            string outString = builder.ToString();
            byte[] outBytes = selectedEncoding.GetBytes(outString);
            var response = context.HttpContext.Response.Body;
            return response.WriteAsync(outBytes, 0, outBytes.Length);
        }
    }
}


