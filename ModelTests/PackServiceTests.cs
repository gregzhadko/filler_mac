using Model;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ModelTests
{
    [TestClass]
    public class PackServiceTests
    {
        private readonly IPackService _service = new PackService();
        private int _testPackId = 20;
        private static string _testAuthor = "zhadko";
        private static readonly Random Random = new Random();

        [TestInitialize]
        public void Setup()
        {
            var pack = _service.GetPackByIdAsync(_testPackId).Result;
            _service.DeletePhrasesAsync(_testPackId, pack.Phrases.Select(p => p.Phrase), _testAuthor);
        }

        [TestMethod]
        public void DeletePhraseAsyncTest()
        {
            var phrase = GeneratePhrase();
            _service.AddPhraseAsync(_testPackId, phrase, _testAuthor).Wait();
            _service.DeletePhraseAsync(_testPackId, phrase.Phrase, _testAuthor).Wait();
            var phrases = _service.GetPackByIdAsync(_testPackId).Result.Phrases;
            Assert.IsFalse(phrases.Select(p => p.Phrase).Contains(phrase.Phrase));
        }

        [TestMethod]
        public void AddDuplicationPhraseToTheSamePackTest()
        {
            var phrase = GeneratePhrase();
            _service.AddPhraseAsync(_testPackId, phrase, _testAuthor).Wait();
            _service.AddPhraseAsync(_testPackId, phrase, _testAuthor).Wait();
            var phrases = _service.GetPackByIdAsync(_testPackId).Result.Phrases;
            CollectionAssert.AllItemsAreUnique(phrases.Select(p => p.Phrase).ToList());
        }

        [TestMethod]
        public void AddDuplicationPhraseToDifferentPackTest()
        {
            var existingPhrase = _service.GetPackByIdAsync(1).Result.Phrases.First();
            var newPhrase = existingPhrase.Clone();

            var result = _service.AddPhraseAsync(_testPackId, newPhrase, _testAuthor).Result;
            Assert.AreEqual("Word is already added to pack 1", result.Trim());
            var phrases = _service.GetPackByIdAsync(_testPackId).Result.Phrases;
            CollectionAssert.AllItemsAreUnique(phrases.Select(p => p.Phrase).ToList());

        }

        private static PhraseItem GeneratePhrase()
        {
            var phrase = new PhraseItem { Phrase = RandomString(15), Description = RandomString(50), Complexity = Random.Next(1, 5)};
            phrase.UpdateAuthor(_testAuthor);
            return phrase;
        }

        private static string RandomString(int length, string prefix = "")
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var randomString = new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());
            return prefix + randomString;
        }
    }
}

