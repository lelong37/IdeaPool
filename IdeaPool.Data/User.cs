using System;
using System.Collections.Generic;

namespace IdeaPool.Data
{
    public partial class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
        public byte[] Hash { get; set; }
        public byte[] Salt { get; set; }
        public string Password { get; set; }
    }
}