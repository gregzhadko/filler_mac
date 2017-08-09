using System;
using System.Reactive.Linq;
using Model;
using ReactiveUI;

namespace UIApp
{
    public class EditingViewModel : ReactiveObject
    {
        private readonly Pack _selectedPack;
        private readonly string _author;
        private IPackService _service = new PackService();
        private string _phrase;
        private int _complexity;
        private string _description;
        private PhraseItem _oldPhrase;
        private PhraseItem _phraseItem;

        public event Action Close;

        public EditingViewModel(Pack selectedPack, PhraseItem phraseItem, string author)
        {
            _selectedPack = selectedPack;
            _author = author;
            _oldPhrase = phraseItem?.Clone();
            if (phraseItem != null)
            {
                Phrase = phraseItem.Phrase;
                Complexity = (int)phraseItem.Complexity;
                Description = phraseItem.Description;
            }
            SaveCommand = ReactiveCommand.Create();
            SaveCommand.Subscribe(_ =>
            {
                if (!Save(out string message))
                {
                    ShowDialog(message);
                }
                else
                {
                    Close?.Invoke();
                }
            });
        }

        private void ShowDialog(string message)
        {
            
        }

        private bool Save(out string message)
        {
            var phraseItem = new PhraseItem {Phrase = Phrase, Complexity = Complexity, Description = Description};
            message = _oldPhrase == null
                ? _service.AddPhraseAsync(_selectedPack.Id, phraseItem, _author).Result
                : _service.EditPhraseAsync(_selectedPack.Id, _oldPhrase, phraseItem, _author).Result;
            if (message.Trim() == "{\"result\": true}")
            {
                return true;
            }
            return false;
        }



        public string Phrase
        {
            get => _phrase;
            set => this.RaiseAndSetIfChanged(ref _phrase, value);
        }
        public int Complexity
        {
            get => _complexity;
            set => this.RaiseAndSetIfChanged(ref _complexity, value);
        }
        public string Description
        {
            get => _description;
            set => this.RaiseAndSetIfChanged(ref _description, value);
        }

        public Pack SelectedPack { get; set; }

        private ReactiveCommand<object> SaveCommand { get; }
    }
}
