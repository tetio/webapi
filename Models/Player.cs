using System;
using System.Collections.Generic;

namespace webapi.Models
{

    public class Player
    {
        public double longitude { get; set; }
        public double latitude { get; set; }
        public string username { get; set; }
        public DateTime joinedAt { get; set; }
        public List<Movement> movements { get; set; }

    }
}