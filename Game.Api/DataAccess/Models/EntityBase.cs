namespace Game.Api.DataAccess.Models
{
    public class EntityBase
    {
        public long Id { get; set; }

        public bool IsNew => Id <= 0;
    }
}
