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


        public static Slide GetBestSlice(Slide currentSlide, List<Slide> slides)
        {
            Slide bestSlide = slides.FirstOrDefault();
            var bestScore = 0;

            for (int i = 0; i < 2000 && slides.Count > 1999; i++)
            {
                var slide = slides[i];

                var score = GetInterestValue(currentSlide, slide);

                if (score > bestScore)
                {
                    bestSlide = slide;
                    bestScore = score;
                }

                if ((currentSlide.Tags.Count / 3) <= bestScore)
                    break;
            }

            return bestSlide;
        }
    }
}