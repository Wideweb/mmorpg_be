using Common.Api.Exceptions;

namespace Identity.Api.Services.Exceptions
{
    public class CharacterNotFoundException : NotFoundException
    {
        public long Id { get; private set; }

        public CharacterNotFoundException(long id)
            : base($"Character '{id}' is not found")
        {
            Id = id;
        }
    }
}
