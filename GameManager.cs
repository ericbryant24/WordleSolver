// See https://aka.ms/new-console-template for more information
namespace WordleGame
{
  public class GameManager
  {
    private readonly List<Game> _games = new();

    public Guid PlayGame(IGameEngine engine, bool output = true)
    {
      if(output)
      {
        Console.WriteLine($"{engine.Name} is playing Wordle");
      }

      var wordsAlreadyPlayed = _games.Where(g => g.PlayerName == engine.Name).Select(g => g.GoalWord);
      var game = new Game(engine.Name, wordsAlreadyPlayed);
      _games.Add(game);

      while (!game.IsOver)
      {
        engine.TakeTurn(game.MaxTurns - game.OnTurn, guessWord => {
          var turn = game.OnTurn;
          var response = game.Guess(guessWord);
          if (output)
          {
            Console.WriteLine($"{turn}: {guessWord}");
            Console.WriteLine($"{turn}: {string.Join("", response.Result)}");
          }
          return response;
        });
      }

      engine.Reset();

      var wonLost = game.DidWin ? "won" : "lost";

      if (output)
      {
        Console.WriteLine($"{engine.Name} has {wonLost}");
        Console.WriteLine($"The word was \"{game.GoalWord}\"");
      }

      return game.GameKey;
    }

    public Game[] GetGamesByPlayer(string playerName)
    {
      return _games.Where(g => g.PlayerName == playerName).ToArray();
    }
  }

  public class EngineTester
  {
    public void TestEngine(IGameEngine engine, int numberOfGames)
    {
      var manager = new GameManager();
      for(var i = 0; i < numberOfGames; i++)
      {
        manager.PlayGame(engine);
      }

      var games = manager.GetGamesByPlayer(engine.Name);
      var wins = games.Count(g => g.DidWin);
      var losses = games.Count(g => !g.DidWin);
      var avgTurns = games.Average(g => g.OnTurn);

      Console.WriteLine($"{engine.Name} has played {games.Length} games");
      Console.WriteLine($"Record: {wins} - {losses}");
      Console.WriteLine($"Avg # of Turns: {avgTurns}");
    }
  }
}

