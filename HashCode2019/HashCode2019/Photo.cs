using System;
using System.Collections.Generic;
using System.Text;

namespace HashCode2019
{
    public class Photo
    {
        public  int ID { get; set; }
        public Horientacion Horientation { get; set; }
        public HashSet<string> Tags { get; set; }
    }


    public enum Horientacion
    {
        Vertical,
        Horizontal
    }
}
