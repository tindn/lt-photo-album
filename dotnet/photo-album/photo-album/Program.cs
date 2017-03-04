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
                var helpText = GetHelp();
                Console.WriteLine(helpText);
                return;
            }
            try
            {
                var arguments = GetArguments(args);
                int albumId = int.Parse(arguments.First(a => a.Key == "albumId").Value ?? "0");
                int displayCount = int.Parse(arguments.FirstOrDefault(a => a.Key == "displayCount").Value ?? "10");
                int startIndex = int.Parse(arguments.FirstOrDefault(a => a.Key == "startIndex").Value ?? "1");
                var photos = GetPhotosForAlbum(albumId);
                Console.WriteLine($"Album {albumId} has {photos.Count} photos");
                if (startIndex > photos.Count)
                {
                    Console.WriteLine("The starting position exceeds the number of photos in album");
                    return;
                }
                Console.WriteLine($"Showing photos from {startIndex} to {GetDisplayEnd(photos.Count, startIndex, displayCount)}");
                photos.Skip(startIndex - 1).Take(displayCount).ToList().ForEach(a =>
                 {
                     Console.WriteLine(a.ToString());
                 });
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }

        static List<KeyValuePair<string, string>> GetArguments(string[] args)
        {
            var inputRegex = new Regex(@"(\d+){1}(\s-([a-zA-Z])\s(\d+))*");
            var inputString = string.Join(" ", args);
            var match = inputRegex.Match(inputString);
            if (!match.Success)
            {
                throw new FormatException("Invalid arguments");
            }
            var arguments = new List<KeyValuePair<string, string>>();
            arguments.Add(new KeyValuePair<string, string>("albumId", match.Groups[1].Value));
            if (match.Groups[3].Success && match.Groups[4].Success)
            {
                for (var i = 0; i < match.Groups[3].Captures.Count; i++)
                {
                    switch (match.Groups[3].Captures[i].Value)
                    {
                        case "n":
                            arguments.Add(new KeyValuePair<string, string>("displayCount", match.Groups[4].Captures[i].Value));
                            break;
                        case "s":
                            arguments.Add(new KeyValuePair<string, string>("startIndex", match.Groups[4].Captures[i].Value));
                            break;
                        default:
                            break;
                    }
                }
            }
            return arguments;
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

        static string GetHelp()
        {
            return @"Use photo-album [id] to get a list of photos for certain photo album
For example 'photo-album 3' to get photos of album 3
You can use argument -n to specify the number of photos to display.
Default number is 10. For example, '-n 15' to display 15 photos.
You can use argument -s to specify the starting position of the photos
Default starting position is 1. For example, '-s 5' to start from the fifth photo.";
        }

        static int GetDisplayEnd(int photosCount, int startIndex, int displayCount)
        {
            return displayCount > photosCount - startIndex ? photosCount : displayCount + startIndex - 1;
        }
    }

    class Photo
    {
        public int AlbumId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }

        public override string ToString()
        {
            return $"[{this.Id}] {this.Title}";
        }
    }
}
