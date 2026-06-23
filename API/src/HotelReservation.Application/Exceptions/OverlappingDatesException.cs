namespace HotelReservation.Application.Exceptions;

public class OverlappingDatesException : Exception
{

    public OverlappingDatesException(string message) 
        : base(message)
    {
    }
}