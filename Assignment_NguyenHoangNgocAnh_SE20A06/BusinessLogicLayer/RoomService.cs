using System;
using System.Collections.Generic;
using BusinessObjects;

namespace BusinessLogicLayer
{
    public interface IRoomService
    {
        List<RoomInformation> GetAllRooms();
        RoomInformation GetRoomById(int id);
        bool AddRoom(RoomInformation room);
        bool UpdateRoom(RoomInformation room);
        bool DeleteRoom(int id);
        List<RoomType> GetRoomTypes();
    }

    public class RoomService : IRoomService
    {
        private readonly IRoomInformationRepository _roomRepository;

        public RoomService()
        {
            _roomRepository = new RoomInformationRepository();
        }

        public List<RoomInformation> GetAllRooms() => _roomRepository.GetRooms();

        public RoomInformation GetRoomById(int id) => _roomRepository.GetRoomById(id);

        public bool AddRoom(RoomInformation room) => _roomRepository.AddRoom(room);

        public bool UpdateRoom(RoomInformation room) => _roomRepository.UpdateRoom(room);

        public bool DeleteRoom(int id) => _roomRepository.DeleteRoom(id);

        public List<RoomType> GetRoomTypes() => _roomRepository.GetRoomTypes();
    }
}
