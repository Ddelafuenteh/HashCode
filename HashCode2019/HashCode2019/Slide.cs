using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HashCode2019
{
   public class Slide
    {
        public List<Photo> Photos { get; set; }


        public Slide(Photo photo)
        {
            Photos = new List<Photo>
            {
                photo
            };
        }
        public Slide(Photo photo1, Photo photo2)
        {
            Photos = new List<Photo>
            {
                photo1,
                photo2
            };
        }

        public HashSet<string> Tags
        {
            get
            {
                if ((Photos?.Count() ?? 0) == 0) return new HashSet<string>();
                if (Photos.Count() == 1) return Photos.First().Tags;

                var first = Photos.ElementAt(0);
                var second = Photos.ElementAt(1);

                return first.Tags.Union(second.Tags).Distinct().ToHashSet();
            }
        }
    }
}
