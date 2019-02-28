using System;
using System.Collections.Generic;
using System.Text;

namespace HashCode2019
{
    public class Algorithm
    {
        public IList<Photo> Photos { get; set; }
        public IList<Slide> TotalSlides { get; set; }

        public void Execute()
        {

        }

        private void JoinPhotos()
        {
            while(Photos.Count > 1)
            {
                var rngesus = new Random(DateTime.Now.Millisecond);
                var firstPhoto = rngesus.Next() % Photos.Count;
                var secondPhoto = rngesus.Next() % Photos.Count;
                //TotalSlides.Add();
            }
        }
    }
}
