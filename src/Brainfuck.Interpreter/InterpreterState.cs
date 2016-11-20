using System.Collections.Generic;

namespace Brainfuck.Interpreter
{
    public sealed class InterpreterState
    {
        private int Pointer { get; } 
        private IReadOnlyCollection<byte> Ram { get; }  

        public InterpreterState(int pointer, byte[] ram)
        {
            Pointer = pointer;
            Ram = ram;
        }
    }
}