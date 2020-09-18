using System;

namespace DogGo.Models
{
    public class Walks
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        private int _duration;
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
        public int WalkerId { get; set; }
        public Walker Walker { get; set; }
        public int DogId { get; set; }
        public Dog Dog { get; set; }
    }
}