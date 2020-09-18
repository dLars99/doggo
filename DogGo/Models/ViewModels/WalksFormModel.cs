using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Models.ViewModels
{
    public class WalksFormModel
    {
        public Walks Walks { get; set; }
        public List<Dog> Dogs { get; set; }
        public List<Owner> Owners { get; set; }
        public int[] SelectedDogs { get; set; }
        public IEnumerable<SelectListItem> Items { get; set; }
    }
}
