using System;
using System.Collections.Generic;
using System.Text;

namespace HashCode2019
{
    public class SlideShow
    {
        public Queue<Slide> SlidesTransition { get; set; }

        public SlideShow()
        {
            SlidesTransition = new Queue<Slide>();
        }
        
    }

}
