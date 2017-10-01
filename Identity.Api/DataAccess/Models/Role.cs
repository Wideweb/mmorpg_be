﻿namespace Identity.Api.DataAccess.Models
{
    public class Role
    {
        public string Name { get; set; }

        public const string UserRoleName = "User";
        public const string AdminRoleName = "Administrator";
    }
}