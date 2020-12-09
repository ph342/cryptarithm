using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Alphametiken
{
    class Input
    {
        private List<Word> inputString;
        private List<Operator> operators;
        private Word result;

        //Liest die Gleichung ein und strukturiert die Buchstaben und Operatoren
        public void readIn(string input)
        {
            inputString = new List<Word>();
            operators = new List<Operator>();

            string[] splittedInput = prepareString(input.ToUpper()).Split(' ');
            for (int i = 0; i < splittedInput.Count() - 1; i += 2)
            {
                inputString.Add(new Word(splittedInput[i]));
                if (splittedInput[i + 1].Equals("+"))
                    operators.Add(Operator.PLUS);
                else if (splittedInput[i + 1].Equals("-"))
                    operators.Add(Operator.MINUS);
                else if (splittedInput[i + 1].Equals("/"))
                    operators.Add(Operator.DIVIDE);
                else if (splittedInput[i + 1].Equals("*"))
                    operators.Add(Operator.TIMES);
            }
            result = new Word(splittedInput[splittedInput.Count() - 1]);
        }

        /*
         * Bereitet einen String vor -> A+ B= C wird zu A + B = C um besser mit ihm
	     * arbeiten zu können
         */
        private string prepareString(string input)
        {
            string result = input.Trim();
            Regex rgx1 = new Regex("\\s{1,}");
            Regex rgx2 = new Regex("([-\\/+*=])");
            result = rgx1.Replace(result, "");
            return rgx2.Replace(result, " $1 ");
        }

        public List<Operator> getOperators()
        {
            return operators;
        }

        public List<Word> getInputWords()
        {
            return inputString;
        }

        public Word getResult()
        {
            return result;
        }
    }
}
