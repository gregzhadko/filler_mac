using System;
using Model;
using ReactiveUI;
using System.Linq;

namespace UIApp
{
    public class EditingViewModel : ReactiveObject
    {
        private readonly Pack _selectedPack;
        private readonly string _author;
        private readonly ObservableAsPropertyHelper<ReactiveList<PhraseItem>> _phrases; //TODO: bullshit, I need to remove it
        private IPackService _service = new PackService();
        private string _phrase;
        private int _complexity;
        private string _description;
        private PhraseItem _oldPhrase;
        private PhraseItem _phraseItem;
        private string _errorMessage;

        public event Action Close;

        public EditingViewModel(Pack selectedPack, PhraseItem phraseItem, string author, ObservableAsPropertyHelper<ReactiveList<PhraseItem>> phrases)
        {
            _selectedPack = selectedPack;
            _author = author;
            _phrases = phrases;
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
                    ShowError(message);
                }
                else
                {
                    Close?.Invoke();
                }
            });

            CloseCommand = ReactiveCommand.Create();
            CloseCommand.Subscribe(_ =>
            {
                Close?.Invoke();
            });

        }

            

        private ReactiveCommand<object> SaveCommand { get; }
        public ReactiveCommand<object> CloseCommand { get; }


        private void ShowError(string message)
        {
            ErrorMessage = message;
        }

        private bool Save(out string message)
        {
            var phraseItem = new PhraseItem { Phrase = Phrase, Complexity = Complexity, Description = Description };

            StringUtils.FormatPhrase(phraseItem);

            if (String.IsNullOrWhiteSpace(phraseItem.Phrase)) //TOFO: fix it, textblock should be highlighted.
            {
                message = "Phrase can't be empty!";
                return false;
            }

            if(_selectedPack.Phrases.Select(p => p.Phrase).Contains(phraseItem.Phrase))
            {
                message = "Phrase is already in the pack";
                return false;
            }

            message = _oldPhrase == null
                ? _service.AddPhraseAsync(_selectedPack.Id, phraseItem, _author).Result
                : _service.EditPhraseAsync(_selectedPack.Id, _oldPhrase, phraseItem, _author).Result;
            if (message.Trim() == "{\"result\": true}")
            {
                _phrases.Value.Add(phraseItem);
                return true;
            }

            return false;
        }



        public string Phrase
        {
            get => _phrase;
            set
            {
                this.RaiseAndSetIfChanged(ref _phrase, value);
            }
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

        public string ErrorMessage {
            get => _errorMessage;
            set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
        }
    }
}
