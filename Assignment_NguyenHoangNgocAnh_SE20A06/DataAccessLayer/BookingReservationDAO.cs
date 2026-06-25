using System;
using System.Collections.Generic;
using System.Linq;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class BookingReservationDAO
    {
        private static BookingReservationDAO instance = null;
        private static readonly object instanceLock = new object();

        private BookingReservationDAO() { }

        public static BookingReservationDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new BookingReservationDAO();
                    }
                    return instance;
                }
            }
        }

        public List<BookingReservation> GetBookingReservations()
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                return context.BookingReservations
                    .Include(b => b.Customer)
                    .Include(b => b.BookingDetails)
                        .ThenInclude(bd => bd.Room)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetBookingReservations: " + ex.Message);
            }
        }

        public BookingReservation GetBookingReservationById(int id)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                return context.BookingReservations
                    .Include(b => b.Customer)
                    .Include(b => b.BookingDetails)
                        .ThenInclude(bd => bd.Room)
                    .FirstOrDefault(b => b.BookingReservationId == id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetBookingReservationById: " + ex.Message);
            }
        }

        public bool AddBookingReservation(BookingReservation booking)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                if (booking.BookingReservationId == 0)
                {
                    int maxId = context.BookingReservations.Any() ? context.BookingReservations.Max(b => b.BookingReservationId) : 0;
                    booking.BookingReservationId = maxId + 1;
                }
                context.BookingReservations.Add(booking);
                return context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in AddBookingReservation: " + ex.Message);
            }
        }

        public bool UpdateBookingReservation(BookingReservation booking)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                context.Entry(booking).State = EntityState.Modified;
                return context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in UpdateBookingReservation: " + ex.Message);
            }
        }

        public bool DeleteBookingReservation(int id)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                var booking = context.BookingReservations.Find(id);
                if (booking != null)
                {
                    // Delete details first
                    var details = context.BookingDetails.Where(d => d.BookingReservationId == id).ToList();
                    context.BookingDetails.RemoveRange(details);
                    
                    context.BookingReservations.Remove(booking);
                    return context.SaveChanges() > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in DeleteBookingReservation: " + ex.Message);
            }
        }
    }
}
