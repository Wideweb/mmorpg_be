using Common.Api.Exceptions;

namespace Idntity.Api.Services.Exceptions
{
    public class PlayerNotFoundException : NotFoundException
    {
        public string Sid { get; private set; }

        public string RoomName { get; private set; }

        public PlayerNotFoundException(string sid, string roomName)
            : base($"Player '{sid}' in room {roomName} is not found")
        {
            Sid = sid;
            RoomName = roomName;
        }
    }
}
