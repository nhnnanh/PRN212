using System;
using System.Collections.Generic;
using System.Linq;
using BusinessObjects;

namespace BusinessLogicLayer
{
    public interface IBookingService
    {
        List<BookingReservation> GetAllBookings();
        BookingReservation GetBookingById(int id);
        bool AddBooking(BookingReservation booking);
        bool UpdateBooking(BookingReservation booking);
        bool DeleteBooking(int id);
        List<BookingReservation> GetStatisticsReport(DateTime startDate, DateTime endDate);
        List<BookingReservation> GetBookingsByCustomerId(int customerId);
        
        List<BookingDetail> GetBookingDetailsByReservationId(int id);
        bool AddBookingDetail(BookingDetail detail);
        bool DeleteBookingDetail(int reservationId, int roomId);
    }

    public class BookingService : IBookingService
    {
        private readonly IBookingReservationRepository _bookingRepository;

        public BookingService()
        {
            _bookingRepository = new BookingReservationRepository();
        }

        public List<BookingReservation> GetAllBookings() => _bookingRepository.GetBookingReservations();

        public BookingReservation GetBookingById(int id) => _bookingRepository.GetBookingReservationById(id);

        public bool AddBooking(BookingReservation booking) => _bookingRepository.AddBookingReservation(booking);

        public bool UpdateBooking(BookingReservation booking) => _bookingRepository.UpdateBookingReservation(booking);

        public bool DeleteBooking(int id) => _bookingRepository.DeleteBookingReservation(id);

        public List<BookingReservation> GetBookingsByCustomerId(int customerId)
        {
            return _bookingRepository.GetBookingReservations()
                .Where(b => b.CustomerId == customerId)
                .ToList();
        }

        public List<BookingReservation> GetStatisticsReport(DateTime startDate, DateTime endDate)
        {
            var start = DateOnly.FromDateTime(startDate);
            var end = DateOnly.FromDateTime(endDate);

            return _bookingRepository.GetBookingReservations()
                .Where(b => b.BookingDate.HasValue && b.BookingDate.Value >= start && b.BookingDate.Value <= end)
                .OrderByDescending(b => b.BookingDate)
                .ToList();
        }

        public List<BookingDetail> GetBookingDetailsByReservationId(int id) => _bookingRepository.GetBookingDetailsByReservationId(id);

        public bool AddBookingDetail(BookingDetail detail) => _bookingRepository.AddBookingDetail(detail);

        public bool DeleteBookingDetail(int reservationId, int roomId) => _bookingRepository.DeleteBookingDetail(reservationId, roomId);
    }
}
