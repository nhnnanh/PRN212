using System.Collections.Generic;
using BusinessObjects;

namespace BusinessLogicLayer
{
    public interface IRoomInformationRepository
    {
        List<RoomInformation> GetRooms();
        RoomInformation GetRoomById(int id);
        bool AddRoom(RoomInformation room);
        bool UpdateRoom(RoomInformation room);
        bool DeleteRoom(int id);
        List<RoomType> GetRoomTypes();
    }
}
