using System.Linq.Expressions;
using HotelReservation.Domain.Common;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Interfaces;
using HotelReservation.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Infrastructure.Repositories;

public class RoomRepository : GenericRepository<Room>, IRoomRepository
{
    public RoomRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<PagedResult<Room>> GetPagedAsync(
        int pageIndex,
        int pageSize,
        Expression<Func<Room, bool>>? filter = null,
        Func<IQueryable<Room>, IOrderedQueryable<Room>>? orderBy = null,
        bool includeReservations = false)
    {
        IQueryable<Room> query = _dbSet.AsQueryable();

        if (includeReservations)
        {
            query = query.Include(room => room.Reservations);
        }

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        var totalCount = await query.CountAsync();

        query = orderBy is not null ? orderBy(query) : query.OrderBy(room => room.Id);

        var items = await query
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Room>
        {
            Items = items,
            TotalCount = totalCount,
            PageIndex = pageIndex,
            PageSize = pageSize,
        };
    }

    public async Task<int> CountAsync(Expression<Func<Room, bool>>? filter = null)
    {
        return filter is null
            ? await _dbSet.CountAsync()
            : await _dbSet.CountAsync(filter);
    }
}
