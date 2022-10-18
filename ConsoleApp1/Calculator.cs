using System.Text;

namespace ConsoleApp1
{
    interface ICalculator
    {
        void Input(string inputText);
        decimal Calculate();
        void Clear();
    }

    class Calculator : ICalculator
    {
        private string inputText;
        private Stack<decimal> numberStack = new Stack<decimal>();
        private Stack<char> operatorStack = new Stack<char>();
        private const char PLUS_SIGN = '+';
        private const char MINUS_SIGN = '-';
        private const char MULTIPLICATION_SIGN = '*';
        private const char DIVISION_SIGN = '/';
        private const char LEFT_PARENTHESIS = '(';
        private const char RIGHT_PARENTHESIS = ')';
        private static readonly ICalculator calculator = new Calculator();
        private Calculator() { }

        public static ICalculator Instance => calculator;
        public void Input(string inputText)
        {
            this.inputText = inputText;
        }

        private void Read()
        {
            var inputStr = inputText.ToArray();
            for (int i = 0; i < inputStr.Length; i++)
            {
                //如果当前字符为数字
                if (!IsOperator(inputStr[i]))
                {
                    var stringBuilder = new StringBuilder();
                    while (i < inputStr.Length && !IsOperator(inputStr[i]))
                    {
                        stringBuilder.Append(inputStr[i]);
                        i++;
                    }
                    i--;

                    var number = Convert.ToDecimal(stringBuilder.ToString());
                    numberStack.Push(number);
                }
                //算数运算符'+' '-' '*' '/'
                else if (IsOperatorWithOutParenthesis(inputStr[i]))
                {
                    if (operatorStack.Count.Equals(0) || operatorStack.Peek().Equals('('))
                    {
                        operatorStack.Push(inputStr[i]);
                    }
                    //如果当前的优先级比栈内优先级更高，则把当前优先级入栈，否则，即刻出栈，把当前栈内的值运算好了
                    else if (OperatorPrecedence(inputStr[i]) > OperatorPrecedence(operatorStack.Peek()))
                    {
                        operatorStack.Push(inputStr[i]);
                    }
                    else
                    {
                        var sum = PopStack();
                        numberStack.Push(sum);
                        operatorStack.Push(inputStr[i]);
                    }
                }
                //左括号'(' 和 右括号 ')'
                else
                {
                    if (inputStr[i].Equals('('))
                    {
                        operatorStack.Push(inputStr[i]);
                    }
                    else if (inputStr[i].Equals(')'))
                    {
                        while (!operatorStack.Peek().Equals('('))
                        {
                            var sum = PopStack();
                            numberStack.Push(sum);
                        }
                        operatorStack.Pop();

                    }
                }
            }
        }

        /// <summary>
        /// 是否运算符
        /// </summary>
        private bool IsOperator(char @char)
        {
            return (@char.Equals(PLUS_SIGN) ||
                @char.Equals(MINUS_SIGN) ||
                @char.Equals(MULTIPLICATION_SIGN) ||
                @char.Equals(DIVISION_SIGN) ||
                @char.Equals(LEFT_PARENTHESIS) ||
                @char.Equals(RIGHT_PARENTHESIS));
        }

        /// <summary>
        /// 是否除加减乘除运算符
        /// </summary>
        private bool IsOperatorWithOutParenthesis(char @char)
        {
            return (@char.Equals(PLUS_SIGN) ||
                @char.Equals(MINUS_SIGN) ||
                @char.Equals(MULTIPLICATION_SIGN) ||
                @char.Equals(DIVISION_SIGN));
        }

        /// <summary>
        /// 计算
        /// </summary>
        private decimal Calculate(decimal firstNumber, decimal secondNumber, char @operator)
        {
            decimal sum = 0;
            switch (@operator)
            {
                case PLUS_SIGN: sum = firstNumber + secondNumber; break;
                case MINUS_SIGN: sum = firstNumber - secondNumber; break;
                case MULTIPLICATION_SIGN: sum = firstNumber * secondNumber; break;
                case DIVISION_SIGN: sum = firstNumber / secondNumber; break;
            }
            return sum;
        }

        /// <summary>
        /// 操作符优先级
        /// </summary>
        private int OperatorPrecedence(char @char)
        {
            int i = 0;
            switch (@char)
            {
                case PLUS_SIGN: i = -1; break;
                case MINUS_SIGN: i = -1; break;
                case MULTIPLICATION_SIGN: i = 1; break;
                case DIVISION_SIGN: i = 1; break;
            }
            return i;
        }

        /// <summary>
        /// 出栈一次
        /// </summary>
        private decimal PopStack()
        {
            decimal firstNumber, secondNumber;
            char @operator;
            secondNumber = numberStack.Pop();
            firstNumber = numberStack.Pop();
            @operator = operatorStack.Pop();
            decimal sum = Calculate(firstNumber, secondNumber, @operator);
            return sum;
        }

        /// <summary>
        /// 计算
        /// </summary>
        public decimal Calculate()
        {
            Read();

            decimal sum = 0;
            while (operatorStack.Count != 0)
            {
                sum = PopStack();
                numberStack.Push(sum);
            }
            return sum;
        }
        /// <summary>
        /// 清除
        /// </summary>
        public void Clear()
        {
            numberStack.Clear();
            operatorStack.Clear();
            inputText = string.Empty;
        }
    }
}
