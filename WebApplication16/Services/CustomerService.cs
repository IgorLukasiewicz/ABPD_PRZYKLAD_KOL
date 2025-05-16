using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace WebApplication16.Services;

public class CustomerService : ICustomerService
{
    private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=APBD_2;Integrated Security=True;";

    public async Task<IActionResult> getInfoForClientById(int id)
    {
        var toBeReturned = new Dictionary<int, RentalDTO>();

        string sql = @"Select C.customer_id, C.first_name, C.last_name, R.rental_id, R.rental_date, R.return_date, S.name, M.title, M.price_per_day 
                       FROM CUSTOMER C
                       JOIN RENTAL R ON R.customer_id = C.customer_id
                       Join Status S ON r.status_id = S.status_id
                       JOIN RENTAL_ITEM RI ON RI.rental_id = R.rental_ID
                       JOIN MOVIE M ON M.movie_id = RI. movie_id
                       WHERE C.customer_id = @customerId";
        
        await using var conn = new SqlConnection(_connectionString);
        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@customerId", id); 

        await conn.OpenAsync();
        using SqlDataReader reader = await cmd.ExecuteReaderAsync();


        while (await reader.ReadAsync())
        {
            int customerID = reader.GetInt32(reader.GetOrdinal("customer_id"));
            if (!toBeReturned.ContainsKey(customerID))
            {
                toBeReturned[customerID] = new RentalDTO
                {
                    FirstName = (string)reader["first_name"],
                    LastName = (string)reader["last_name"],
                    Rentals = new List<RentalDetailsDTO>()
                };
            }

            var newRentalID = reader.GetInt32(reader.GetOrdinal("rental_id"));
            if (!toBeReturned[customerID].Rentals.Any(r => r.RentalID == newRentalID))
            {
                var Rental = new RentalDetailsDTO()
                {
                    RentalID = newRentalID,
                    RentalDate = reader.GetDateTime(reader.GetOrdinal("rental_date")),
                    ReturnDate = reader.IsDBNull(reader.GetOrdinal("return_date")) ? null : reader.GetDateTime(reader.GetOrdinal("return_date")),
                    status = reader.GetString(reader.GetOrdinal("name")),
                    Movies = new List<MovieRentedDTO>()
                };
                toBeReturned[customerID].Rentals.Add(Rental);
            }
            var Movie = new MovieRentedDTO()
            {
                Title = reader.GetString(reader.GetOrdinal("title")),
                PriceAtRental = reader.GetDecimal(reader.GetOrdinal("price_per_day")),
            };

            var currentRental = toBeReturned[customerID].Rentals.First(r => r.RentalID == newRentalID);

            currentRental.Movies.Add(Movie);
    }
        return new OkObjectResult(toBeReturned.Values.ToList());
    }
}