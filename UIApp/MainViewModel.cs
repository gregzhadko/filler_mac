using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using Model;
using ReactiveUI;

namespace UIApp
{
    public class MainViewModel : ReactiveObject
    {
        private readonly int _defaultPackId = 20;
        private string _selectedAuthor = "zhadko";
        private readonly ObservableAsPropertyHelper<ReactiveList<PhraseItem>> _phrases;
        private readonly IPackService _service = new PackService();

        private ObservableCollection<Pack> _packs;

        private Pack _selectedPack;

        public MainViewModel()
        {
            Init();

            _phrases = this.WhenAnyValue(vm => vm.SelectedPack)
                .Select(p =>
                {
                    var phrases = _service.GetPackByIdAsync(p.Id).Result.Phrases;
                    return new ReactiveList<PhraseItem>(phrases);
                })
                .ToProperty(this, vm => vm.Phrases);

            NewPhraseCommand = ReactiveCommand.Create();
            NewPhraseCommand.Subscribe(_ =>
            {
                EditPhrase(null);
            });
        }


        private void EditPhrase(PhraseItem phraseItem)
        {
            //TODO: Rewrite with DI
            var editViewModel = new EditingViewModel(_selectedPack, phraseItem, _selectedAuthor);
            var editView = new EditingView(editViewModel);
            editView.ShowDialog();
        }

        public ReactiveList<PhraseItem> Phrases => _phrases.Value;

        public Pack SelectedPack
        {
            get => _selectedPack;
            set => this.RaiseAndSetIfChanged(ref _selectedPack, value);
        }

        public ObservableCollection<Pack> Packs
        {
            get => _packs;
            set => this.RaiseAndSetIfChanged(ref _packs, value);
        }

        public ReactiveCommand<object> NewPhraseCommand { get; }

        private void Init()
        {
            var packsTask = _service.GetAllPacksInfoAsync();
            Packs = new ObservableCollection<Pack>(packsTask.Result.OrderBy(p => p.Id));

            var packTask = _service.GetPackByIdAsync(_defaultPackId);

            var selectedPack = Packs.First(p => p.Id == packTask.Result.Id);
            selectedPack.Phrases = packTask.Result.Phrases;
            selectedPack.Description = packTask.Result.Description;
            SelectedPack = selectedPack;
        }
    }
}