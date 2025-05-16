namespace WebApplication16.Services;

public class RentalDTO
{
   public string FirstName { get; set; }
   public string LastName { get; set; }
   public List<RentalDetailsDTO> Rentals { get; set; }
}

public class RentalDetailsDTO
{
   public int RentalID { get; set; }
   public DateTime RentalDate { get; set; }
   public DateTime? ReturnDate { get; set; }
   public string status { get; set; }
   public List<MovieRentedDTO> Movies { get; set; }
   
}

public class MovieRentedDTO
{
   public string Title { get; set; }
   public decimal PriceAtRental { get; set; }
}