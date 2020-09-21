using DogGo.Models;
using Microsoft.JSInterop.Infrastructure;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public interface IDogRepository
    {
        List<Dog> GetAllDogs();
        Dog GetDogById(int id);
        List<Dog> GetDogsByOwnerId(int ownerId);
        List<Dog> GetDogsInNeighborhood(int neighborhoodId);
        void AddDog(Dog dog);
        void UpdateDog(Dog dog);
        void DeleteDog(int dogId);
    }
}