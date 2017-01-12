using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Solution
{
static void Main(string[] args)
    {
        var letters = new List<LetterScore>
                      {
                          new LetterScore('a', 1),
                          new LetterScore('b', 3),
                          new LetterScore('c', 3),
                          new LetterScore('d', 2),
                          new LetterScore('e', 1),
                          new LetterScore('f', 4),
                          new LetterScore('g', 2),
                          new LetterScore('h', 4),
                          new LetterScore('i', 1),
                          new LetterScore('j', 8),
                          new LetterScore('k', 5),
                          new LetterScore('l', 1),
                          new LetterScore('m', 3),
                          new LetterScore('n', 1),
                          new LetterScore('o', 1),
                          new LetterScore('p', 3),
                          new LetterScore('q', 10),
                          new LetterScore('r', 1),
                          new LetterScore('s', 1),
                          new LetterScore('t', 1),
                          new LetterScore('u', 1),
                          new LetterScore('v', 4),
                          new LetterScore('w', 4),
                          new LetterScore('x', 8),
                          new LetterScore('y', 4),
                          new LetterScore('z', 10)
                      };

        var words = new List<WordScore>();        
        int N = int.Parse(Console.ReadLine());
        Console.Error.WriteLine(N);

        for (int i = 0; i < N; i++)
        {
            var W = Console.ReadLine();
            Console.Error.WriteLine(W);
            words.Add(new WordScore(W, letters));
        }
        
        string LETTERS = Console.ReadLine();
        Console.Error.WriteLine(LETTERS);

        WordScore selectedWord = null;
        foreach (var word in words)
        {
            var res = word.CanDotheWord(LETTERS);
            if ((selectedWord == null && res > 0) || (selectedWord!= null && res > selectedWord.Score))
            {
                selectedWord = word;                
            }
        }
        
        Console.WriteLine(selectedWord == null ? "invalid word" : selectedWord.Word);
    }

    public class WordScore
    {
        public string Word { get; private set; }

        public int Score { get; private set; }
        
        public WordScore(string word, List<LetterScore> letters)
        {
            this.Word = word;
            foreach (var letter in this.Word)
            {
                var letterScore = letters.First(l => l.Letter == letter);
                this.Score += letterScore.Score;
            }
        }

        public int CanDotheWord(string letters)
        {
            var score = 0;
            var match = true;
            var currentword = this.Word.ToCharArray().ToList();
            var currentletters = letters.ToCharArray().ToList();
            foreach (var letter in currentword)
            {
                var matchchar = this.Matchchar(letter, currentletters);
                if (matchchar)
                {
                    currentletters.Remove(letter);
                }
                match &= matchchar;
            }

            if (match)
            {
                return this.Score;
            }

            return score;
        }

        private bool Matchchar(char letter, IEnumerable<char> word)
        {
            var matchchar = false;
            foreach (var letterWord in word)
            {
                if (letterWord == letter)
                {
                    matchchar = true;
                }
            }
            return matchchar;
        }
    }
        

    public class LetterScore
    {
        public char Letter { get; private set; }

        public int Score { get; private set; }

        public LetterScore(char letter, int score)
        {
            this.Letter = letter;
            this.Score = score;
        }
    }
}
