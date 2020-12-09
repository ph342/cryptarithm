using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphametiken
{
    class Calculator
    {
        //eingelesene Daten
        private Input input;
        //alle einzigartigen Charaktere
        private List<char> allChars;
        //alle richtigen Ergebnisse
        private List<int[]> correctResults;

        /*
         * berechnet alle Wertesets, für die die eingelesene Gleichung aufgeht
         * wenn onlyOneResult, dann soll nach einem gefundenen Ergebnis abgebrochen werden
         */
        public List<int[]> calcResults(Input input, bool onlyOneResult)
        {
            this.input = input;
            correctResults = new List<int[]>();

            allChars = getUniqueCharacters();
            /*
            * Kontrollstruktur:
            * valide Gleichung hat mind. 2 Buchstaben und 1 Operator
            */
            if (allChars.Count() <= 1) return correctResults;
            if (input.getOperators().Count() < 1) return correctResults;

            int[] valueSet = new int[allChars.Count()];
            //valueSet mit ..., 2, 1, 0 initialisieren
            for (int i = 0; i < valueSet.Count(); i++)
                valueSet[valueSet.Count() - 1 - i] = i;

            bool running = true;
            while (running)
            {
                //erste Stelle inkrementieren
                for (int number = valueSet[0]; number < 10; number++)
                {
                    valueSet[0] = number;
                    try
                    {
                        //Prüfung
                        permutationsOfArray(valueSet, valueSet.Count() - 1, onlyOneResult);
                    }
                    catch (OneResultOnlyException)
                    {
                        //für Aufgabe 2 wesentlich
                        running = false;
                        break; //endgültig
                    }
                }
                for (int arrayIdx = 1; arrayIdx < valueSet.Count(); arrayIdx++)
                {
                    /*  
                        * nach dem nächsten Wert suchen,  der nicht einen
                        * geringer ist als sein Vorgänger
                    */
                    if (valueSet[arrayIdx] == valueSet[arrayIdx - 1] - 1)
                    {
                        //wenn am Ende angekommen, abbrechen
                        if (arrayIdx == valueSet.Count() - 1)
                        {
                            running = false;
                            break; //endgültig
                        }
                        continue;
                    }
                    else
                    {
                        //Wert inkrementieren
                        valueSet[arrayIdx]++;
                        //alle Vorgänger entsprechend eins höher setzten als ihre Nachfolger
                        for (int goBackIdx = arrayIdx - 1; goBackIdx >= 0; goBackIdx--)
                        {
                            valueSet[goBackIdx] = valueSet[goBackIdx + 1] + 1;
                        }
                        break;
                    }
                }
            }
            return correctResults;
        }

        /* 
         * rekursive Methode, die beim übergebenen Array die einmaligen Permutationen 
         * errechnet und die Überprüfung ausführt
         * endIndex stellt die Rekursionstiefe dar
         */
        private void permutationsOfArray(int[] valueSet, int endIndex, bool onlyOneResult)
        {
            if (endIndex == 0)
            {
                try
                {
                    if (calculateCurrentSet(valueSet))
                    {
                        correctResults.Add((int[])valueSet.Clone());
                        if (onlyOneResult)
                            throw new OneResultOnlyException();
                    }
                }
                catch (LeadingZeroException)
                {
                    //führende 0 einer Kombination ignorieren
                }
            }
            else
            {
                permutationsOfArray(valueSet, endIndex - 1, onlyOneResult);
                for (int i = 0; i <= endIndex - 1; i++)
                {
                    exchange(valueSet, i, endIndex);
                    permutationsOfArray(valueSet, endIndex - 1, onlyOneResult);
                    exchange(valueSet, i, endIndex);
                }
            }
        }

        //tauscht 2 Werte eines Arrays
        public void exchange(int[] a, int x, int y)
        {
            int temp = a[x];
            a[x] = a[y];
            a[y] = temp;
        }

        //überprüft das aktuelle Werteset
        private bool calculateCurrentSet(int[] valueSet)
        {
            int expectedResult = convertWordToNumber(valueSet, input.getResult());
            List<Operator> operators = new List<Operator>(input.getOperators());
            List<int> convertedNumbers = new List<int>();

            for (int i = 0; i < input.getInputWords().Count(); i++)
                convertedNumbers.Add(convertWordToNumber(valueSet, input.getInputWords().ElementAt(i)));

            for (int i = 0; i < operators.Count(); i++)
            {
                Operator op = operators.ElementAt(i);
                if (op.Equals(Operator.DIVIDE) || op.Equals(Operator.TIMES))
                {
				    int newResult = OperatorMethods.calculateTwoValues(convertedNumbers.ElementAt(i), convertedNumbers.ElementAt(i+1), op);
				    convertedNumbers[i] = newResult;
                    convertedNumbers.RemoveAt(i + 1);
                    operators.RemoveAt(i-1);
			    }
            }

            int calculatedResult = convertedNumbers.ElementAt(0);

            for (int i = 1; i < convertedNumbers.Count(); i++)
                calculatedResult = OperatorMethods.calculateTwoValues(calculatedResult, convertedNumbers.ElementAt(i), operators.ElementAt(i - 1));

            return expectedResult == calculatedResult;
        }

        //konvertiert ein Wort zur Zahl anhand der aktuellen Wertekonfiguration
        private int convertWordToNumber(int[] valueSet, Word word)
        {
            char currentChar;
            double result = 0;
            int length = word.getCharacters().Count();
            for (int i = 0; i < length; i++)
            {
                currentChar = word.getCharacters()[i];
                result += valueSet[allChars.IndexOf(currentChar)] * Math.Pow(10, length - i - 1);

                //führende 0 unterbinden
                if (result == 0)
                    throw new LeadingZeroException();
            }
            return Convert.ToInt32(result);
        }

        //gibt alle benutzten distinktiven Charaktere des Inputstrings zurück
        public List<char> getUniqueCharacters()
        {
            List<char> chars = new List<char>();
            foreach (Word word in input.getInputWords())
            {
                foreach (char c in word.getCharacters())
                {
                    if (!chars.Contains(c))
                        chars.Add(c);
                }
            }
            foreach (char c in input.getResult().getCharacters())
            {
                if (!chars.Contains(c))
                    chars.Add(c);
            }

            if(chars.Count() > 10) throw new ArgumentOutOfRangeException();
            chars.Sort();
            return chars;
        }

        public List<char> getAllChars()
        {
            return allChars;
        }

        public List<int[]> getCorrectResults()
        {
            return correctResults;
        }
    }
}
