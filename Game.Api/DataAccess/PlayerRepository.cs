using Game.Api.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Api.DataAccess
{
    public class PlayerRepository
    {
        private static List<Player> players = new List<Player>
        {
            new Player{Id = Guid.NewGuid(), X = 10, Y = 10 },
            new Player{Id = Guid.NewGuid(), X = 11, Y = 11 }
        };

        public Player GetById(Guid id)
        {
            return players.FirstOrDefault(it => it.Id == id);
        }

        public Guid Add(Player player)
        {
            var id = Guid.NewGuid();
            player.Id = id;
            players.Add(player);
            return id;
        }
    }
}
