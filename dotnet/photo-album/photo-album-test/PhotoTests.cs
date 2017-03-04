using NUnit.Framework;
using photo_album;

namespace photo_album_test
{
    [TestFixture]
    public class PhotoTests
    {
        [Test]
        public void ToStringTest()
        {
            var photo = new Photo
            {
                Id = 5,
                Title = "Test title"
            };
            var result = photo.ToString();
            Assert.That(result, Is.EqualTo("[5] Test title"));
        }
    }
}
