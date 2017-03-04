using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Configuration;

namespace photo_album
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0 || args[0] == "-h")
            {
                Console.WriteLine("Use photo-album [id] to get a list of photos for certain photo album");
                Console.Write("For example 'photo-album 3' to get photos of album 3");
                return;
            }

            int albumId;
            if (!int.TryParse(args[0], out albumId))
            {
                Console.WriteLine("Invalid album id.");
                return;
            }
            Console.WriteLine($"Album Id: {albumId}");
            var responseString = GetAlbumJsonString(albumId);
            var albums = JsonConvert.DeserializeObject<List<Album>>(responseString);
            Console.WriteLine($"Album {albumId} has {albums.Count} photos");
            Console.WriteLine($"Showing the first 10 photos");
            albums.Take(10).ToList().ForEach(a =>
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

    class Album
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
