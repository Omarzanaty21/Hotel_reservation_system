namespace HotelReservation.Domain.Common;

public sealed class PagedResult<T>
{
    public IReadOnlyList<T> Items { get; init; } = Array.Empty<T>();
    public int TotalCount { get; init; }
    public int PageIndex { get; init; }
    public int PageSize { get; init; }

    public static PagedResult<T> Empty(int pageIndex, int pageSize) => new()
    {
        Items = Array.Empty<T>(),
        TotalCount = 0,
        PageIndex = pageIndex,
        PageSize = pageSize
    };
}
