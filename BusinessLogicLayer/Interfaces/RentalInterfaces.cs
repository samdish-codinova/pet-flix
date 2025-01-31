using Models;

namespace BusinessLogicLayer
{
  public interface IRentalService
  {
    Task<List<Rental>> GetAllRentals();
  }
}
