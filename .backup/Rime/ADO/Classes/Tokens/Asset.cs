﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rime.ADO.Classes
{
    public class Asset
    {
        public string Image { get; set; }
        public string Name { get; set; }
        public int TraitCount { get; set; }
        public string Fingerprint { get; set; }
    }
}