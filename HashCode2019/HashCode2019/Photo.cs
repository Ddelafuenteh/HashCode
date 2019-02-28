using System;
using System.Collections.Generic;
using System.Text;

namespace HashCode2019
{
    public class Photo
    {
        public int ID { get; set; }
        public Orientation Orientation { get; set; }
        public HashSet<string> Tags { get; set; }
    }


    public enum Orientation
    {
        Vertical,
        Horizontal
    }
}
