using Common.Api.Exceptions;

namespace Game.Api.Services.Exceptions
{
    public class RoomNotFoundException : NotFoundException
    {
        public string RoomName { get; private set; }

        public RoomNotFoundException(string roomName)
            : base($"Room '{roomName}' is not found")
        {
            RoomName = roomName;
        }
    }
}
