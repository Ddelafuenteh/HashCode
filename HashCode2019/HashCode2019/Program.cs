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

        static object Load(string filePath)
        {
            // string filename = $"Inputs/{file}.in";
            // string[] fileContent = File.ReadAllLines(filename);
            // int[] parameters = fileContent[0].Split(' ').Select((s) => int.Parse(s)).ToArray();

            // int rows = parameters[0];
            // int columns = parameters[1];

            ////Read file content

            // return null;

            //var toLoad = new Pizza();

            using (var file = new StreamReader($"Inputs/{ filePath }", Encoding.Default))
            {
                var firstInput = file.ReadLine().Split(' ').Select(val => int.Parse(val)).ToArray();

                // Header information
                //toLoad.Information = new PizzaInformation()
                //{
                //    TotalAmountOfRows = firstInput[0],
                //    TotalAmountOfColumns = firstInput[1],
                //    MinimumAmountPerIngredient = firstInput[2],
                //    MaximumIngredientsInTotal = firstInput[3]
                //};

                //toLoad.Deliciousness = new char[firstInput[0]][];
                var currentIndex = 0;

                while (!file.EndOfStream)
                {
                    var content = file.ReadLine();
                    //toLoad.Deliciousness[currentIndex++] = file.ReadLine().ToCharArray();
                }
            }

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
