using System.ComponentModel.DataAnnotations;

namespace A2.Dtos
{
    public class CommentInputDto
    {
        [Required]
        public string UserComment { get; set; }
        [Required]
        public string Name { get; set; }

    }
}


