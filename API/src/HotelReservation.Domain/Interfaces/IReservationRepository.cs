using System.Linq.Expressions;
using HotelReservation.Domain.Common;
using HotelReservation.Domain.Entities;

namespace HotelReservation.Domain.Interfaces;

public interface IReservationRepository : IGenericRepository<Reservation>
{
    Task<PagedResult<Reservation>> GetPagedAsync(
        int pageIndex,
        int pageSize,
        Expression<Func<Reservation, bool>>? filter = null,
        Func<IQueryable<Reservation>, IOrderedQueryable<Reservation>>? orderBy = null,
        bool includeRoom = false);

    Task<int> CountAsync(Expression<Func<Reservation, bool>>? filter = null);
}
