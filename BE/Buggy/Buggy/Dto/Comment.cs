using System;

using Buggy.Data.Users;
using Buggy.Models.Votes;

namespace Buggy.Dto
{
    public class Comment
    {
        public string User { get; set; }
        public DateTime DatePosted { get; set; }
        public string Text { get; set; }

        public Comment()
        {
        }

        public Comment(UserVote source, User user)
        {
            User = user != null ? $"{user.FirstName} {user.LastName}" : string.Empty;
            DatePosted = source.DateVoted;
            Text = source.Comment;
        }
    }
}