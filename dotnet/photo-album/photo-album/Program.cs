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
                Console.WriteLine("Use photo-album [id] to get a list of photos for certain photo album");
                Console.WriteLine("For example 'photo-album 3' to get photos of album 3");
                Console.WriteLine(@"You can use argument -n to specify the number of photos to display. 
                                    Default number is 10.");
                Console.WriteLine(@"You can use argument -s to specify the starting position of the photos. 
                                    Default starting position is 1.");
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
            Console.WriteLine($"Album Id: {albumId}");
            var displayCount = match.Groups[3].Success ? int.Parse(match.Groups[3].Value) : 10;
            var startIndex = match.Groups[5].Success ? int.Parse(match.Groups[5].Value) : 0;
            var responseString = GetAlbumJsonString(albumId);
            var photos = JsonConvert.DeserializeObject<List<Photo>>(responseString);
            Console.WriteLine($"Album {albumId} has {photos.Count} photos");
            if (startIndex > photos.Count)
            {
                Console.WriteLine("The starting position exceeds the number of photos in album");
                return;
            }
            var displayEnd = displayCount > photos.Count - startIndex ? photos.Count 
                : displayCount + startIndex - 1;
            Console.WriteLine($"Showing photos from {startIndex} to {displayEnd}");
            photos.Skip(startIndex -1).Take(displayCount).ToList().ForEach(a =>
            {
                Console.WriteLine(a.Display());
            });
        }

        static string GetAlbumJsonString(int id)
        {
            var url = ConfigurationManager.AppSettings.Get("photo-album-source-url");
            var getAlbumTask = new HttpClient().GetAsync($"{url}?albumId={id}");
            getAlbumTask.Wait();
            var response = getAlbumTask.Result;
            var readTask = response.Content.ReadAsStringAsync();
            readTask.Wait();
            return readTask.Result;
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
