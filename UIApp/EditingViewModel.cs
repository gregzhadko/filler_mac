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
            PhraseItem = phraseItem ?? new PhraseItem();
            SaveCommand = ReactiveCommand.Create();
            SaveCommand.Subscribe(_ =>
            {
                if (Save())
                {
                    Close?.Invoke();
                }
            });
        }

        private bool Save()
        {
            var result = _oldPhrase == null
                ? _service.AddPhraseAsync(_selectedPack.Id, PhraseItem, _author).Result
                : _service.EditPhraseAsync(_selectedPack.Id, _oldPhrase, PhraseItem, _author).Result;
            return true;
        }



        public string Phrase
        {
            get { return _phrase; }
            set { this.RaiseAndSetIfChanged(ref _phrase, value); }
        }
        public int Complexity
        {
            get { return _complexity; }
            set { this.RaiseAndSetIfChanged(ref _complexity, value); }
        }
        public string Description
        {
            get { return _description; }
            set { this.RaiseAndSetIfChanged(ref _description, value); }
        }

        public PhraseItem PhraseItem
        {
            get
            {
                return _phraseItem;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _phraseItem, value);
                Phrase = _phraseItem.Phrase;
                Complexity = (int)_phraseItem.Complexity;
                Description = _phraseItem.Description;
            }
        }

        public Pack SelectedPack { get; set; }

        private ReactiveCommand<object> SaveCommand { get; }
    }
}
