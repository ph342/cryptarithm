using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphametiken
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("------ Alphametiken ------\n\nBitte wählen Sie eine Option:\n(1) Eingabe einer Buchstabengleichung\n(2) Zahlalphametiken errechnen lassen\n");
            bool inputEqExit = false;
            string cons_input;
            //Struktur zum Aufbereiten der Eingabe
            Input input;
            Calculator calc;
            //Liste gefüllt mit den richtigen Zahlenfeldern
            List<int[]> results;
            //alle einzigartigen Charaktere der Eingabe
            List<char> chars;
            //für Aufgabe 2
            FindCryptarithm alphametics;

            while (!inputEqExit)
            {
                //Eingabe
                cons_input = Console.ReadLine();
                //auf "exit" prüfen
                inputEqExit = checkForExit(cons_input);
                if (inputEqExit) break;

                //Eingabe '1'
                if (cons_input.Contains('1'))
                {
                    Console.WriteLine("Bitte die zu lösende Gleichung eingeben (Beispiel: ABC – DBB = EFG)");
                    try
                    {
                        //Eingabe
                        cons_input = Console.ReadLine();
                        //auf "exit" prüfen
                        inputEqExit = checkForExit(cons_input);
                        if (inputEqExit) break;

                        //Verarbeitung
                        input = new Input();
                        input.readIn(cons_input);
                        Console.WriteLine("Berechne...");
                        calc = new Calculator();
                        results = calc.calcResults(input, false);
                        chars = calc.getUniqueCharacters();

                        if (results.Count() == 0) Console.WriteLine("Keine Lösungen gefunden!");
                        //Lösungen gefunden
                        else
                            Console.WriteLine("Es wurde{0} gefunden:\n", results.Count() == 1 ? " 1 Lösung" : "n " + results.Count().ToString() + " Lösungen");
                        
                        foreach (int[] r in results)
                        {
                            for (int i = 0; i < chars.Count(); i++)
                            {
                                if (i != 0) Console.Write(", ");
                                Console.Write("{0}={1}", chars.ElementAt(i), r.ElementAt(i));
                            }
                            Console.Write(" (" + substituteCorrectValues(cons_input, r, chars) + ")\n");
                        }

                        //Ende der Ausgabe, neue Iteration der while-Schleife
                        Console.WriteLine("\n---------------------------------------------------------\n\nBitte wählen Sie eine Option:\n(1) Eingabe einer Buchstabengleichung\n(2) Zahlalphametiken errechnen lassen (\"exit\" zum Beenden)");
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        Console.WriteLine("\nNicht mehr als 10 Buchstaben verwenden!\n");
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("\nFalsch formatierte Eingaben!\n");
                    }
                }

                //Eingabe '2'
                else if (cons_input.Contains('2'))
                {
                    alphametics = new FindCryptarithm();
                    alphametics.findCombinations();
                }

                else Console.WriteLine("Bitte eine 1 oder 2 eingeben!");                
            }
        }

        public static string substituteCorrectValues(string term, int[] values, List<char> characters)
        {
            string s = term.ToUpper();
            for (int i = 0; i < values.Count(); i++)
                s = s.Replace(Convert.ToString(characters.ElementAt(i)), Convert.ToString(values[i]));
            return s;
        }

        private static bool checkForExit(string inp)
        {
            if (inp.Trim().ToUpper() == "EXIT" || inp.Trim().ToLower() == "exit")   return true;
            else return false;
        }
    }
}
