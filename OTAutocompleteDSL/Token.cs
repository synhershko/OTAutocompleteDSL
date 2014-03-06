﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTAutocompleteDSL
{
    public class Token
    {
        public int Position { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Type, Value);
        }
    }
}
