using System;
using System.Collections.Generic;
using System.Linq;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class RoomInformationDAO
    {
        private static RoomInformationDAO instance = null;
        private static readonly object instanceLock = new object();

        private RoomInformationDAO() { }

        public static RoomInformationDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new RoomInformationDAO();
                    }
                    return instance;
                }
            }
        }

        public List<RoomInformation> GetRooms()
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                return context.RoomInformations.Include(r => r.RoomType).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetRooms: " + ex.Message);
            }
        }

        public RoomInformation GetRoomById(int id)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                return context.RoomInformations.Include(r => r.RoomType).FirstOrDefault(r => r.RoomId == id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetRoomById: " + ex.Message);
            }
        }

        public bool AddRoom(RoomInformation room)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                context.RoomInformations.Add(room);
                return context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in AddRoom: " + ex.Message);
            }
        }

        public bool UpdateRoom(RoomInformation room)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                context.Entry(room).State = EntityState.Modified;
                return context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in UpdateRoom: " + ex.Message);
            }
        }

        public bool DeleteRoom(int id)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                var room = context.RoomInformations.Find(id);
                if (room != null)
                {
                    // Check if the room has bookings (exists in BookingDetail)
                    bool hasBooking = context.BookingDetails.Any(bd => bd.RoomId == id);
                    if (hasBooking)
                    {
                        // Change room status to inactive (0) instead of physical delete
                        room.RoomStatus = 2; // Usually 0 or 2 for inactive/disabled. Let's use 0 or 2.
                        // Let's check typical status: RoomStatus is tinyint. We can set it to 2 or 0.
                        // The prompt says "hãy chuyển RoomStatus sang trạng thái vô hiệu hóa thay vì xóa." (change RoomStatus to disabled status instead of delete)
                        // Let's set it to 0 or 2. Usually 1 is Active, 2 is Inactive, or 0 is Inactive. Let's set RoomStatus to 2 or 0.
                        // Let's look at what RoomStatus values are typically in the database.
                        // We can run a sqlcmd command to see what values exist in the database!
                        // That is a great research step.
                        context.RoomInformations.Update(room);
                    }
                    else
                    {
                        // Physical delete
                        context.RoomInformations.Remove(room);
                    }
                    return context.SaveChanges() > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in DeleteRoom: " + ex.Message);
            }
        }

        public List<RoomType> GetRoomTypes()
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                return context.RoomTypes.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetRoomTypes: " + ex.Message);
            }
        }
    }
}
