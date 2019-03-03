using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HashCode2019
{
   public class Slide
    {
        public List<Photo> Photos { get; set; }

        public HashSet<string> Tags { get; set; }

        public Slide(Photo photo, HashSet<string> tags)
        {
            Photos = new List<Photo>
            {
                photo
            };

            Tags = tags;
        }


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
            Tags = new HashSet<string>(photo1.Tags.Concat(photo2.Tags));
        }
    }
}
