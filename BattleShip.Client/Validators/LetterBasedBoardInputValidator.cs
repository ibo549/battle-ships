using System;
using BattleShip.Client.Extensions;
using BattleShip.Projections;

namespace BattleShip.Client.Validators 
{ 
    public class LetterBasedBoardInputValidator : IGameInputValidator
    {
        public LetterBasedBoardInputValidator(){}

        public bool IsBoardSizeValid(int boardSize)
        {
            int maxLettersInEnglishAlphabet = 26;

            if(boardSize > maxLettersInEnglishAlphabet || boardSize <= 0)
                return false;

            return true;
        }

        public bool IsCoordinateValid(string letterCoordinate)
        {
            if(string.IsNullOrWhiteSpace(letterCoordinate))
                return false;

            if(letterCoordinate.Length < 2)
                return false;
                       
            var parsed = letterCoordinate.ToUpperParsedLetterCoordinates();

            if (!parsed.letterPart.IsEnglishLetter())
                return false;

            if(Int32.TryParse(parsed.numberPart, out int number))
                return true;

            return false;
        }

        
    }

}