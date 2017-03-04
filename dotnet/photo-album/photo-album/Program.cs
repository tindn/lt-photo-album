using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Configuration;
using System.Text.RegularExpressions;

namespace photo_album
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0 || args[0] == "-h")
            {
                DisplayHelp();
                return;
            }
            var inputRegex = new Regex(@"(\d+){1}(\s{1}-n\s(\d+))?(\s{1}-s\s(\d+))?");
            var inputString = string.Join(" ", args);
            var match = inputRegex.Match(inputString);
            if (!match.Success)
            {
                Console.WriteLine("Incorrect format");
                return;
            }

            int albumId = int.Parse(match.Groups[1].Value);
            var displayCount = match.Groups[3].Success ? int.Parse(match.Groups[3].Value) : 10;
            var startIndex = match.Groups[5].Success ? int.Parse(match.Groups[5].Value) : 0;

            var photos = GetPhotosForAlbum(albumId);
            Console.WriteLine($"Album {albumId} has {photos.Count} photos");
            if (startIndex > photos.Count)
            {
                Console.WriteLine("The starting position exceeds the number of photos in album");
                return;
            }

            DisplayPhotos(photos, startIndex, displayCount);
        }

        static List<Photo> GetPhotosForAlbum(int id)
        {
            var url = ConfigurationManager.AppSettings.Get("photo-album-source-url");
            var getAlbumTask = new HttpClient().GetAsync($"{url}?albumId={id}");
            getAlbumTask.Wait();
            var response = getAlbumTask.Result;
            var readTask = response.Content.ReadAsStringAsync();
            readTask.Wait();
            return JsonConvert.DeserializeObject<List<Photo>>(readTask.Result);
        }

        static void DisplayHelp()
        {
            Console.WriteLine("Use photo-album [id] to get a list of photos for certain photo album");
            Console.WriteLine("For example 'photo-album 3' to get photos of album 3");
            Console.WriteLine("You can use argument -n to specify the number of photos to display.");
            Console.WriteLine("Default number is 10. For example, '-n 15' to display 15 photos.");
            Console.WriteLine("You can use argument -s to specify the starting position of the photos.");
            Console.WriteLine("Default starting position is 1. For example, '-s 5' to start from the fifth photo.");
        }

        static void DisplayPhotos(List<Photo> photos, int startIndex, int displayCount)
        {
            var displayEnd = displayCount > photos.Count - startIndex ? photos.Count
                            : displayCount + startIndex - 1;
            Console.WriteLine($"Showing photos from {startIndex} to {displayEnd}");
            photos.Skip(startIndex - 1).Take(displayCount).ToList().ForEach(a =>
             {
                 Console.WriteLine(a.Display());
             });
        }
    }

    class Photo
    {
        public int AlbumId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }

        public string Display()
        {
            return $"[{this.Id}] {this.Title}";
        }
    }
}
