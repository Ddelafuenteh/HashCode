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
            string[] files = new string[] { "a_example", "b_lovely_landscapes", "c_memorable_moments", "d_pet_pictures", "e_shiny_selfies" };
            foreach (var file in files)
            {
                Console.WriteLine($"Processing {file}");

                var loadedModel = Load(file);
                var result = Run(loadedModel.Item1.ToList());
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

        public static List<Slide> Run(List<Slide> slides)
        {
            List<Slide> SalidaProcesada;
            List<Slide> SalidaAProcesar;
            List<Slide> slideShowOutput = new List<Slide>();

            List<Slide> slideVertical = slides.Where(x => x.Photos[0].Orientation == Orientation.Vertical).ToList();
            List<Slide> slideHorizontal = slides.Where(x => x.Photos[0].Orientation == Orientation.Horizontal).ToList();

            bool AllVertical = slides.TrueForAll(x => x.Photos[0].Orientation == Orientation.Vertical);
            bool AllHorizontal = slides.TrueForAll(x => x.Photos[0].Orientation == Orientation.Horizontal);
            bool proccesedWithBestSlides = AllVertical == AllHorizontal;
            if (!AllVertical)
            {

                int middleSize = slideVertical.Count / 2;

                List<Slide> slideVerticalBeforeMiddle = slideVertical.Take(middleSize).ToList();
                List<Slide> restSlideVertical = slideVertical.TakeLast(middleSize).ToList();

                GetAllSlidesOrder(slideShowOutput, slideVerticalBeforeMiddle, restSlideVertical);
                List<Slide> SlideVerticalDsscending = slideShowOutput.OrderByDescending(x => x.Tags.Count).ToList();
                SalidaProcesada = SlideVerticalDsscending.Concat(slideHorizontal).OrderByDescending(x => x.Tags.Count).ToList();
                SalidaAProcesar = SlideVerticalDsscending.Concat(slideHorizontal).OrderByDescending(x => x.Tags.Count).ToList();

            }
            else
            {
                GetAllSlidesOrderDescending(slides,slideShowOutput);
                SalidaAProcesar = new List<Slide>(slideShowOutput);
                SalidaProcesada = new List<Slide>(slideShowOutput);
            }


            

            List<Slide> Salida = new List<Slide>();
            
            foreach (Slide currentSlide in SalidaAProcesar)
            {
                if (proccesedWithBestSlides) {
                    Salida.Add(currentSlide);
                    SalidaProcesada.Remove(currentSlide);
                    Slide slide = Utils.GetBestSlice(currentSlide, SalidaProcesada);
                    if(slide != null) { 
                        Salida.Add(slide);
                        SalidaProcesada.Remove(slide);
                    }
                }
                else
                { 
                    Salida.Add(currentSlide);
                    SalidaProcesada.Remove(currentSlide);
                }
            }

            return Salida;
        }
        private static void GetAllSlidesOrder(List<Slide> output, List<Slide> slideShowA, List<Slide> slideShowD)
        {
            if (!slideShowA.Any() || !slideShowD.Any())
                return;

            for (int i=0; i < slideShowA.Count; i++)
            {
                Slide slideVertical = new Slide(slideShowA[i].Photos[0], slideShowA[i].Tags);
                slideVertical.Photos.Add(slideShowD[i].Photos[0]);
                slideVertical.Tags = slideVertical.Tags.Union(slideShowD[i].Tags).ToHashSet();
                output.Add(slideVertical);
            }
        
        }


        private static void GetAllSlidesOrderDescending(List<Slide> slideShow, List<Slide> slideShowOutput)
        {
            int positionVertical = -1;
            for(int i = 0; i < slideShow.Count-1; i++) {
                if (slideShow[i].Photos[0].Orientation == Orientation.Vertical)
                {
                    if (positionVertical == -1)
                    {
                        positionVertical = slideShowOutput.Count - 1;
                        slideShowOutput.Add(new Slide(slideShow[i].Photos[0], slideShow[i].Tags));
                    }
                    else
                    {
                        slideShowOutput[positionVertical].Photos.Add(slideShow[i].Photos[0]);
                        slideShowOutput[positionVertical].Tags = slideShowOutput[positionVertical].Tags.Union(slideShow[i].Tags).ToHashSet();
                        positionVertical = -1;
                    }
                }
                else
                {
                    Slide slideHorizontal = new Slide(slideShow[i].Photos[0], slideShow[i].Tags);
                    slideShowOutput.Add(slideHorizontal);
                }
            }     
        }
    }
}
  