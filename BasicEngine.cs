// See https://aka.ms/new-console-template for more information
namespace WordleGame
{
  public abstract class BasicEngine
  {
    protected List<string> _possibleAnswers;
    protected Dictionary<char, int> _scoredLetters = new();
    protected Dictionary<string, int> _scoredAnswers = new();
    protected Dictionary<string, int> _scoredGuesses = new();
    protected List<char> _foundLetters = new();
    protected List<char> _foundExclusions = new();
    protected Dictionary<int, char> _foundPositions = new();

    public BasicEngine()
    {
      Reset();
    }

    public void TakeTurn(Func<string, GuessResponse> guessFunc)
    {
      var guess = ChooseGuess();
      var response = guessFunc(guess);
      var result = response.Result;
      ProcessResult(guess, result);
    }

    public void Reset()
    {
      _possibleAnswers = Words.All.ToList();
      _scoredAnswers.Clear();
      _scoredAnswers.Clear();
      _scoredGuesses.Clear();
      _foundLetters.Clear();
      _foundExclusions.Clear();
      _foundPositions.Clear();
      ScoreWords();
    }

    protected virtual string ChooseGuess()
    {
      if(_possibleAnswers.Count == 1)
      {
        return _possibleAnswers[0];
      }

      return _scoredGuesses.OrderByDescending(a => a.Value).First().Key;
    }

    private void ProcessResult(string guess, int[] result)
    {
      for (var i = 0; i < guess.Length; i++)
      {
        var resultCode = Convert.ToInt32(result[i]);
        var letter = guess[i];

        if (resultCode == 2)
        {
          if (!_foundPositions.ContainsKey(i))
          {
            _foundPositions.Add(i, letter);
          }
        }

        if (resultCode > 0)
        {
          if (!_foundLetters.Contains(letter))
          {
            _foundLetters.Add(letter);
          }
        }
        else
        {
          if (!_foundExclusions.Contains(letter))
          {
            _foundExclusions.Add(letter);
          }
        }
      }

      RemoveImpossibleAnswers();
      ScoreWords();
    }

    private void RemoveImpossibleAnswers()
    {
      _possibleAnswers = _possibleAnswers.Where(a => WordIsStillPossible(a)).ToList();
    }

    private bool WordIsStillPossible(string word)
    {
      return _foundLetters.All(l => word.Contains(l))
        && _foundPositions.All(l => word[l.Key] == l.Value)
        && !_foundExclusions.Any(l => word.Contains(l));
    }

    protected virtual void ScoreWords()
    {
      _scoredLetters = ScoreLetters();
      _scoredAnswers = _possibleAnswers.ToDictionary(word => word, ScoreWord);
      _scoredGuesses = Words.All.ToDictionary(word => word, ScoreWord);
    }

    protected abstract Dictionary<char, int> ScoreLetters();

    private int ScoreWord(string word)
    {
      return word.Distinct().Aggregate(0, (score, letter) => 
        _scoredLetters.ContainsKey(letter) ? _scoredLetters[letter] + score : score);
    }
  }

  public class HighestOccuranceEngine : BasicEngine, IGameEngine
  {
    public string Name => "Highest Occurance Engine";

    protected override Dictionary<char, int> ScoreLetters()
    {
      return _possibleAnswers
        .Select(a => a.Distinct())
        .SelectMany(a => a)
        .GroupBy(a => a)
        .ToDictionary(a => a.Key, a => a.Count());
    }
  }

  public class HalfItEngine : BasicEngine, IGameEngine
  {
    public string Name => "Half It Engine";

    protected override Dictionary<char, int> ScoreLetters()
    {
      var letters = _possibleAnswers
        .Select(a => a.Distinct())
        .SelectMany(a => a)
        .Distinct();

      var half = _possibleAnswers.Count() / 2;
      return letters.ToDictionary(l => l, l =>
      {
        var count = _possibleAnswers.Count(a => a.Contains(l));
        return half = Math.Abs(half - count);
      });
    }
  }

  public class FocusOnTheAnswersEngine : HighestOccuranceEngine, IGameEngine
  {
    public new string Name => "Focus On The Answers Engine";

    protected override string ChooseGuess()
    {
      return _scoredAnswers.OrderByDescending(a => a.Value).First().Key;
    }
  }
}

