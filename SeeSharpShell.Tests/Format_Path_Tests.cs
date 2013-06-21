using System;
using System.Collections;
using System.IO;
using System.Management.Automation;
using NUnit.Framework;

namespace SeeSharpShell.Tests {
    [TestFixture]
    public class Format_Path_Tests {
        [Test]
        public void Ctor_ShouldCreateCmdLet()
        {
            var formatPath = new Format_Path();
            var cmdlet = formatPath as Cmdlet;

            Assert.IsTrue(cmdlet != null);
        }

        [Test]
        public void HomePath_ShouldReturnShortenedPathWithTilde()
        {
            string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string testPath = Path.Combine(home, @"Hello\World");
            const string pathToValidate = @"~\H\World";

            var cmd = new Format_Path {Path = testPath};
            IEnumerator result = cmd.Invoke().GetEnumerator();

            Assert.IsTrue(result.MoveNext());
            Assert.IsTrue(result.Current is string);

            Assert.That(result.Current.ToString(), Is.EqualTo(pathToValidate));
        }

        [Test]
        public void LocalPath_ShouldReturnShortenedPath()
        {
            const string testPath = @"C:\Hello\World";
            const string pathToValidate = @"C:\H\World";

            var cmd = new Format_Path {Path = testPath};
            IEnumerator result = cmd.Invoke().GetEnumerator();

            Assert.IsTrue(result.MoveNext());
            Assert.IsTrue(result.Current is string);

            Assert.That(result.Current.ToString(), Is.EqualTo(pathToValidate));
        }

        [Test]
        public void UNCPath_ShouldReturnShortenedPath()
        {
            const string testPath = @"\\SERVER\Hello\World";
            const string pathToValidate = @"\\S\H\World";

            var formatPath = new Format_Path {Path = testPath};
            IEnumerator result = formatPath.Invoke().GetEnumerator();

            Assert.IsTrue(result.MoveNext());
            Assert.IsTrue(result.Current is string);

            Assert.That(result.Current.ToString(), Is.EqualTo(pathToValidate));
        }
    }
}
