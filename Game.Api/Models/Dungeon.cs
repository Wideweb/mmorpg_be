﻿using System.Collections.Concurrent;

namespace Game.Api.Models
{
    public class Dungeon
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public int MaxPlayersNumber { get; set; }

        public MapCell[][] Map { get; set; }

        public int Height => Map.Length;

        public int Width => Map[0].Length;

        public ConcurrentDictionary<string, GameObject> GameObjects { get; set; }

        public MapCell GetCell(int x, int y)
        {
            return Map[y][x];
        }

        public Point OriginPosition { get; set; }

        public Dungeon()
        {
            GameObjects = new ConcurrentDictionary<string, GameObject>();
        }
    }
}
