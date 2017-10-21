using Game.Api.DataAccess.Models;
using System.Collections.Generic;
using System.Linq;

namespace Game.Api.DataAccess
{
    public class CharacterRepository
    {
        private static List<Character> characters = new List<Character>
        {
            new Character{ Id = 1, Armor = 0, Damage = 10, Health = 100, Speed = 1, Name = "archer" },
            new Character{ Id = 2, Armor = 10, Damage = 5, Health = 150, Speed = 1, Name = "warrior" }
        };

        public Character GetById(long id)
        {
            return characters.FirstOrDefault(it => it.Id == id);
        }
    }
}
