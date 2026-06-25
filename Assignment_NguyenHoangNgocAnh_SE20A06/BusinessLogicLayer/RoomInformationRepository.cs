using System.Collections.Generic;
using BusinessObjects;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class RoomInformationRepository : IRoomInformationRepository
    {
        public List<RoomInformation> GetRooms() => RoomInformationDAO.Instance.GetRooms();

        public RoomInformation GetRoomById(int id) => RoomInformationDAO.Instance.GetRoomById(id);

        public bool AddRoom(RoomInformation room) => RoomInformationDAO.Instance.AddRoom(room);

        public bool UpdateRoom(RoomInformation room) => RoomInformationDAO.Instance.UpdateRoom(room);

        public bool DeleteRoom(int id) => RoomInformationDAO.Instance.DeleteRoom(id);

        public List<RoomType> GetRoomTypes() => RoomInformationDAO.Instance.GetRoomTypes();
    }
}
