using Models;

namespace DataAccessLayer.Repositories
{
  public interface IRentalRepository : IRepository<Rental>
  {
    Task<RentalResponseDTO> GetRentalWithDetails(int id);
    Task<List<RentalResponseDTO>> GetRentalsWithDetails();
  }
}