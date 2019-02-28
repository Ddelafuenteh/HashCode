﻿using System;
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
                var model = Load(file).Item1;
                //model.Run();
                //PUNTOS++
                //prueba
                Save(file, null);
            }
        }

        static (IList<Photo>, HashSet<Coincidence>) Load(string filePath)
        {
            var toRet = new List<Photo>();
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

                    toRet.Add(new Photo() {
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

            return (toRet.OrderByDescending(x => x.Tags.Count).ToList(), coincidences);
        }


        public static void Save(string file, object o)
        {
            string filename = $"Outputs/{file}.out";
            List<string> content = new List<string>();
            //Get object properties

            File.WriteAllLines(filename, content.ToArray());
        }
    }
}
