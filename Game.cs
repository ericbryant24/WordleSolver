// See https://aka.ms/new-console-template for more information
namespace WordleGame
{
  public class Game
  {
    public int MaxTurns => 6;

    public Game(string goalWord)
    {
      GameKey = Guid.NewGuid();
      OnTurn = 1;
      GoalWord = goalWord;
    }

    public Guid GameKey { get; private set; }
    public string GoalWord { get; private set; }
    public int OnTurn { get; private set; }
    public bool IsOver { get; private set; }
    public bool DidWin { get; private set; }

    public GuessResponse Guess(string guess)
    {
      var result = guess.Select((letter, idx) => 
        GoalWord[idx] == letter ? 2
        : GoalWord.Contains(letter) ? 1
        : 0
      );

      DidWin = guess == GoalWord;

      if(OnTurn >= MaxTurns || DidWin)
      {
        IsOver = true;
      } else
      {
        OnTurn++;
      }


      return new GuessResponse
      {
        Result = result.ToArray(),
        OnTurn = OnTurn,
        DidWin = DidWin,
        IsOver = IsOver
      };
    }
  }

  public class GuessResponse
  {
    public int[] Result { get; set; } = Array.Empty<int>(); 
    public bool IsOver { get; set; }
    public bool DidWin { get; set; }
    public int OnTurn { get; set; }
  }
}

