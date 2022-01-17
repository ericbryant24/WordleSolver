// See https://aka.ms/new-console-template for more information
namespace WordleGame
{
  public class ManagedGame
  {
    private bool _onlyOutputLosses;
    private bool _hideOutput;
    private string? _goalWord;
    private readonly IGameEngine _engine;
    private string[] _excludedWords = Array.Empty<string>();

    public string PlayerName { get; private set; }
    public Game? Game { get; private set; }

    public ManagedGame(IGameEngine engine)
    {
      _engine = engine;
      PlayerName = engine.Name;
    }

    public ManagedGame OnlyOutputLosses()
    {
      _onlyOutputLosses = true;
      return this;
    }

    public ManagedGame HideOutput()
    {
      _hideOutput = true;
      return this;
    }

    public ManagedGame UseGoalWord(string goalWord)
    {
      _goalWord = goalWord;
      return this;
    }

    public ManagedGame ExcludeWords(IEnumerable<string> words)
    {
      _excludedWords = words.ToArray();
      return this;
    }

    public Game Play()
    {
      var output = new List<string> { $"{_engine.Name} is playing Wordle" };

      var words = Words.AllAnswers.Where(w => !_excludedWords.Contains(w));
      var index = new Random().Next(words.Count());
      var goalWord = _goalWord ?? words.ElementAt(index);

      Game = new Game(goalWord);

      while (!Game.IsOver)
      {
        _engine.TakeTurn(guessWord => {
          var turn = Game.OnTurn;
          var response = Game.Guess(guessWord);
          output.Add($"{turn}: {guessWord}");
          output.Add($"{turn}: {string.Join("", response.Result)}");

          return response;
        });
      }

      var wonLost = Game.DidWin ? "won" : "lost";

      output.Add($"{_engine.Name} has {wonLost}");
      output.Add($"The word was \"{Game.GoalWord}\"");

      if(!_hideOutput && (!_onlyOutputLosses || !Game.DidWin))
      {
        output.ForEach(Console.WriteLine);
      }

      return Game;
    }
  }

  public class EngineTester
  {
    public void TestEngine(IGameEngine engine, int numberOfGames)
    {
      var games = new List<Game>();
      for(var i = 0; i < numberOfGames; i++)
      {
        games.Add(new ManagedGame(engine)
          .ExcludeWords(games.Select(g => g.GoalWord))
          .OnlyOutputLosses()
          .Play()
        );
      }

      var wins = games.Count(g => g.DidWin);
      var losses = games.Count(g => !g.DidWin);
      var avgTurns = games.Average(g => g.OnTurn);

      Console.WriteLine($"{engine.Name} has played {games.Count()} games");
      Console.WriteLine($"Record: {wins} - {losses}");
      Console.WriteLine($"Avg # of Turns: {avgTurns}");
    }
  }
}

