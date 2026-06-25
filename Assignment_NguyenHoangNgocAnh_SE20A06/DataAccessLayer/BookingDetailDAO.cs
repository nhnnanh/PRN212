using System;
using System.Collections.Generic;
using System.Linq;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class BookingDetailDAO
    {
        private static BookingDetailDAO instance = null;
        private static readonly object instanceLock = new object();

        private BookingDetailDAO() { }

        public static BookingDetailDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new BookingDetailDAO();
                    }
                    return instance;
                }
            }
        }

        public List<BookingDetail> GetBookingDetails()
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                return context.BookingDetails
                    .Include(bd => bd.Room)
                    .Include(bd => bd.BookingReservation)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetBookingDetails: " + ex.Message);
            }
        }

        public List<BookingDetail> GetBookingDetailsByReservationId(int reservationId)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                return context.BookingDetails
                    .Include(bd => bd.Room)
                    .Where(bd => bd.BookingReservationId == reservationId)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetBookingDetailsByReservationId: " + ex.Message);
            }
        }

        public bool AddBookingDetail(BookingDetail detail)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                context.BookingDetails.Add(detail);
                return context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in AddBookingDetail: " + ex.Message);
            }
        }

        public bool UpdateBookingDetail(BookingDetail detail)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                context.Entry(detail).State = EntityState.Modified;
                return context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in UpdateBookingDetail: " + ex.Message);
            }
        }

        public bool DeleteBookingDetail(int reservationId, int roomId)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                var detail = context.BookingDetails.FirstOrDefault(bd => bd.BookingReservationId == reservationId && bd.RoomId == roomId);
                if (detail != null)
                {
                    context.BookingDetails.Remove(detail);
                    return context.SaveChanges() > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in DeleteBookingDetail: " + ex.Message);
            }
        }
    }
}
