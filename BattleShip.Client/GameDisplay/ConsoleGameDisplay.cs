using System;
using System.Collections.Generic;
using BattleShip.Projections;

namespace BattleShip.Client.Validators 
{
    public class ConsoleGameDisplay : IGameDisplay
    {
        public void DisplayBoard(IReadOnlyDictionary<(int x, int y), bool> shotsTaken, int boardSize)
        {   
            int defaultSpc = 2;
            int extraSpaceAfterCoordinate = 2;
            int maxDigitCount = boardSize.ToString().Length; //max digit you can print on left
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            
            //Only header column first
            //Print empty space to align with the first column of board
            Console.Write(new string(' ', (maxDigitCount + extraSpaceAfterCoordinate)));
            for (int i = 0; i < boardSize; i++)
            {
                Console.Write($"{i.ToLetterPart()}{new string(' ', defaultSpc+1)}");
                if(i == (boardSize-1)) //crlf after printing letter coordinates
                {
                    Console.Write(Environment.NewLine + Environment.NewLine);
                }
            }

            //Rest of the board
            for (int i = 0; i < boardSize; i++)
            {
                var spaceToPrint = maxDigitCount - (i+1).ToString().Length;
                Console.Write($"{i+1}{new string(' ', (spaceToPrint + extraSpaceAfterCoordinate))}");
                
                for (int j = 0; j < boardSize; j++)
                {
                    if(shotsTaken.ContainsKey((j,i))) 
                    {
                        if(shotsTaken[(j,i)]) 
                            Console.Write($"✅{new string(' ', defaultSpc)}");
                        else 
                            Console.Write($"❌{new string(' ', defaultSpc)}");
                    }
                    else 
                    { 
                        Console.Write($"🌊{new string(' ', defaultSpc)}");
                    }                
                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }

            Console.ResetColor();
        }

        public void DisplayBoardCreated(int dimension)
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine($"{dimension}x{dimension} Battle Ships Board Created!");
            Console.ResetColor();
        }

        public void DisplayHit(string shipName)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"------------------------------------");
            Console.WriteLine($"  🚀 Target Hit 💥 ➡️ 🛳 {shipName}");
            Console.WriteLine($"------------------------------------");
            Console.ResetColor();
        }

        public void DisplayMissed()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"------------------------------------");
            Console.WriteLine($"           🚀 Miss ❌");
            Console.WriteLine($"------------------------------------");
            Console.ResetColor();
        }

        public void DisplayOperationFailure(string reason)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"❌ -> {reason}");
            Console.ResetColor();
        }

        public void DisplayShipAdded(string shipName, int size, string hint = null)
        {
            hint = Program.WithHints ? $" hint: {hint}{Environment.NewLine}" : Environment.NewLine;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"🛳  {shipName} (size {size}) added!");
            Console.ForegroundColor  = ConsoleColor.DarkGray;
            Console.Write(hint);
            Console.ResetColor();
        }

        public void DisplayShipSunk(string shipName)
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine($"------------------------------------");
            Console.WriteLine($"💀💀💀 Ship Sunk! RIP ✝ {shipName}!!");
            Console.WriteLine($"------------------------------------");
            Console.ResetColor();
        }

        public void DisplayWon(string playerName = null)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"------------------------------------");
            Console.WriteLine($"🎉🎉🎉🎉🎉 Game Over! Player Won 👏👏👏👏👏");
            Console.WriteLine($"------------------------------------");
            Console.ResetColor();
            Program.PlayerWon = true;
            Environment.Exit(-1);
        }
    }

}