using System.Collections.Generic;
using BusinessObjects;

namespace BusinessLogicLayer
{
    public interface IBookingReservationRepository
    {
        List<BookingReservation> GetBookingReservations();
        BookingReservation GetBookingReservationById(int id);
        bool AddBookingReservation(BookingReservation booking);
        bool UpdateBookingReservation(BookingReservation booking);
        bool DeleteBookingReservation(int id);
        
        List<BookingDetail> GetBookingDetailsByReservationId(int reservationId);
        bool AddBookingDetail(BookingDetail detail);
        bool DeleteBookingDetail(int reservationId, int roomId);
    }
}
