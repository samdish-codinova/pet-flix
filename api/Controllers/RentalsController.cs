using BusinessLogicLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers
{
  [ApiController]
  [Authorize(Roles = "Admin")]
  [Route("/api/[Controller]")]
  public class RentalsController : ControllerBase
  {
    private IRentalService _rentalService;

    public RentalsController(IRentalService rentalService)
    {
      _rentalService = rentalService;
    }

    [HttpGet]
    public Task<List<Rental>> GetAll()
    {
      return _rentalService.GetAllRentals();
    }
  }
}