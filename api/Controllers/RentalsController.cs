using BusinessLogicLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers
{
  [ApiController]
  [Route("/api/[Controller]")]
  public class RentalsController : ControllerBase
  {
    private IRentalService _rentalService;

    public RentalsController(IRentalService rentalService)
    {
      _rentalService = rentalService;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public Task<List<RentalResponseDTO>> GetAll()
    {
      return _rentalService.GetAllRentals();
    }

    [HttpGet("{id}")]
    public Task<RentalResponseDTO> GetById(int id)
    {
      return _rentalService.GetById(id);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> CreateRental(CreateRentalRequestDTO createRentalRequestDto)
    {
      var userId = User.FindFirst("id")?.Value;
      var rental = await _rentalService.CreateRentalAsync(createRentalRequestDto, Convert.ToInt32(userId));

      return CreatedAtAction(nameof(GetById), new { Id = rental.Id }, rental);
    }
  }
}