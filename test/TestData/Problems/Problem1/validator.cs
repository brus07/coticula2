using System;

public class ValidatorForSwapProblem
{
    private static void Main()
    {
        string[] tokens = Console.ReadLine().Split();
        if (tokens.Length != 2)
        {
                Console.WriteLine("the number of parameters should be 2");
                return;
        }
        int a = int.Parse(tokens[0]);
        int b = int.Parse(tokens[1]);
        if (a < 0 || a > 1000)
        {
                Console.WriteLine("a is bad");
                return;
        }
        if (b < 0 || b > 1000)
        {
                Console.WriteLine("b is bad");
                return;
        }
        Console.WriteLine("ok");
    }
}