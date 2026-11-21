using System;

namespace HotelBookingSystem_New
{
    public class HotelBookingService : IBooking
    {
        // Implementation of BookRoom()
        public void BookRoom()
        {
            Console.WriteLine("Room booking operation started.");
        }

        // Implementation of CancelBooking()
        public void CancelBooking()
        {
            Console.WriteLine("Booking cancellation operation started.");
        }
    }
}