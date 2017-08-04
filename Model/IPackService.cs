using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Model
{
    public interface IPackService
    {
        Task<string> AddPhraseAsync(int packId, [NotNull]PhraseItem phrase, [NotNull]string author);
        Task<string> DeletePhraseAsync(int packId, [NotNull]string phrase, [NotNull]string author);
        Task EditPackAsync(int id, [NotNull]string name, [NotNull]string description);

        Task<string> EditPhraseAsync(int packId, [NotNull]PhraseItem oldPhrase, [NotNull]PhraseItem newPhrase, [NotNull]string selectedAuthor);

        Task ReviewPhraseAsync(int packId, [NotNull]PhraseItem phrase, [NotNull]string reviewerName, State state);
        Task<IEnumerable<Pack>> GetAllPacksInfoAsync();

        Task<Pack> GetPackByIdAsync(int id);

        Task<List<PhraseEditInfo>> GetPackEditingInfoAsync([NotNull]Dictionary<int, string> packDictionary);
        Task<string[]> DeletePhrasesAsync(int packId, [NotNull]IEnumerable<string> phrases, [NotNull] string author);
    }
}
