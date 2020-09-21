using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Models.ViewModels
{
    public class WalksDeleteModel
    {
        public int[] SelectedWalks { get; set; }
        public IEnumerable<SelectListItem> Walks { get; set; }
    }
}
