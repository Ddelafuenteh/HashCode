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
            string[] files = new string[] { "a_example","c_memorable_moments", "d_pet_pictures", "e_shiny_selfies" };
            foreach (var file in files)
            {
                Console.WriteLine($"Processing {file}");

                var loadedModel = Load(file);
                var result = Run(loadedModel.Item1.ToList(), loadedModel.Item2.ToList());
                Save(file, result);

            }
        }

        public static List<Slide> Run(List<Slide> slidesH, List<Photo> photos)
        {

            slidesH.AddRange(GetVerticalSlides(photos)); //Añado en la lista de Horizontales las Verticales.

            var slidesOrderByScore = GetSlidesOrderByScore(slidesH);

            return slidesOrderByScore;
        }

        public static List<Slide> GetVerticalSlides(List<Photo> verticalPhotos)
        {
            List<Slide> verticalSlides = new List<Slide>();

            while (verticalPhotos.Count > 1) //para un slide necesito dos verticales Por lo que si tenemos el numero de fotos es impar no la utilizo.
            {
                var FirstPhoto = verticalPhotos[0];
                var SecondPhoto = verticalPhotos[1];
                var bestPositionSecondPhoto = SecondPhoto;
                var bestNumNotRepeatedTags = FirstPhoto.Tags.Intersect(bestPositionSecondPhoto.Tags).Count();

                if (bestNumNotRepeatedTags == 0) //No tienen ningún tag repetido.
                {
                    SecondPhoto = bestPositionSecondPhoto;
                    verticalSlides.Add(new Slide(FirstPhoto, SecondPhoto));
                    verticalPhotos.Remove(FirstPhoto);
                    verticalPhotos.Remove(SecondPhoto);
                }
                else
                {
                    if (verticalPhotos.Count == 2) //Caso muy particular solo quedan 2 fotos.
                    {
                        SecondPhoto = verticalPhotos[1];
                        verticalSlides.Add(new Slide(FirstPhoto, SecondPhoto));
                        verticalPhotos.Remove(FirstPhoto);
                        verticalPhotos.Remove(SecondPhoto);
                    }
                    else if (verticalPhotos.Count > 2)
                    {
                        int index = 2; //Puede ser confuso pero como hemos asignado al inicio del bucle firstPhoto y secondPhoto que son [0] y [1] tenemos que empezar a buscar la mejor desde la segunda posicion.
                        for (int i = 2; i < verticalPhotos.Count; i++)
                        {
                            var numRepeatedTags = FirstPhoto.Tags.Intersect(verticalPhotos[i].Tags).Count();

                            if (numRepeatedTags == 0) //No tienen ningún tag repetido.
                            {
                                index = i;
                                break;
                            }
                            else if (numRepeatedTags < bestNumNotRepeatedTags)
                            {
                                index = i;
                                bestNumNotRepeatedTags = numRepeatedTags;
                            }
                        }
                        SecondPhoto = verticalPhotos[index];
                        verticalSlides.Add(new Slide(FirstPhoto, SecondPhoto)); //posicion de la buena foto del bucle anterior.
                        verticalPhotos.Remove(FirstPhoto);
                        verticalPhotos.Remove(SecondPhoto);
                    }
                }
            }

            return verticalSlides;
        }

        public static List<Slide> GetSlidesOrderByScore(List<Slide> slides)
        {
            var slidesList = new LinkedList<Slide>(slides);
            var finalSlides = new List<Slide>(slidesList.Take(1).ToList());
            slidesList.RemoveFirst();


            while (slidesList.Any())
            {
                var prev = finalSlides.Last(); //Nodo ya insertado, me sirve para comprobar la puntuacion con la siguiente SLIDE.
                LinkedListNode<Slide> bestPositionSlide = slidesList.First; //Primera slide a comprobar
                LinkedListNode<Slide> SlideNode = slidesList.First; //Primera slide a comprobar
                var bestScore = Utils.GetInterestValue(prev, bestPositionSlide.Value); //puntuacion primera slide

                int counter = 0; //solo sirve para cortar el numero de nodos a comprobar.
                while (SlideNode != null)
                {
                    if (counter++ > 10000) break; //Corto por optimización (Cuanto mayor valor aquí mas posibilidad de conseguir un buen resultado)
                    if (SlideNode.Value.Tags.Count / 2 <= bestScore) break; //Maxima puntuacion que puedo conseguir entre dos Slides. Si no es la máxima compruebo la siguiente.
                    var nextNode = SlideNode.Next; //nuevo nodo a comprobar
                    var score = Utils.GetInterestValue(prev, SlideNode.Value); //puntuacion slide anterior con la nueva.
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestPositionSlide = SlideNode;
                    }
                    SlideNode = nextNode;
                }

                finalSlides.Add(bestPositionSlide.Value);
                slidesList.Remove(bestPositionSlide);
            }

            return finalSlides;
        }

        #region "Load and Save"
        static (IList<Slide>, IList<Photo>) Load(string filePath)
        {
            var photosToReturn = new List<Photo>();
            var slidesToReturn = new List<Slide>();

            string filename = $"Inputs/{filePath}.txt";
            var currentPhotoId = 0;

            using (var file = new StreamReader(filename, Encoding.Default))
            {
                file.ReadLine();

                while (!file.EndOfStream)
                {
                    var content = file.ReadLine().Split(' ');
                    string[] tagArray = new string[int.Parse(content[1])];
                    Array.Copy(content, 2, tagArray, 0, int.Parse(content[1]));

                    var currentPhoto = new Photo()
                    {
                        Orientation = content[0].ElementAt(0) == 'H' ? Orientation.Horizontal : Orientation.Vertical,
                        ID = currentPhotoId,
                        Tags = tagArray.ToHashSet()
                    };

                    if (currentPhoto.Orientation == Orientation.Horizontal)
                        slidesToReturn.Add(new Slide(currentPhoto, tagArray.ToHashSet()));
                    else
                        photosToReturn.Add(currentPhoto);

                    currentPhotoId++;
                }
            }

            return (slidesToReturn.OrderBy(x => x.Tags.Count).ToList(), photosToReturn);
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

        #endregion
    }
}
  