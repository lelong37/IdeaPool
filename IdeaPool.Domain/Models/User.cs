using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdeaPool.Domain.Models
{
    public partial class User
    {
        public User()
        {
            Idea = new HashSet<Idea>();
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
        public byte[] Hash { get; set; }
        public byte[] Salt { get; set; }
        public virtual ICollection<Idea> Idea { get; set; }
    }
}