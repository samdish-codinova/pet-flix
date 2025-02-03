using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccessLayer.Repositories
{
  public class RentalRepository : Repository<Rental>, IRentalRepository
  {
    public RentalRepository(ApplicationDBContext dBContext) : base(dBContext)
    {

    }

    public ApplicationDBContext DBContext
    {
      get { return Context as ApplicationDBContext; }
    }

    public async Task<RentalResponseDTO> GetRentalWithDetails(int id)
    {
      var rental = await DBContext.Rental.Include(r => r.Movie).Include(r => r.User).FirstOrDefaultAsync(r => r.Id == id);
      if (rental == null)
      {
        return null;
      }

      return new RentalResponseDTO()
      {
        Id = rental.Id,
        Movie = rental.Movie,
        ReturnDate = rental.ReturnDate,
        TotalPrice = rental.TotalPrice,
        User = new BaseUser()
        {
          Id = rental.User.Id,
          Email = rental.User.Email,
          Name = rental.User.Name,
          Role = rental.User.Role,
          CreatedAt = rental.User.CreatedAt,
          UpdatedAt = rental.User.UpdatedAt
        },
        IssuedDate = rental.IssuedDate
      };
    }

    public async Task<List<RentalResponseDTO>> GetRentalsWithDetails()
    {
      var list = await DBContext.Rental.Include(r => r.Movie).Include(r => r.User).ToListAsync();
      var qs = DBContext.Rental.Include(r => r.Movie).Include(r => r.User).ToQueryString();

      return list.Select(s => new RentalResponseDTO()
      {
        Id = s.Id,
        Movie = s.Movie,
        ReturnDate = s.ReturnDate,
        TotalPrice = s.TotalPrice,
        User = new BaseUser()
        {
          Id = s.User.Id,
          Email = s.User.Email,
          Name = s.User.Name,
          Role = s.User.Role,
          CreatedAt = s.User.CreatedAt,
          UpdatedAt = s.User.UpdatedAt
        },
        IssuedDate = s.IssuedDate
      }).ToList();
    }
  }
}