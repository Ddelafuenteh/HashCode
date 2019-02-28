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
            string[] files = new string[] { "example", "small", "medium", "big" };
            foreach (var file in files)
            {
                Console.WriteLine($"Processing {file}");
                var model = Load(file);
                //model.Run();
                //PUNTOS++
                //prueba
                Save(file, null);
            }
        }

        static IList<Photo> Load(string filePath)
        {
            var toRet = new List<Photo>();

            using (var file = new StreamReader($"Inputs/{ filePath }", Encoding.Default))
            {
                var totalPhotos = file.ReadLine().Split(' ').Select(val => int.Parse(val)).ToArray();

                while (!file.EndOfStream)
                {
                    var content = file.ReadLine().Split(' ');
                    string[] leftover = new string[content.Length - 2];
                    Array.Copy(content, 2, leftover, 0, content.Length - 2);

                    toRet.Add(new Photo() {
                        Orientation = content[0].ElementAt(0) == 'H' ? Orientation.Horizontal : Orientation.Vertical,
                        ID = int.Parse(content[1]),
                        Tags = new HashSet<string>(leftover)
                    });
                }
            }

            return toRet;
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
