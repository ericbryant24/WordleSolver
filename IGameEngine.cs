// See https://aka.ms/new-console-template for more information

namespace WordleGame
{
  public interface IGameEngine
  {
    string Name { get; }

    void Reset();
    void TakeTurn(Func<string, GuessResponse> guessFunc);
  }
}