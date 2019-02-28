using System;
using System.Collections.Generic;
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
    }
}
