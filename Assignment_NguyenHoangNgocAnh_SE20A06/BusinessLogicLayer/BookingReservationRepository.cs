using System.Collections.Generic;
using BusinessObjects;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class BookingReservationRepository : IBookingReservationRepository
    {
        public List<BookingReservation> GetBookingReservations() => BookingReservationDAO.Instance.GetBookingReservations();

        public BookingReservation GetBookingReservationById(int id) => BookingReservationDAO.Instance.GetBookingReservationById(id);

        public bool AddBookingReservation(BookingReservation booking) => BookingReservationDAO.Instance.AddBookingReservation(booking);

        public bool UpdateBookingReservation(BookingReservation booking) => BookingReservationDAO.Instance.UpdateBookingReservation(booking);

        public bool DeleteBookingReservation(int id) => BookingReservationDAO.Instance.DeleteBookingReservation(id);

        public List<BookingDetail> GetBookingDetailsByReservationId(int reservationId) => BookingDetailDAO.Instance.GetBookingDetailsByReservationId(reservationId);

        public bool AddBookingDetail(BookingDetail detail) => BookingDetailDAO.Instance.AddBookingDetail(detail);

        public bool DeleteBookingDetail(int reservationId, int roomId) => BookingDetailDAO.Instance.DeleteBookingDetail(reservationId, roomId);
    }
}
