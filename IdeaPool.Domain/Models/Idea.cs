using System;
using System.Collections.Generic;

namespace IdeaPool.Domain.Models
{
    public partial class Idea
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long? UserId { get; set; }

        public virtual User User { get; set; }
    }
}