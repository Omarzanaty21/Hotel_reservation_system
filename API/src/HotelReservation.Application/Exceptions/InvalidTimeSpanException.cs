using System;
namespace HotelReservation.Application.Exceptions;

public class InvalidTimeSpanException : Exception
{

    public InvalidTimeSpanException(string message) 
        : base(message)
    {
    }
}