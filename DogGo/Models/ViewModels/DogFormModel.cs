using System.Collections.Generic;

namespace DogGo.Models.ViewModels
{
    public class DogFormModel
    {
        public Dog Dog { get; set; }
        public List<Owner> Owners { get; set; }
    }
}
