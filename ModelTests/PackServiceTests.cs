using Model;
using NUnit.Framework;
using System;
using System.Linq;

namespace ModelTests
{
    [TestFixture]
    public class PackServiceTests
    {
        private IPackService _service = new PackService();
        private int _testPackId = 20;
        private string _testAuthor = "zhadko";
        private static readonly Random Random = new Random();

        [Test]
        public void DeletePhraseAsyncTest()
        {
            var phrase = GeneratePhrase();
            _service.AddPhraseAsync(_testPackId, phrase).Start();
            _service.DeletePhraseAsync(_testPackId, phrase.Phrase, _testAuthor);

        }

        private static PhraseItem GeneratePhrase()
        {
            return new PhraseItem { Phrase = RandomString(15), Description = RandomString(50), Complexity = Random.Next(1, 5) };
        }

        private static string RandomString(int length, string prefix = "")
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var randomString = new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());
            return prefix + randomString;
        }
    }
}
