using Models;

namespace BusinessLogicLayer
{
  public interface IRentalService
  {
    Task<RentalResponseDTO> GetById(int id);
    Task<List<RentalResponseDTO>> GetAllRentals();
    Task<RentalResponseDTO> CreateRentalAsync(CreateRentalRequestDTO rentalRequestDto, int userId);
  }
}
