using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;

namespace DogGo.Controllers
{
    public class WalksController : Controller
    {
        private readonly IWalksRepository _walksRepo;
        private readonly IWalkerRepository _walkerRepo;
        private readonly IOwnerRepository _ownerRepo;
        private readonly IDogRepository _dogRepo;

        public WalksController(IWalksRepository walksRepository,
                                IWalkerRepository walkerRepository,
                                IOwnerRepository ownerRepository,
                                IDogRepository dogRepository)
        {
            _walksRepo = walksRepository;
            _walkerRepo = walkerRepository;
            _ownerRepo = ownerRepository;
            _dogRepo = dogRepository;
        }

        // GET: WalksController
        public ActionResult Index()
        {
            return View();
        }

        // GET: WalksController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: WalksController/Create
        public ActionResult Create(int neighborhoodId)
        {
            List<Dog> allDogs = _dogRepo.GetDogsInNeighborhood(neighborhoodId);

            WalksFormModel wfm = new WalksFormModel()
            {
                Walks = new Walks(),
                Owners = _ownerRepo.GetOwnersInNeighborhood(neighborhoodId),
                Dogs = allDogs.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
            };
            return View(wfm);
        }

        // POST: WalksController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WalksFormModel res)
        {
            try
            {
                foreach (int id in res.SelectedDogs)
                {
                    res.Walks.DogId = id;
                    _walksRepo.AddWalks(res.Walks);
                }
                return RedirectToAction("Index", "Walker");
            }
            catch
            {
                List<Dog> Dogs = _dogRepo.GetAllDogs();
                WalksFormModel wfm = new WalksFormModel()
                {
                    Walks = res.Walks,
                    Owners = _ownerRepo.GetAllOwners(),
                    Dogs = Dogs.Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Name
                    })
                };
                return View(wfm);
            }
        }

        // GET: WalksController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WalksController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: WalksController/Delete/5
        public ActionResult Delete(int walkerId)
        {
            List<Walks> walks = _walksRepo.GetWalksByWalkerId(walkerId);
            WalksDeleteModel wdm = new WalksDeleteModel()
            {
                Walks = walks.Select(w => new SelectListItem
                {
                    Value = w.Id.ToString(),
                    Text = $"{w.Date} {w.Dog.Name} {w.Duration / 60}"
                })
            };

            return View(wdm);
        }

        // POST: WalksController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(WalksDeleteModel res)
        {
            try
            {
                int idToDelete;
                foreach (int id in res.SelectedWalks)
                {
                    idToDelete = id;
                    _walksRepo.DeleteWalks(idToDelete);
                }
                return RedirectToAction("Index", "Walker");
            }
            catch
            {
                return View(res);
            }
        }
    }
}
