using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Repositories
{
    public class WalksRepository : IWalksRepository
    {
        private readonly IConfiguration _config;

        public WalksRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public List<Walks> GetWalksByWalkerId(int walkerId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    SELECT w.Id, w.[Date], w.Duration, w.WalkerId, w.DogId, 
	                    d.[Name] AS DogName, d.Breed, d.Notes, d.ImageUrl, 
	                    d.OwnerId, o.[Name] AS OwnerName, o.Email, o.Address, o.NeighborhoodId, o.Phone
                    FROM Walks w
                    LEFT JOIN Dog d ON d.Id = w.DogId
                    LEFT JOIN Owner o on o.Id = d.OwnerId
                    WHERE WalkerId = @walkerId
                    ORDER BY o.[Name]";

                    cmd.Parameters.AddWithValue("@walkerId", walkerId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Walks> walks = new List<Walks>();

                    while (reader.Read())
                    {

                        Owner owner = new Owner
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            Name = reader.GetString(reader.GetOrdinal("OwnerName")),
                            Address = reader.GetString(reader.GetOrdinal("Address")),
                            NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                            Phone = reader.GetString(reader.GetOrdinal("Phone"))
                        };

                        Dog dog = new Dog
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("DogId")),
                            Name = reader.GetString(reader.GetOrdinal("DogName")),
                            Breed = reader.GetString(reader.GetOrdinal("Breed")),
                            Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes")),
                            OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                            Owner = owner,
                            ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? null : reader.GetString(reader.GetOrdinal("ImageUrl"))
                        };

                        Walks walk = new Walks()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                            Duration = reader.GetInt32(reader.GetOrdinal("Duration")) / 60,
                            DogId = reader.GetInt32(reader.GetOrdinal("DogId")),
                            Dog = dog
                        };

                        walks.Add(walk);
                    }
                    reader.Close();
                    return walks;
                }
            }
        }
        public void AddWalks(Walks walks)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    INSERT INTO Walks (Date, Duration, WalkerId, DogId)
                    OUTPUT INSERTED.ID
                    VALUES (@date, @duration, @walkerId, @dogId);
                ";

                    cmd.Parameters.AddWithValue("@date", walks.Date);
                    cmd.Parameters.AddWithValue("@duration", walks.Duration);
                    cmd.Parameters.AddWithValue("@walkerId", walks.WalkerId);
                    cmd.Parameters.AddWithValue("@dogId", walks.DogId);

                    int id = (int)cmd.ExecuteScalar();

                    walks.Id = id;
                }
            }
        }

    }
}
