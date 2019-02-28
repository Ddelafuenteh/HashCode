using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HashCode2019
{
    public class Program
    {
        static void Main(string[] args)
        {
            string[] files = new string[] { "a_example", "b_lovely_landscapes", "c_memorable_moments", "d_pet_pictures", "e_shiny_selfies" };
            //string[] files = new string[] { "b_lovely_landscapes" };
            foreach (var file in files)
            {
                Console.WriteLine($"Processing {file}");

                var loadedModel = Load(file);
                var model = loadedModel.Item1;

                var algorithm = new Algorithm()
                {
                    Photos = loadedModel.Item1,
                    TotalSlides = loadedModel.Item2
                };

                var result = algorithm.Execute();

                SaveEnumerable(file, result.SlidesTransition, result.SlidesTransition.Count);
            }
        }

        static (IList<Photo>, IList<Slide>, HashSet<Coincidence>) Load(string filePath)
        {
            var photosToReturn = new List<Photo>();
            var slidesToReturn = new List<Slide>();

            string filename = $"Inputs/{filePath}.txt";
            var currentPhotoId = 0;

            var coincidences = new HashSet<Coincidence>();

            using (var file = new StreamReader(filename, Encoding.Default))
            {
                var totalPhotos = file.ReadLine().Split(' ').Select(val => int.Parse(val)).ToArray();

                while (!file.EndOfStream)
                {
                    var content = file.ReadLine().Split(' ');
                    string[] tagArray = new string[int.Parse(content[1])];
                    Array.Copy(content, 2, tagArray, 0, int.Parse(content[1]));

                    var currentPhoto = new Photo()
                    {
                        Orientation = content[0].ElementAt(0) == 'H' ? Orientation.Horizontal : Orientation.Vertical,
                        ID = currentPhotoId,
                        Tags = new HashSet<string>(tagArray)
                    };

                    if (currentPhoto.Orientation == Orientation.Vertical)
                    {
                        photosToReturn.Add(currentPhoto);
                    }
                    else
                    {
                        slidesToReturn.Add(new Slide(currentPhoto));
                    }

                    currentPhotoId++;
                }
            }

            return (photosToReturn.OrderByDescending(x => x.Tags.Count).ToList(), 
                        slidesToReturn.OrderByDescending(x => x.Tags.Count).ToList(), 
                        coincidences);
        }


        public static void Save(string filename, IList<Slide> slides)
        {
            string filePath = $"./../../../Outputs/{filename}.out";

            using (StreamWriter file = new StreamWriter(filePath, false, Encoding.Default))
            {
                file.WriteLine($"{slides.Count}");
                foreach (Slide slide in slides)
                {
                    var ids = slide.Photos.Select(val => val.ID.ToString()).ToArray();
                    file.WriteLine(string.Join(' ', ids));
                }
            }
        }

        public static void SaveEnumerable(string filename, IEnumerable<Slide> slides, int totalCount)
        {
            string filePath = $"./../../../Outputs/{filename}.out";

            using (StreamWriter file = new StreamWriter(filePath, false, Encoding.Default))
            {
                file.WriteLine($"{ totalCount }");
                foreach (Slide slide in slides)
                {
                    var ids = slide.Photos.Select(val => val.ID.ToString()).ToArray();
                    file.WriteLine(string.Join(' ', ids));
                }
            }
        }
    }
}
