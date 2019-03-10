using System;
using System.Collections.Generic;

namespace IdeaPool.Domain.Models
{
    public partial class User
    {
        public User()
        {
            Idea = new HashSet<Idea>();
        }

        public long Id { get; set; }
        public string Email { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }

        public virtual ICollection<Idea> Idea { get; set; }
    }
}