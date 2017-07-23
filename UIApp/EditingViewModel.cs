using System;
using System.Collections.Generic;
using System.Text;
using Model;
using ReactiveUI;

namespace UIApp
{
    public class EditingViewModel : ReactiveObject
    {
        private IPackService _service = new PackService();

        public EditingViewModel(PhraseItem phrase)
        {
            Phrase = phrase;
            SaveCommand = ReactiveCommand.Create();
            SaveCommand.Subscribe(_ =>
            {
                
            });
        }

        private PhraseItem _phrase = new PhraseItem() {Complexity = 1, Description = "Bla bla bla", IsNew = true, Phrase = "Drune"};

        public PhraseItem Phrase
        {
            get => _phrase;
            set => this.RaiseAndSetIfChanged(ref _phrase, value);
        }

        public Pack SelectedPack { get; set; }

        private ReactiveCommand<object> SaveCommand { get; }
    }
}
