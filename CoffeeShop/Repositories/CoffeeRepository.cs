using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using CoffeeShop.Models;

namespace CoffeeShop.Repositories
{
    public class CoffeeRepository
    {
        private readonly string _connectionString;
        public CoffeeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection Connection
        {
            get { return new SqlConnection(_connectionString); }
        }

        public List<Coffee> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT c.Id, c.Title, c.BeanVarietyId, bv.[Name], bv.Region, bv.Notes FROM Coffee c
                                        JOIN BeanVariety bv
                                        ON c.BeanVarietyId = bv.Id";
                    using (var reader = cmd.ExecuteReader())
                    {
                        var coffees = new List<Coffee>();
                        while (reader.Read())
                        {
                            var coffee = new Coffee()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("c.Id")),
                                Title = reader.GetString(reader.GetOrdinal("c.Title")),
                                BeanVarietyId = reader.GetInt32(reader.GetOrdinal("c.BeanVarietyId")),
                                BeanVariety = new BeanVariety
                                {
                                    Name = reader.GetString(reader.GetOrdinal("bv.[Name]")),
                                    Region = reader.GetString(reader.GetOrdinal("bv.Region"))
                                }
                            };
                            if (!reader.IsDBNull(reader.GetOrdinal("Notes")))
                            {
                                coffee.BeanVariety.Notes = reader.GetString(reader.GetOrdinal("Notes"));
                            }
                            coffees.Add(coffee);

                        }
                    return coffees;
                    }
                }
            }
        }

        public Coffee Get(int id)
        {
                using (var conn = Connection)
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"SELECT c.Id, c.Title, c.BeanVarietyId, bv.[Name], bv.Region, bv.Notes FROM Coffee c
                                          JOIN BeanVariety bv
                                          ON c.BeanVarietyId = bv.Id
                                          WHERE c.Id = @id";

                        cmd.Parameters.AddWithValue("@id", id);

                        using (var reader = cmd.ExecuteReader())
                        {
                        var coffee = new Coffee();

                            if (reader.Read())
                            {
                                {
                                coffee.Id = reader.GetInt32(reader.GetOrdinal("c.Id"));
                                coffee.Title = reader.GetString(reader.GetOrdinal("c.Title"));
                                coffee.BeanVarietyId = reader.GetInt32(reader.GetOrdinal("c.BeanVarietyId"));
                                    coffee.BeanVariety = new BeanVariety
                                    {
                                        Name = reader.GetString(reader.GetOrdinal("bv.[Name]")),
                                        Region = reader.GetString(reader.GetOrdinal("bv.Region"))
                                    };
                                };
                                if (!reader.IsDBNull(reader.GetOrdinal("Notes")))
                                {
                                    coffee.BeanVariety.Notes = reader.GetString(reader.GetOrdinal("Notes"));
                                }
                            }
                            return coffee;
                        }
                    }
                }
            }

        public void Add(Coffee coffee)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Coffee (Title, BeanVarietyId)
                        OUTPUT INSERTED.ID
                        VALUES (@title, @beanVarietyId)";
                    cmd.Parameters.AddWithValue("@title", coffee.Title);
                    cmd.Parameters.AddWithValue("@beanVarietyId", coffee.BeanVarietyId);

                    coffee.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(Coffee coffee)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE BeanVariety 
                           SET [Name] = @name, 
                               Region = @region, 
                               Notes = @notes
                         WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", variety.Id);
                    cmd.Parameters.AddWithValue("@name", variety.Name);
                    cmd.Parameters.AddWithValue("@region", variety.Region);
                    if (variety.Notes == null)
                    {
                        cmd.Parameters.AddWithValue("@notes", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@notes", variety.Notes);
                    }

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Coffee WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}