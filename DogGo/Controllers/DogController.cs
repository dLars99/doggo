using System.Collections.Generic;
using System.Security.Claims;
using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DogGo.Controllers
{
    public class DogController : Controller
    {
        private readonly IDogRepository _dogRepo;
        private readonly IOwnerRepository _ownerRepo;

        public DogController(IDogRepository dogRepository, IOwnerRepository ownerRepository)
        {
            _dogRepo = dogRepository;
            _ownerRepo = ownerRepository;
        }

        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }

        // GET: DogController
        [Authorize]
        public ActionResult Index()
        {
            int ownerId = GetCurrentUserId();

            List<Dog> dogs = _dogRepo.GetDogsByOwnerId(ownerId);
            return View(dogs);
        }

        // GET: DogController/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);
            if (dog == null)
            {
                return NotFound();
            }
            return View(dog);
        }

        // GET: DogController/Create
        [Authorize]
        public ActionResult Create()
        {
            DogFormModel dfm = new DogFormModel()
            {
                Dog = new Dog(),
                Owners = _ownerRepo.GetAllOwners()
            };
            return View(dfm);
        }

        // POST: DogController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Dog dog)
        {
            try
            {
                // update the dogs OwnerId to the current user's Id 
                dog.OwnerId = GetCurrentUserId();
                _dogRepo.AddDog(dog);
                return RedirectToAction("Index");
            }
            catch
            {
                DogFormModel dfm = new DogFormModel()
                {
                    Dog = dog,
                    Owners = _ownerRepo.GetAllOwners()
                };
                return View(dfm);
            }
        }

        // GET: DogController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);
            int ownerId = GetCurrentUserId();

            if (dog == null || dog.OwnerId != ownerId)
            {
                return NotFound();
            }

            DogFormModel dfm = new DogFormModel()
            {
                Dog = dog,
                Owners = _ownerRepo.GetAllOwners()
            };
            return View(dfm);
        }

        // POST: DogController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Dog dog)
        {
            try
            {
                _dogRepo.UpdateDog(dog);
                return RedirectToAction("Index");
            }
            catch
            {
                DogFormModel dfm = new DogFormModel()
                {
                    Dog = dog,
                    Owners = _ownerRepo.GetAllOwners()
                };
                return View(dfm);
            }
        }

        [Authorize]
        // GET: DogController/Delete/5
        public ActionResult Delete(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);
            int ownerId = GetCurrentUserId();

            if (dog == null || dog.OwnerId != ownerId)
            {
                return NotFound();
            }

            return View(dog);
        }

        // POST: DogController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Dog dog)
        {
            try
            {
                _dogRepo.DeleteDog(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(dog);
            }
        }
    }
}
