/*
 Enhancements Beyond Core Requirements:
 - Library of Scriptures: Instead of using a single scripture, the program loads multiple scriptures from a file (`scriptures.txt`).
 - Random Scripture Selection: Upon launch, the program picks a scripture randomly from the loaded library.
 - Improved Word Hiding Strategy: Hides 20% of visible words in each round, making memorization progressively more challenging.
 - File-Based Scripture Loading: Allows users to add more scriptures to `scriptures.txt` without modifying the code.
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        ScriptureLibrary library = new ScriptureLibrary("scriptures.txt");
        if (!library.HasScriptures())
        {
            Console.WriteLine("No scriptures found. Please ensure 'scriptures.txt' exists and has content in the format:");
            Console.WriteLine("Reference|Scripture text");
            Console.WriteLine("Example: John 3:16|For God so loved the world...");
            return;
        }

        Scripture scripture = library.GetRandomScripture();

        while (!scripture.IsCompletelyHidden())
        {
            Console.Clear();
            Console.WriteLine(scripture);
            Console.WriteLine("\nPress Enter to hide words or type 'quit' to exit.");

            string input = Console.ReadLine();
            if (input?.ToLower() == "quit") break;

            scripture.HideRandomWords();
        }

        Console.Clear();
        Console.WriteLine(scripture);
        Console.WriteLine("\nAll words hidden. Well done!");
    }
}

class ScriptureLibrary
{
    private List<Scripture> _scriptures = new();

    public ScriptureLibrary(string filePath)
    {
        LoadScriptures(filePath);
    }

    private void LoadScriptures(string filePath)
    {
        if (!File.Exists(filePath)) return;

        var lines = File.ReadAllLines(filePath);
        foreach (var line in lines)
        {
            var parts = line.Split('|');
            if (parts.Length == 2)
            {
                _scriptures.Add(new Scripture(new Reference(parts[0]), parts[1]));
            }
        }
    }

    public bool HasScriptures() => _scriptures.Count > 0;

    public Scripture GetRandomScripture()
    {
        Random rnd = new();
        return _scriptures[rnd.Next(_scriptures.Count)];
    }
}

class Scripture
{
    private Reference _reference;
    private List<Word> _words;

    public Scripture(Reference reference, string text)
    {
        _reference = reference;
        _words = text.Split(' ').Select(word => new Word(word)).ToList();
    }

    public void HideRandomWords()
    {
        Random rnd = new();
        int wordsToHide = Math.Max(1, _words.Count / 5); // Hide 20% of words each round
        var visibleWords = _words.Where(w => !w.IsHidden).ToList();

        for (int i = 0; i < wordsToHide && visibleWords.Count > 0; i++)
        {
            var wordToHide = visibleWords[rnd.Next(visibleWords.Count)];
            wordToHide.Hide();
            visibleWords.Remove(wordToHide);
        }
    }

    public bool IsCompletelyHidden() => _words.All(w => w.IsHidden);

    public override string ToString() => $"{_reference}\n{string.Join(" ", _words)}";
}

class Reference
{
    private string _text;

    public Reference(string text) => _text = text;

    public override string ToString() => _text;
}

class Word
{
    private string _text;
    public bool IsHidden { get; private set; }

    public Word(string text) => _text = text;

    public void Hide() => IsHidden = true;

    public override string ToString() => IsHidden ? new string('_', _text.Length) : _text;
}