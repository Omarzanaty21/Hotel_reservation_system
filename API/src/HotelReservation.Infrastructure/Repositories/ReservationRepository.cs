using System.Linq.Expressions;
using HotelReservation.Domain.Common;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Interfaces;
using HotelReservation.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Infrastructure.Repositories;

public class ReservationRepository : GenericRepository<Reservation>, IReservationRepository
{
    public ReservationRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<PagedResult<Reservation>> GetPagedAsync(
        int pageIndex,
        int pageSize,
        Expression<Func<Reservation, bool>>? filter = null,
        Func<IQueryable<Reservation>, IOrderedQueryable<Reservation>>? orderBy = null,
        bool includeRoom = false)
    {
        IQueryable<Reservation> query = _dbSet.AsQueryable();

        if (includeRoom)
        {
            query = query.Include(reservation => reservation.Room);
        }

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        var totalCount = await query.CountAsync();

        query = orderBy is not null ? orderBy(query) : query.OrderBy(reservation => reservation.Id);

        var items = await query
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Reservation>
        {
            Items = items,
            TotalCount = totalCount,
            PageIndex = pageIndex,
            PageSize = pageSize,
        };
    }

    public async Task<int> CountAsync(Expression<Func<Reservation, bool>>? filter = null)
    {
        return filter is null
            ? await _dbSet.CountAsync()
            : await _dbSet.CountAsync(filter);
    }
}
