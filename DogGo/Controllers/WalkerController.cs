using System;
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
        private readonly INeighborhoodRepository _neighborhoodRepo;

        // Constructor to assign the repository
        public WalkerController(IWalkerRepository walkerRepository,
                                IWalksRepository walksRepository,
                                IOwnerRepository ownerRepository,
                                INeighborhoodRepository neighborhoodRepository)
        {
            _walkerRepo = walkerRepository;
            _walksRepo = walksRepository;
            _ownerRepo = ownerRepository;
            _neighborhoodRepo = neighborhoodRepository;
        }

        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                return int.Parse(id);
            }
            catch (ArgumentNullException)
            {
                return 0;
            }
        }

        // GET: Walkers
        public ActionResult Index()
        {
            int currentUserId = GetCurrentUserId();
            List<Walker> walkers = new List<Walker>();
            if (currentUserId != 0)
            {
                Owner currentUser = _ownerRepo.GetOwnerById(currentUserId);
                walkers = _walkerRepo.GetWalkersInNeighborhood(currentUser.NeighborhoodId);
            }
            else
            {
                walkers = _walkerRepo.GetAllWalkers();
            }

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
            List<Neighborhood> neighborhoods = _neighborhoodRepo.GetAll();

            WalkerFormModel vm = new WalkerFormModel()
            {
                Walker = new Walker(),
                Neighborhoods = neighborhoods
            };
            return View(vm);
        }

        // POST: WalkersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Walker walker)
        {
            try
            {
                _walkerRepo.AddWalker(walker);

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                WalkerFormModel vm = new WalkerFormModel()
                {
                    Walker = walker,
                    Neighborhoods = _neighborhoodRepo.GetAll()
                };

                return View(vm);
            }
        }

        // GET: WalkersController/Edit/5
        public ActionResult Edit(int id)
        {
            Walker walker = _walkerRepo.GetWalkerById(id);

            if (walker == null)
            {
                return NotFound();
            }
            List<Neighborhood> neighborhoods = _neighborhoodRepo.GetAll();

            WalkerFormModel vm = new WalkerFormModel()
            {
                Walker = walker,
                Neighborhoods = neighborhoods
            };

            return View(vm);
        }

        // POST: WalkersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Walker walker)
        {
            try
            {
                _walkerRepo.UpdateWalker(walker);

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                WalkerFormModel vm = new WalkerFormModel()
                {
                    Walker = walker,
                    Neighborhoods = _neighborhoodRepo.GetAll()
                };
                return View(vm);
            }
        }

        // GET: WalkersController/Delete/5
        public ActionResult Delete(int id)
        {
            Walker walker = _walkerRepo.GetWalkerById(id);

            return View(walker);
        }

        // POST: WalkersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Walker walker)
        {
            try
            {
                _walkerRepo.DeleteWalker(id);

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(walker);
            }
        }
    }
}
