﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPark_Models.Models
{
    public class ParkingSpace : BaseEntity
    {
        public string Name { get; set; }
        public bool Isbooked { get; set; } = false;
    }
}
