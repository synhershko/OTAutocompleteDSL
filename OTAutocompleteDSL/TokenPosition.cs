﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTAutocompleteDSL
{
    public class TokenPosition
    {
        public int Column { get; set; }
        public int Index { get; set; }
        public int Line { get; set; }
    }
}
