using System.Collections.Generic;
using System.Security.Claims;
using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DogGo.Controllers
{
    public class WalkerController : Controller
    {
        // Private field to hold the repository
        private readonly IWalkerRepository _walkerRepo;
        private readonly IWalksRepository _walksRepo;
        private readonly IOwnerRepository _ownerRepo;

        // Constructor to assign the repository
        public WalkerController(IWalkerRepository walkerRepository,
                                IWalksRepository walksRepository,
                                IOwnerRepository ownerRepository)
        {
            _walkerRepo = walkerRepository;
            _walksRepo = walksRepository;
            _ownerRepo = ownerRepository;
        }

        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }

        // GET: Walkers
        public ActionResult Index()
        {
            int currentUserId = GetCurrentUserId();
            Owner currentUser = _ownerRepo.GetOwnerById(currentUserId);
            List<Walker> walkers = _walkerRepo.GetWalkersInNeighborhood(currentUser.NeighborhoodId);

            return View(walkers);
        }

        // GET: WalkersController/Details/5
        public ActionResult Details(int id)
        {
            Walker walker = _walkerRepo.GetWalkerById(id);

            if (walker == null)
            {
                return NotFound();
            }

            List<Walks> walks = _walksRepo.GetWalksByWalkerId(id);
            WalkerProfileModel vm = new WalkerProfileModel()
            {
                Walker = walker,
                Walks = walks
            };

            return View(vm);
        }

        // GET: WalkersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WalkersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: WalkersController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WalkersController/Edit/5
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

        // GET: WalkersController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WalkersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
    }
}
