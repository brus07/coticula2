using System;

public class Swap
{
    private static void Main()
    {
        string[] tokens = Console.ReadLine().Split();
        Console.WriteLine("{0} {1}", int.Parse(tokens[1]), int.Parse(tokens[0]));
    }
}