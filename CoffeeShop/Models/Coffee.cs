﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeShop.Models
{
    public class Coffee
    {
        public int BeanVarietyId { get; set; }
        public BeanVariety BeanVariety { get; set; }
        public string Title { get; set; }
    }
}