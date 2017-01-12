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
        var player1 = new Queue();
        var player2 = new Queue();       
        
        var value = Console.ReadLine(); 
        Console.Error.WriteLine(value);
        var n = int.Parse(value); // the number of cards for player 1
        for (var i = 0; i < n; i++)
        {
            value = Console.ReadLine(); 
            Console.Error.WriteLine(value);
            player1.Enqueue(new Card(value));            
        }

        value = Console.ReadLine(); 
        Console.Error.WriteLine(value);
        var m = int.Parse(value); // the number of cards for player 2
        for (var i = 0; i < m; i++)
        {
            value = Console.ReadLine(); 
            Console.Error.WriteLine(value);
            player2.Enqueue(new Card(value));
        }

        var cards1 = new List<Card>();
        var cards2 = new List<Card>();
        var numberOfSet = 0;
        
        while (player1.Count > 0 || player2.Count > 0)
        {
            var canContinue = true;
            Card card1;
            Card card2;

            var win1 = GetCard(cards1, player1, out card1);
            var win2 = GetCard(cards2, player2, out card2);

            if (!win1 || !win2)
            {
                Console.WriteLine("{0} {1}", win1 ? "1" : "2", numberOfSet);
                break;
            }
            
             if (card1.priority < card2.priority)
            {
                PullCard(cards1, cards2, player1);
                numberOfSet++;
            }
            else if (card1.priority > card2.priority)
            {
                PullCard(cards1, cards2, player2);
                numberOfSet++;
            }
            else if (card1.priority == card2.priority)
            {
                canContinue = GetCard(cards1, player1, out card1);
                canContinue &= GetCard(cards1, player1, out card1);
                canContinue &= GetCard(cards1, player1, out card1);

                canContinue &= GetCard(cards2, player2, out card2);
                canContinue &= GetCard(cards2, player2, out card2);
                canContinue &= GetCard(cards2, player2, out card2);

                if (!canContinue)
                {
                    Console.WriteLine("PAT");
                    break;
                }
            }
        }
    }

    private static void PullCard(List<Card> cards1, List<Card> cards2, Queue player)
    {
        foreach (var card in cards1)
        {
            player.Enqueue(card);
        }
        foreach (var card in cards2)
        {
            player.Enqueue(card);
        }

        cards1.Clear();
        cards2.Clear();
    }

    private static bool GetCard(List<Card> cards, Queue player, out Card card)
    {
        if (player.Count <= 0)
        {
            card = null;
            return false;
        }

        card = (Card)player.Dequeue();
        cards.Add(card);
        return true;
    }
}

public class Card
{

    public Card(string cardp1)
    {
        this.Value = cardp1.Substring(0, cardp1.Length-1);
        this.Color = cardp1.Substring(cardp1.Length - 1, 1);

        switch (this.Value)
        {
            case "2":
                this.priority = 13;
                break;
            case "3":
                this.priority = 12;
                break;
            case "4":
                this.priority = 11;
                break;
            case "5":
                this.priority = 10;
                break;
            case "6":
                this.priority = 9;
                break;
            case "7":
                this.priority = 8;
                break;
            case "8":
                this.priority = 7;
                break;
            case "9":
                this.priority = 6;
                break;
            case "10":
                this.priority = 5;
                break;
            case "J":
                this.priority = 4;
                break;
            case "Q":
                this.priority = 3;
                break;
            case "K":
                this.priority = 2;
                break;
            case "A":
                this.priority = 1;
                break;
        }
    }

    public string Color { get; set;}

    public string Value { get; set; }

    public int priority { get; set; }
}
