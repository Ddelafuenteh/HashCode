using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HashCode2019
{
    public class Program
    {
        public static HashSet<Slide> selectedSlides = new HashSet<Slide>();

        static void Main(string[] args)
        {
            string[] files = new string[] { "a_example", "b_lovely_landscapes" ,"c_memorable_moments", "d_pet_pictures", "e_shiny_selfies" };
            foreach (var file in files)
            {
                Console.WriteLine($"Processing {file}");

                var loadedModel = Load(file);
                var result = Run(loadedModel.Item1);
                Save(file, result);

            }
        }

        static (IList<Slide>, HashSet<Coincidence>) Load(string filePath)
        {
            var photosToReturn = new List<Photo>();
            var slidesToReturn = new List<Slide>();

            string filename = $"Inputs/{filePath}.txt";
            var currentPhotoId = 0;

            var coincidences = new HashSet<Coincidence>();

            using (var file = new StreamReader(filename, Encoding.Default))
            {

                var totalPhotos = file.ReadLine().Split(' ').Select(val => int.Parse(val)).ToArray();
                int positionVertical = -1;
                int cont = 0;
                while (!file.EndOfStream)
                {
                    var content = file.ReadLine().Split(' ');
                    string[] tagArray = new string[int.Parse(content[1])];
                    Array.Copy(content, 2, tagArray, 0, int.Parse(content[1]));

                    var currentPhoto = new Photo()
                    {
                        Orientation = content[0].ElementAt(0) == 'H' ? Orientation.Horizontal : Orientation.Vertical,
                        ID = currentPhotoId
                    };

                    slidesToReturn.Add(new Slide(currentPhoto,tagArray.ToHashSet()));                

                    currentPhotoId++;
                }
            }


            

            return (slidesToReturn.OrderByDescending(x => x.Tags.Count).ToList(), coincidences);
        }


        public static void Save(string filename, IList<Slide> slides)
        {
            string filePath = $"Outputs/{filename}.out";

                using (StreamWriter file = new StreamWriter(filePath))
                {
                    file.WriteLine($"{slides.Count}");
                    foreach (Slide slide in slides)
                    {
                        var ids = slide.Photos.Select(val => val.ID.ToString()).ToArray();
                        file.WriteLine(string.Join(' ', ids));
                    }
                }

        }

        public static List<Slide> Run(IList<Slide> slides)
        {
            List<Slide> slideShow = new List<Slide>();

            GetAllSlidesOrderDescending(slides, slideShow);

            List<Slide> bestSlidesScores = new List<Slide>();
            List<Slide> SlidesScoresProccessed = new List<Slide>(slideShow);

            foreach (var slide in slideShow)
            {
                Slide bestSlide = Utils.GetBestSlice(slide, SlidesScoresProccessed);
                bestSlidesScores.Add(bestSlide);
                SlidesScoresProccessed.Remove(bestSlide);

            }

            return bestSlidesScores.Union(SlidesScoresProccessed).ToList();
        }

        private static void GetAllSlidesOrderDescending(IList<Slide> slides, List<Slide> slideShow)
        {
            int positionVertical = -1;
            foreach (var slide in slides)
            {
                if (slide.Photos[0].Orientation == Orientation.Vertical)
                {
                    if (positionVertical == -1)
                    {
                        slideShow.Add(new Slide(slide.Photos[0], slide.Tags));
                        positionVertical = slideShow.Count - 1;
                    }
                    else
                    {
                        slideShow[positionVertical].Photos.Add(slide.Photos[0]);
                        slideShow[positionVertical].Tags = slideShow[positionVertical].Tags.Union(slide.Tags).ToHashSet();
                        positionVertical = -1;
                    }
                }
                else
                {
                    slideShow.Add(new Slide(slide.Photos[0], slide.Tags));
                }

            }
            slideShow = slideShow.OrderByDescending(x => x.Tags.Count).ToList();
        }
    }
}
  