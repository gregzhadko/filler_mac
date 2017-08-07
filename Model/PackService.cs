using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Model
{
    public class PackService : IPackService
    {
        private static readonly string Url;
        private static readonly string Username;
        private static readonly string Password;

        static PackService()
        {
            var settings = File.ReadAllLines("settings.txt");
            Url = settings[0];
            Username = settings[1];
            Password = settings[2];
        }

        public async Task<string> AddPhraseAsync(int packId, PhraseItem phrase, string author)
        {
            return await GetResponseAsync(
                $"addPackWordDescription?id={packId}&word={phrase.Phrase}&description={phrase.Description}&level={phrase.Complexity}&author={author}",
                8091).ConfigureAwait(false);
        }

        public Task<string> DeletePhraseAsync(int packId, string phrase, string author) => GetResponseAsync(
            $"removePackWord?id={packId}&word={phrase}&author={author}", 8091);

        public Task<string[]> DeletePhrasesAsync(int packId, IEnumerable<string> phrases, string author)
        {
            var tasks = new List<Task<string>>();
            foreach (var phrase in phrases)
            {
                var task = DeletePhraseAsync(packId, phrase, author);
                tasks.Add(task);
            }

            var deletePhrasesAsync = Task.WhenAll(tasks);
            deletePhrasesAsync.ConfigureAwait(false);
            return deletePhrasesAsync;
        }

        public async Task EditPackAsync(int id, string name, string description)
        {
            if (id == 0)
            {
                return;
            }

            await GetResponseAsync($"updatePackInfo?id={id}&name={name}&description={description}", 8091).ConfigureAwait(false);
        }

        public async Task<string> EditPhraseAsync(int packId, PhraseItem oldPhrase, PhraseItem newPhrase, string selectedAuthor)
        {
            if (oldPhrase.Phrase != newPhrase.Phrase)
            {
                await DeletePhraseAsync(packId, oldPhrase.Phrase, selectedAuthor).ConfigureAwait(false);
            }

            if (!string.Equals(oldPhrase.Phrase, newPhrase.Phrase, StringComparison.Ordinal) ||
                Math.Abs(oldPhrase.Complexity - newPhrase.Complexity) > 0.01 ||
                !string.Equals(oldPhrase.Description, newPhrase.Description, StringComparison.Ordinal))
            {
                return await GetResponseAsync(
                    $"addPackWordDescription?id={packId}&word={newPhrase.Phrase}&description={newPhrase.Description}&level={newPhrase.Complexity}&author={selectedAuthor}",
                    8091).ConfigureAwait(false);
            }

            return await new Task<string>(() => "");
        }

        public Task ReviewPhraseAsync(int packId, PhraseItem phrase, string reviewerName, State state)
        {
            return GetResponseAsync($"reviewPackWord?id={packId}&word={phrase.Phrase}&author={reviewerName}&state={(int)state}", 8091);
        }

        public async Task<IEnumerable<Pack>> GetAllPacksInfoAsync()
        {
            var packsInfo = await GetPacksInfoAsync(8081).ConfigureAwait(false);
            return packsInfo.Select(p => new Pack
            {
                Id = Convert.ToInt32(p.Value<int>("id")),
                Name = p["name"].ToString()
            }).OrderBy(p => p.Id);
        }

        public async Task<Pack> GetPackByIdAsync(int id)
        {
            if (id == 0)
            {
                return null;
            }

            var response = await GetResponseAsync($"getPack?id={id}", 8081).ConfigureAwait(false);

            var pack = JsonConvert.DeserializeObject<Pack>(response);
            if (pack.Phrases == null)
            {
                pack.Phrases = new List<PhraseItem>();
            }
            pack.Phrases = pack.Phrases.OrderBy(p => p.Phrase).ToList();
            return pack;
        }

        public List<Tuple<int, string>> GetPorts() => new List<Tuple<int, string>>
        {
            new Tuple<int, string>(8081, "test")
        };

        private static async Task<IEnumerable<JToken>> GetPacksInfoAsync(int port)
        {
            var response = await GetResponseAsync("getPacks", port).ConfigureAwait(false);
            var jObject = JObject.Parse(response)["packs"];
            var packs = jObject.Select(i => i["pack"]);
            return packs;
        }

        public async Task<int[]> GetPackIdsAsync()
        {
            var packInfo = await GetPacksInfoAsync(8081).ConfigureAwait(false);
            return packInfo.Select(p => Convert.ToInt32(p["id"])).ToArray();
        }

        private static async Task<string> GetResponseAsync(string requestUriString, int port)
        {
            var protocol = "http";
            var request = WebRequest.Create($"{protocol}://{Url}:{port}/" + requestUriString);

            request.Credentials = new NetworkCredential(Username, Password);

            try
            {
                using (var response = await request.GetResponseAsync().ConfigureAwait(false))
                {
                    using (var dataStream = response.GetResponseStream())
                    {
                        if (dataStream == null || dataStream == Stream.Null)
                        {
                            throw new WebException("Stream is null");
                        }

                        var reader = new StreamReader(dataStream);
                        return await reader.ReadToEndAsync().ConfigureAwait(false);
                    }
                }
            }
            catch (WebException ex)
            {
                return await new StreamReader(ex.Response.GetResponseStream()).ReadToEndAsync().ConfigureAwait(false);
            }

        }

        public async Task<List<PhraseEditInfo>> GetPackEditingInfoAsync(Dictionary<int, string> packDictionary)
        {
            var response = await GetResponseAsync("packEditingInfo", 8091).ConfigureAwait(false);
            var jArray = JArray.Parse(response);

            var result = new List<PhraseEditInfo>();
            foreach (var obj in jArray)
            {
                var info = obj.ToObject<PhraseEditInfo>();
                info.PackName = packDictionary[info.Pack];
                var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                info.Date = dtDateTime.AddSeconds(info.Timestamp).ToLocalTime();
                result.Add(info);
            }

            return result;
        }

    }
}
