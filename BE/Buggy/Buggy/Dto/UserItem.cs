﻿namespace Buggy.Dto
{
    public class UserItem
    {
        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool LockedOut { get; set; }

        public string Password { get; set; }

        public bool CanDelete { get; set; }
    }
}