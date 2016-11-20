using System;
using Brainfuck.Interpreter;

namespace ConsoleTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var helloWorld =
                "++++++++++[>+++++++>++++++++++>+++>+<<<<-]>++.>+.+++++++..+++.>++.<<+++++++++++++++.>.+++.------.--------.>+.>.";
            var incrementToThreeAndPrint = "++++++++++ ++++++++++ ++++++++++ ++++++++++ ++++++++++ +.";

            var interpreter = new BrainfuckInterpreter(Console.OpenStandardInput(), Console.OpenStandardOutput());
            //interpreter.RunInteractive(helloWorld, 15, @is =>
            //{
            //    Console.Write($"{@is.PointerPosition} => ");
            //    Console.WriteLine(BitConverter.ToString(@is.Ram.ToArray()));
            //    Thread.Sleep(1000);
            //});
            interpreter.Run(helloWorld, 25);

            Console.ReadLine();
        }
    }
}