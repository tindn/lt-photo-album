using NUnit.Framework;
using photo_album;

namespace photo_album_test
{
    [TestFixture]
    public class GetPhotosTests
    {
        [Test]
        public void GetPhotosForAlbumTest()
        {
            var photos = Program.GetPhotosForAlbum(1,"https://jsonplaceholder.typicode.com/photos");
            Assert.That(photos.Count, Is.EqualTo(50));
        }
    }
}
