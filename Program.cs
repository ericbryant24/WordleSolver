// See https://aka.ms/new-console-template for more information
using WordleGame;

var engineTester = new EngineTester();
var hoEngine = new HighestOccuranceEngine();
var hiEngine = new HalfItEngine();
var fotaEngine = new HalfItEngine();

engineTester.TestEngine(hoEngine, 100);
engineTester.TestEngine(hiEngine, 100);
engineTester.TestEngine(fotaEngine, 100);

Console.WriteLine();
Console.WriteLine("Press Enter To Continue");
Console.ReadLine();
