using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Brainfuck.Interpreter
{
    public class BrainfuckInterpreter
    {
        private readonly Dictionary<char, Func<int, int>> _actions = new Dictionary<char, Func<int, int>>();
        private readonly Stack<int> _pointerStack = new Stack<int>();
        private int _pointer;
        private byte[] _ram;

        public BrainfuckInterpreter(Stream input, Stream output)
        {
            _actions.Add('+', index =>
            {
                _ram[_pointer] += 1;
                return index;
            });
            _actions.Add('-', index =>
            {
                _ram[_pointer] -= 1;
                return index;
            });
            _actions.Add('>', index =>
            {
                _pointer++;
                return index;
            });
            _actions.Add('<', index =>
            {
                _pointer--;
                return index;
            });
            _actions.Add('.', index =>
            {
                output.WriteByte(_ram[_pointer]);
                return index;
            });
            _actions.Add('[', index =>
            {
                _pointerStack.Push(index);
                return index;
            });
            _actions.Add(']', index =>
            {
                if (_ram[_pointer] != 0)
                    return _pointerStack.Peek();
                _pointerStack.Pop();
                return index;
            });
            _actions.Add(',', index =>
            {
                _ram[_pointer] = (byte) input.ReadByte();
                return index;
            });
        }

        public void Run(string code, int ramSize = 30000, CancellationToken cancellation = default(CancellationToken))
        {
            _ram = new byte[ramSize];
            var codePoints = code.ToCharArray();

            for (var index = 0; index < codePoints.Length; index++)
            {
                var codePoint = codePoints[index];
                if (cancellation.IsCancellationRequested) return;

                Func<int, int> action;
                if (_actions.TryGetValue(codePoint, out action))
                {
                    index = action(index);
                }
            }
        }

        public async Task RunInteractive(string code, int ramSize, Func<InterpreterState, Task> investigate,
            CancellationToken cancellation = default(CancellationToken))
        {
            _ram = new byte[ramSize];
            var codePoints = code.ToCharArray();

            for (var index = 0; index < codePoints.Length; index++)
            {
                await investigate(new InterpreterState(_pointer, _ram));
                var codePoint = codePoints[index];
                if (cancellation.IsCancellationRequested) return;

                Func<int, int> action;
                if (!_actions.TryGetValue(codePoint, out action)) continue;

                action(index);
            }
        }
    }
}