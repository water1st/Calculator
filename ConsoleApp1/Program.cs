namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var calculator = Calculator.Instance;
            while (true)
            {
                Console.WriteLine("Enter your expression");
                var inputText = Console.ReadLine();
                calculator.Clear();

                calculator.Input(inputText);
                var result = calculator.Calculate();
                Console.WriteLine($"{inputText}={result}");
            }
        }

    }

}

