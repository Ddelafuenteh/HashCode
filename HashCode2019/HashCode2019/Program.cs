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
            foreach (var file in files)
            {
                Console.WriteLine($"Processing {file}");

                var loadedModel = Load(file);
                var model = loadedModel.Item1;

                Save(file, loadedModel.Item2);
            }
        }

        static (IList<Photo>, IList<Slide>, HashSet<Coincidence>) Load(string filePath)
        {
            var photosToReturn = new List<Photo>();
            var slidesToReturn = new List<Slide>();

            string filename = $"./../../../Inputs/{filePath}.txt";
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

                    photosToReturn.Add(new Photo() {
                        Orientation = content[0].ElementAt(0) == 'H' ? Orientation.Horizontal : Orientation.Vertical,
                        ID = currentPhotoId,
                        Tags = new HashSet<string>(tagArray)
                    });

                    foreach(var currentTag in tagArray)
                    {
                        var coincidence = coincidences.FirstOrDefault(val => val.Tag.Equals(currentTag));

                        if (coincidence == null)
                        {
                            coincidence = new Coincidence()
                            {
                                Tag = currentTag,
                                Matches = new List<Match>()
                            };

                            coincidences.Add(coincidence);
                        }

                        coincidence.Matches.Add(new Match() {
                            PhotoId = currentPhotoId,
                            NumberOfCoincidences = tagArray.Length
                        });
                    }

                    currentPhotoId++;
                }
            }

            return (photosToReturn.OrderByDescending(x => x.Tags.Count).ToList(), slidesToReturn, coincidences);
        }


        public static void Save(string filename, IList<Slide> slides)
        {
            string filePath = $"./../../../Outputs/{filename}.out";

            using (StreamWriter file = new StreamWriter(filePath))
            {
                file.WriteLine($"{slides.Count}");
                foreach (Slide slide in slides)
                {
                    foreach (Photo photo in slide.Photos)
                        file.Write($"{photo.ID} ");

                    file.Write("\n");
                }
            }
        }
    }
}
