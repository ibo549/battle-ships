using System;
using BattleShip.Domain;

namespace BattleShip.Client.Extensions 
{
    public static class CoordinateExtensions
    {
        public static BoardPoint ToBoardPoint(this string letterCoordinate)
        {
            if(string.IsNullOrWhiteSpace(letterCoordinate) && letterCoordinate.Length < 2)
                throw new ArgumentException($"{letterCoordinate} length must be at least 2");
            
            var parsed = letterCoordinate.ToUpperParsedLetterCoordinates();
            
            int X = parsed.letterPart.ToIndexInAlphabet() - 1;
            int Y = Int32.Parse(parsed.numberPart) - 1;

            return new BoardPoint(X, Y);
        }

        private static int ToIndexInAlphabet(this char ch)
        {
            var upper = char.ToUpper(ch);
            if (!upper.IsEnglishLetter())
            {
                throw new ArgumentOutOfRangeException("Only Standard Latin characters.");
            }

            return upper - 64;
        }

        public static (char letterPart, string numberPart) ToUpperParsedLetterCoordinates(this string letterCoordinates)
        {
            //Could use string builder
            return (char.ToUpper(letterCoordinates[0]), letterCoordinates.Substring(1));
        }

        public static bool IsEnglishLetter(this char c)
        {
             return (c>='A' && c<='Z') || (c>='a' && c<='z');
        }
    }
}