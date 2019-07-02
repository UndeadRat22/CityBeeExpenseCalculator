using System;
using System.Collections.Generic;
using System.Text;

namespace deadrat22
{
    public class Journey
    {
        public int ID { get; set; }
        public string Vehicle { get; set; }
        public string StartAddress { get; set; }
        public DateTime StartTime { get; set; }
        public string EndAddress { get; set; }
        public DateTime EndTime { get; set; }
        public int Distance { get; set; }
        public string Duration { get; set; }
        public double Price { get; set; }
    }
}
