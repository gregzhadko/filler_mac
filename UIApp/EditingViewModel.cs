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
        private ObservableAsPropertyHelper<string> _phrase;
        private ObservableAsPropertyHelper<int> _complexity;
        private ObservableAsPropertyHelper<string> _description;

        public EditingViewModel(Pack selectedPack, PhraseItem phraseItem, string author)
        {
            _selectedPack = selectedPack;
            _author = author;
            
            //TODO: Do we really need so complex initialization?
            _phrase = this.WhenAnyValue(vm => vm.PhraseItem).Select(p => p.Phrase).ToProperty(this, vm => vm.Phrase);
            _complexity = this.WhenAnyValue(vm => vm.PhraseItem).Select(p => (int)p.Complexity).ToProperty(this, vm => vm.Complexity);
            _description = this.WhenAnyValue(vm => vm.PhraseItem).Select(p => p.Description).ToProperty(this, vm => vm.Description);

            PhraseItem = phraseItem;
            SaveCommand = ReactiveCommand.Create();
            SaveCommand.Subscribe(_ =>
            {
                if (Save())
                {
                    Close();
                }
            });
        }

        private bool Save()
        {
            return true;
        }

        private void Close()
        {
            //TODO: implement
        }

        public string Phrase => _phrase.Value;
        public int Complexity => _complexity.Value;
        public string Description => _description.Value;

        public PhraseItem PhraseItem { get; set; }

        public Pack SelectedPack { get; set; }

        private ReactiveCommand<object> SaveCommand { get; }
    }
}
