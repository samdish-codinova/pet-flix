using DataAccessLayer.Repositories;
using Models;

namespace BusinessLogicLayer
{
  public class RentalService : IRentalService
  {
    private IUnitOfWork _unitOfWork;
    public RentalService(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<List<Rental>> GetAllRentals()
    {
      return _unitOfWork.Rentals.GetAll().ToList();
    }
  }
}