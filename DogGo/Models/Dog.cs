using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DogGo.Models
{
    public class Dog
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Every dog needs a name...")]
        [MaxLength(35)]
        public string Name { get; set; }
        [Required]
        public int OwnerId { get; set; }
        [Required]
        public string Breed { get; set; }
        public string Notes { get; set; }
        [Url]
        [DisplayName("Picture URL")]

        public string ImageUrl { get; set; }
        public Owner Owner { get; set; }
    }
}
