using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphametiken
{
    class FindCryptarithm
    {
        private Object[,] numbers = new Object[21, 2]{{0, "NULL"},
                                                      {1, "EINS"},
                                                      {2, "ZWEI"},
                                                      {3, "DREI"},
                                                      {4, "VIER"},
                                                      {5, "FUENF"},
                                                      {6, "SECHS"},
                                                      {7, "SIEBEN"},
                                                      {8, "ACHT"},
                                                      {9, "NEUN"},
                                                      {10, "ZEHN"},
                                                      {11, "ELF"},
                                                      {12, "ZWOELF"},
                                                      {13, "DREIZEHN"},
                                                      {14, "VIERZEHN"},
                                                      {15, "FUENFZEHN"},
                                                      {16, "SECHZEHN"},
                                                      {17, "SIEBZEHN"},
                                                      {18, "ACHTZEHN"},
                                                      {19, "NEUNZEHN"},
                                                      {20, "ZWANZIG"}};

        public void findCombinations()
        {
            Console.WriteLine("Suche Zahl-Alphametiken...");

            //verschachtelte Schleife für die Kombinationen zweier Wörter
            for (int firstNumber = 1; firstNumber < numbers.GetLength(0); firstNumber++)
            {
                for (int secondNumber = 1; secondNumber < numbers.GetLength(0); secondNumber++)
                {
                    if (firstNumber + secondNumber < numbers.GetLength(0))
                        calculateThisString(numbers[firstNumber, 1] + "+" + numbers[secondNumber, 1] + "=" + numbers[firstNumber + secondNumber, 1]);
                    if (firstNumber - secondNumber > 0)
                        calculateThisString(numbers[firstNumber, 1] + "-" + numbers[secondNumber, 1] + "=" + numbers[firstNumber - secondNumber, 1]);
                    if (firstNumber / secondNumber > 0 && firstNumber % secondNumber == 0)
                        calculateThisString(numbers[firstNumber, 1] + "/" + numbers[secondNumber, 1] + "=" + numbers[firstNumber / secondNumber, 1]);
                    if (firstNumber * secondNumber < numbers.GetLength(0))
                        calculateThisString(numbers[firstNumber, 1] + "*" + numbers[secondNumber, 1] + "=" + numbers[firstNumber * secondNumber, 1]);
                }
            }
        }

        private void calculateThisString(string term)
        {
            //generierten Term wie eine Benutzereingabe behandeln
            Input input = new Input();
            input.readIn(term);
            Calculator calculator = new Calculator();
            List<int[]> calculatedResults;

            try
            {
                calculatedResults = calculator.calcResults(input, true);
                //Erfolg
                if (calculatedResults.Count() > 0)
                {
                    Console.WriteLine(term + ":");
                    //das korrekte Zahlenset (es wird nur eins zurückgegeben)
                    int[] replacedWith = calculatedResults.ElementAt(0);
                    //Ergebnisausgabe
                    for (int i = 0; i < calculator.getUniqueCharacters().Count(); i++)
                    {
                        if (i != 0)
                            Console.Write(", ");
                        Console.Write("{0}={1}", calculator.getUniqueCharacters().ElementAt(i), replacedWith.ElementAt(i));
                    }
                    //den kompletten ersetzten Term hintendran schreiben
                    Console.Write(" (" + Program.substituteCorrectValues(term, replacedWith, calculator.getUniqueCharacters()) + ")\n");
                }
            }
            //mehr als 10 Buchstaben abfangen
            catch (ArgumentOutOfRangeException) { }
        }
    }
}
