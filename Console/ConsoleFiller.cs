using Model;
using System;
using System.Linq;

namespace Filler_Mac
{
    public class ConsoleFiller
    {
        Pack _selectedPack = null;
        PackService _service = new PackService();
        string _selectedAuthor = "";

        public void Run()
        {
            Console.WriteLine("This is dotnet filler for mac.");
            var command = "";

            Console.WriteLine("Select a pack to work");
            HandleAuthorCommand();

            Console.WriteLine("Select a pack to work");
            HandlePackCommand();

            while (command != "exit" || command != "e")
            {
                Console.WriteLine("Enter command");
                command = Console.ReadLine();
                HandleCommand(command.ToLowerInvariant());
            }
        }

        private void HandleAuthorCommand()
        {
            Console.WriteLine("Possible Authors");
            for(int i = 1; i <= Reviewer.DefaultReviewers.Length; i++)
            {
                Console.WriteLine($"{i} {Reviewer.DefaultReviewers[i-1]}");
            }

            var index = ReadIntFromConsole(1, Reviewer.DefaultReviewers.Length, "Enter Author index");
            _selectedAuthor = Reviewer.DefaultReviewers[index - 1];
        }

        private void HandlePackCommand()
        {
            Console.WriteLine("Packs loading...");
            var packs = _service.GetAllPacksInfoAsync().Result; packs.Max(p => p.Id);
            foreach (var pack in packs)
            {
                Console.WriteLine($"{pack.Id} {pack.Name}");
            }

            var id = ReadIntFromConsole(1, packs.Max(p => p.Id), "Enter Id of the pack");

            Console.WriteLine($"Loading pack {packs.First(p => p.Id == id).Name}...");
            _selectedPack = _service.GetPackByIdAsync(id).Result;
            Console.WriteLine($"Pack was loaded. Use 'add' command to add a new word");
        }

        private int ReadIntFromConsole(int min, int max, string message)
        {
            int result = 0;
            Console.WriteLine(message);
            Console.WriteLine($"Value should be in range [{min} ... {max}]");
            while (true)
            {
                var line = Console.ReadLine().Trim();
                if (!Int32.TryParse(line, out result))
                {
                    Console.WriteLine("Please write integer");
                }
                else if(result > max || result < min)
                {
                    Console.WriteLine($"Value should be in range [{min} ... {max}]");
                }
                else
                {
                    break;
                }
            }

            return result;
        }

        private void HandleCommand(string command)
        {
            switch (command)
            {
                case "add":
                    HandleAddCommand();
                    break;
                case "pack":
                    HandlePackCommand();
                    break;
                case "delete":
                    HadleDeleteCommand();
                    break;
            }
        }

        private void HadleDeleteCommand()
        {
            Console.WriteLine("Enter word to delete");
            var word = Console.ReadLine();
            var responce = _service.DeletePhraseAsync(_selectedPack.Id, word, _selectedAuthor).Result;
            Console.WriteLine(responce);
        }

        private void HandleAddCommand()
        {
            Console.WriteLine("Enter the word");
            var word = Console.ReadLine();

            var complexity = ReadIntFromConsole(1, 5, "Enter the complexity");

            Console.WriteLine("Enter the description");
            var description = Console.ReadLine();

            var phrase = new PhraseItem
            {
                Phrase = word,
                Complexity = complexity,
                Description = description,
            };

            phrase.FormatPhrase();
            phrase.UpdateAuthor(_selectedAuthor);

            var response = _service.AddPhraseAsync(_selectedPack.Id, phrase, _selectedAuthor).Result;

            //Исправить эту дичь
            if (response.Trim() == "{\"result\": true}")
            {
                Console.WriteLine($"Word {phrase.Phrase} was saved");
            }
            else
            {
                Console.WriteLine($"Error on the word saving: {response}");
            }
        }
    }
}
