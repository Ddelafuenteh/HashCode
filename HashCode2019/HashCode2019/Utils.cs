using System.Collections.Generic;
using System.Linq;

namespace HashCode2019
{
    public static class Utils
    {
        public static int GetInterestValue(Slide p1, Slide p2)
        {
            int numOverlap = p1.Tags.Intersect(p2.Tags).ToList().Count;
            int tagsl1 = p1.Tags.Count - numOverlap;
            int tagsl2 = p2.Tags.Count - numOverlap;

            return new List<int>() { tagsl1, numOverlap, tagsl2 }.Min();
        }
    }
}