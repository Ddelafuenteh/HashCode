using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        static object Load(string file)
        {
            string filename = $"Inputs/{file}.in";
            string[] fileContent = File.ReadAllLines(filename);
            int[] parameters = fileContent[0].Split(' ').Select((s) => int.Parse(s)).ToArray();

            int rows = parameters[0];
            int columns = parameters[1];

           //Read file content

            return null;
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
