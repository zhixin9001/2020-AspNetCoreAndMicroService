﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocationService.Models
{
    public class LocationRecord
    {
        public Guid ID { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public float Altitude { get; set; }
        public long Timestamp { get; set; }
        public Guid MemberID { get; set; }
    }
}
