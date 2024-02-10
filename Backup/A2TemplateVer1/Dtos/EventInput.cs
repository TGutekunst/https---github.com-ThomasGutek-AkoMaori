using System.ComponentModel.DataAnnotations;

namespace A2.Dtos
{
    public class EventInput
    {
        [Required]
        public string Start { get; set; }

        [Required]
        public string End { get; set; }

        //[Required]
        public string summary { get; set; }
        //[Required]
        public string description { get; set; }
        [Required]
        public DateTime location { get; set; }


    }
}


