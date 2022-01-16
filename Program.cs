// See https://aka.ms/new-console-template for more information
using WordleGame;
Console.WriteLine($"{Words.AllAnswers.Count()} possible answers");
Console.WriteLine($"{Words.All.Count()} possible guess words");

var engineTester = new EngineTester();
var hoEngine = new HighestOccuranceEngine();
var hiEngine = new HalfItEngine();
var fotaEngine = new FocusOnTheAnswersEngine();
var randomEngine = new RandomEngine();

engineTester.TestEngine(hoEngine, 100);
engineTester.TestEngine(hiEngine, 100);
engineTester.TestEngine(fotaEngine, 100);
engineTester.TestEngine(randomEngine, 100);

Console.WriteLine();
Console.WriteLine("Press Enter To Continue");
Console.ReadLine();
