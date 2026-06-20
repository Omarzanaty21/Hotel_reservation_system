using System.Linq.Expressions;
using HotelReservation.Domain.Common;
using HotelReservation.Domain.Entities;

namespace HotelReservation.Domain.Interfaces;

public interface IRoomRepository : IGenericRepository<Room>
{
    Task<PagedResult<Room>> GetPagedAsync(
        int pageIndex,
        int pageSize,
        Expression<Func<Room, bool>>? filter = null,
        Func<IQueryable<Room>, IOrderedQueryable<Room>>? orderBy = null,
        bool includeReservations = false);

    Task<int> CountAsync(Expression<Func<Room, bool>>? filter = null);
}
