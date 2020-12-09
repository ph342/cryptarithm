namespace Alphametiken
{
    enum Operator
    {
        PLUS, MINUS, DIVIDE, TIMES
    }

    static class OperatorMethods
    {
        //verrechnet 2 Werte
        public static int calculateTwoValues(int value1, int value2, Operator op)
        {
            int result = 0;
            switch (op)
            {
                case Operator.PLUS:
                    result = value1 + value2;
                    break;
                case Operator.MINUS:
                    result = value1 - value2;
                    break;
                case Operator.TIMES:
                    result = value1 * value2;
                    break;
                case Operator.DIVIDE:
                    result = value1 / value2;
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}
