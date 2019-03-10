using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Newtonsoft.Json;

namespace IdeaPool.Domain.Models
{
    public partial class User
    {
        [JsonIgnore]
        public byte[] Hash { get; set; }
        [JsonIgnore]
        public byte[] Salt { get; set; }
    }
}
