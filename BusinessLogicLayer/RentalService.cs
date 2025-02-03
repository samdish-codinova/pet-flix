using System.Net;
using System.Security.Claims;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Http;
using Models;

namespace BusinessLogicLayer
{
  public class RentalService : IRentalService
  {
    private IUnitOfWork _unitOfWork;
    private IHttpContextAccessor _httpContext;
    public RentalService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContext)
    {
      _unitOfWork = unitOfWork;
      _httpContext = httpContext;
    }

    public async Task<RentalResponseDTO> GetById(int id)
    {
      var rental = await _unitOfWork.Rentals.GetRentalWithDetails(id);
      if (rental == null)
      {
        throw new ErrorResponseException($"Rental not found for the given {id}", HttpStatusCode.NotFound);
      }

      var currentUserId = _httpContext.HttpContext?.User?.FindFirst("id")?.Value;
      var currentUserRole = _httpContext.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;

      if (currentUserRole != UserRole.Admin.ToString() && rental.User.Id != Convert.ToInt32(currentUserId))
      {
        throw new ErrorResponseException($"Rental not found for the given {id}", HttpStatusCode.NotFound);
      }

      return rental;
    }

    public async Task<List<RentalResponseDTO>> GetAllRentals()
    {
      return await _unitOfWork.Rentals.GetRentalsWithDetails();
    }

    public async Task<RentalResponseDTO> CreateRentalAsync(CreateRentalRequestDTO rentalRequestDto, int userId)
    {
      var movie = _unitOfWork.Movies.Get(rentalRequestDto.MovieId);
      if (movie == null)
      {
        throw new ErrorResponseException($"Movie not found for the given {rentalRequestDto.MovieId}", HttpStatusCode.NotFound);
      }

      var rental = new Rental()
      {
        MovieId = movie.Id,
        IssuedDate = DateTime.UtcNow,
        UserId = userId,
        TotalPrice = movie.RentalPrice,
      };
      _unitOfWork.Rentals.Add(rental);

      int isSaved = _unitOfWork.Complete();
      if (isSaved == 0)
      {
        throw new ErrorResponseException("Something went wrong while creating rental.", HttpStatusCode.InternalServerError);
      }

      return await GetById(rental.Id);
    }
  }
}