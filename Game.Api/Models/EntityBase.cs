﻿namespace Game.Api.Models
{
    public class EntityBase
    {
        public long Id { get; set; }

        public bool IsNew => Id <= 0;
    }
}
