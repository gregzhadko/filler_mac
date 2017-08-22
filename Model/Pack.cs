using System.Collections.Generic;
using ReactiveUI;

namespace Model
{
    public class Pack : ReactiveObject
    {
        public int Id { get; set; }
        public string Language { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ReactiveList<PhraseItem> Phrases { get; set; } = new ReactiveList<PhraseItem>();

        public override string ToString() => Name;

        public string WholeName => $"{Id}. {Name}";

    }
}
