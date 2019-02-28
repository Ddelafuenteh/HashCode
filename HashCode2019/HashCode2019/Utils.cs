using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HashCode2019
{
    public static class Utils
    {
        public static int GetInterestValue(Slide p1, Slide p2)
        {
            Dictionary<string, int> freqs = new Dictionary<string, int>();
            foreach (var t in p1.Tags)
            {
                if (freqs.ContainsKey(t))
                    freqs[t]++;
                else
                    freqs.Add(t, 1);
            }

            foreach (var t in p2.Tags)
            {
                if (freqs.ContainsKey(t))
                    freqs[t]++;
                else
                    freqs.Add(t, 1);
            }

            int numOverlap = freqs.Count(x => x.Value > 1);
            int tagsl1 = p1.Tags.Count - numOverlap;
            int tagsl2 = p2.Tags.Count - numOverlap;

            return new List<int>() { numOverlap, tagsl1, tagsl2 }.Min();
        }


        public static Slide GetBestSlice(Slide currentSlide, List<Slide> slides)
        {
            Slide bestSlide = slides.FirstOrDefault();
            int bestScore = 0;

            for (int i = 0; i < slides.Count - 1; i++)
            {
                var slide = slides[i];
                if ((bestScore) >= (currentSlide.Tags.Count / 2))
                    break;

                if (bestScore >= slide.Tags.Count / 2)
                    break;

                int score = GetInterestValue(currentSlide, slide);
                if (score > bestScore)
                {
                    bestSlide = slide;
                    bestScore = score;
                }
            }





            Program.selectedSlides.Add(bestSlide);
            return bestSlide;
        }
    }
}