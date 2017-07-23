using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface IPackService
    {
        Task<string> AddPhraseAsync(int packId, PhraseItem phrase);
        Task<string> DeletePhraseAsync(int packId, string phrase, string author);
        Task EditPackAsync(int id, string name, string description);

        Task EditPhraseAsync(int packId, PhraseItem oldPhrase, PhraseItem newPhrase, string selectedAuthor);

        Task ReviewPhraseAsync(int packId, PhraseItem phrase, string reviewerName, State state);
        Task<IEnumerable<Pack>> GetAllPacksInfoAsync();

        Task<Pack> GetPackByIdAsync(int id);

        Task<List<PhraseEditInfo>> GetPackEditingInfoAsync(Dictionary<int, string> packDictionary);
    }
}
