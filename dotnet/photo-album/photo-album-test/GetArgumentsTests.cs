using NUnit.Framework;
using photo_album;
using System;
using System.Linq;

namespace photo_album_test
{
    [TestFixture]
    public class GetArgumentsTests
    {
        [Test]
        public void TestNoArguments()
        {
            var arguments = new string[] { };
            var exception = Assert.Throws<FormatException>(delegate
            {
                Program.GetArguments(arguments);
            });
            Assert.That(exception.Message, Is.EqualTo("Invalid arguments"));
        }

        [Test]
        public void TestOnlyAlbumIdArgument()
        {
            var arguments = Program.GetArguments(new string[] { "1" });
            Assert.Multiple(() =>
            {
                Assert.That(arguments.Count, Is.EqualTo(1));
                Assert.That(arguments.FirstOrDefault(a => a.Key == "albumId").Value, Is.EqualTo("1"));
            });
        }

        [Test]
        public void TestAlbumIdAndDisplayCountArguments()
        {
            var arguments = Program.GetArguments(new string[] { "2", "-n 5" });
            Assert.Multiple(() =>
            {
                Assert.That(arguments.Count, Is.EqualTo(2));
                Assert.That(arguments.FirstOrDefault(a => a.Key == "albumId").Value, Is.EqualTo("2"));
                Assert.That(arguments.FirstOrDefault(a => a.Key == "displayCount").Value, Is.EqualTo("5"));
            });
        }

        [Test]
        public void TestAlbumIdAndDisplayCountAndStartPositionArguments()
        {
            var arguments = Program.GetArguments(new string[] { "3", "-n 10", "-s 5" });
            Assert.Multiple(() =>
            {
                Assert.That(arguments.Count, Is.EqualTo(3));
                Assert.That(arguments.FirstOrDefault(a => a.Key == "albumId").Value, Is.EqualTo("3"));
                Assert.That(arguments.FirstOrDefault(a => a.Key == "displayCount").Value, Is.EqualTo("10"));
                Assert.That(arguments.FirstOrDefault(a => a.Key == "startIndex").Value, Is.EqualTo("5"));
            });
        }
    }
}
