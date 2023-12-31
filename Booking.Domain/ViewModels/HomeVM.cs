﻿using Booking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Villa>? VillaList { get; set; }

        public DateOnly CheckInDate { get; set; }
        public DateOnly? CheckOutDate { get;set; }

        public int Nights {  get; set; }    

    }
}
