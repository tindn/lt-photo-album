using NUnit.Framework;
using photo_album;

namespace photo_album_test
{
    [TestFixture]
    public class GetDisplayEndTests
    {
        [Test]
        public void DisplayCountLessThanAvailableTest()
        {
            var displayEnd = Program.GetDisplayEnd(50, 1, 40);
            Assert.That(displayEnd, Is.EqualTo(40));
        }

        [Test]
        public void DisplayCountMoreThanAvailableTest()
        {
            var displayEnd = Program.GetDisplayEnd(50, 40, 20);
            Assert.That(displayEnd, Is.EqualTo(50));
        }
    }
}
