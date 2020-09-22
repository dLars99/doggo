using System;
using System.ComponentModel.DataAnnotations;

namespace DogGo.Models
{
    public class Walks
    {
        public int Id { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:d")]
        public DateTime Date { get; set; }
        private int _duration;
        [Required]
        public int Duration
        {
            get
            {
                return _duration;
            }
            set
            {
                _duration = value * 60;
            }
        }
        [Required]
        public int WalkerId { get; set; }
        public Walker Walker { get; set; }
        [Required]
        public int DogId { get; set; }
        public Dog Dog { get; set; }
    }
}