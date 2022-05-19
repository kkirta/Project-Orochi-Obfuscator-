using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Orochi.Protections.Mutations.Arithmetic;
using Orochi.Protections.Mutations.Arithmetic.Structures;
using Orochi.Protections.Mutations.Arithmetic.Utils;

namespace Orochi.Protections.Mutations.Arithmetic.Function
{
    public class Xor : iFunction
    {
        public override ArithmeticTypes ArithmeticTypes => ArithmeticTypes.Xor;
        public override ArithmeticVT Arithmetic(Instruction instruction, ModuleDef module)
        {
            Utils.Generator generator = new Utils.Generator();
            if (!ArithmeticUtils.CheckArithmetic(instruction)) return null;
            ArithmeticEmulator arithmeticEmulator = new ArithmeticEmulator(instruction.GetLdcI4Value(), generator.Next(), ArithmeticTypes);
            return (new ArithmeticVT(new Value(arithmeticEmulator.GetValue(), arithmeticEmulator.GetY()), new Token(OpCodes.Xor), ArithmeticTypes));
        }
    }
}
