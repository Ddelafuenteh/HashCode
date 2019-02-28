using System;
using System.Collections.Generic;
using System.Text;

namespace HashCode2019
{
    public class Coincidence
    {
        public string Tag { get; set; }
        public IList<Match> Matches { get; set; }

        public override int GetHashCode()
        {
            return 0;
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;

            if (!(obj is Coincidence))
                return false;

            return Tag.Equals((obj as Coincidence).Tag);
        }
    }

    public class Match
    {
        public int PhotoId { get; set; }
        public int NumberOfCoincidences { get; set; }
    }
}
