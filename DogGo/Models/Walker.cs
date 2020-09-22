using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DogGo.Models
{
    public class Walker
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "What are we supposed to call this person? \"Hey, you\"?")]
        [MaxLength(35)]
        public string Name { get; set; }
        [Required]
        public int NeighborhoodId { get; set; }
        [Url]
        [DisplayName("Picture URL")]
        public string ImageUrl { get; set; }
        public Neighborhood Neighborhood { get; set; }
    }
}