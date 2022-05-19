using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orochi.Protections.Mutations.Arithmetic.Utils
{
    public class ArithmeticUtils
    {
        public static bool CheckArithmetic(Instruction instruction)
        {
            if (!instruction.IsLdcI4())
                return false;
            if (instruction.GetLdcI4Value() == 1)
                return false;
            if (instruction.GetLdcI4Value() == 0)
                return false;
            return true;
        }

        public static double GetY(double x) => (x / 2);

        public static System.Reflection.MethodInfo GetMethod(ArithmeticTypes mathType)
        {
            switch (mathType)
            {
                case ArithmeticTypes.Abs:
                    return ((typeof(Math).GetMethod("Abs", new Type[] { typeof(double) })));
                case ArithmeticTypes.Round:
                    return ((typeof(Math).GetMethod("Round", new Type[] { typeof(double) })));
                case ArithmeticTypes.Sin:
                    return ((typeof(Math).GetMethod("Sin", new Type[] { typeof(double) })));
                case ArithmeticTypes.Cos:
                    return ((typeof(Math).GetMethod("Cos", new Type[] { typeof(double) })));
                case ArithmeticTypes.Log:
                    return ((typeof(Math).GetMethod("Log", new Type[] { typeof(double) })));
                case ArithmeticTypes.Log10:
                    return ((typeof(Math).GetMethod("Log10", new Type[] { typeof(double) })));
                case ArithmeticTypes.Sqrt:
                    return ((typeof(Math).GetMethod("Sqrt", new Type[] { typeof(double) })));
                case ArithmeticTypes.Ceiling:
                    return ((typeof(Math).GetMethod("Ceiling", new Type[] { typeof(double) })));
                case ArithmeticTypes.Floor:
                    return ((typeof(Math).GetMethod("Floor", new Type[] { typeof(double) })));
                case ArithmeticTypes.Tan:
                    return ((typeof(Math).GetMethod("Tan", new Type[] { typeof(double) })));
                case ArithmeticTypes.Tanh:
                    return ((typeof(Math).GetMethod("Tanh", new Type[] { typeof(double) })));
                case ArithmeticTypes.Truncate:
                    return ((typeof(Math).GetMethod("Truncate", new Type[] { typeof(double) })));
            }
            return null;
        }

        public static OpCode GetOpCode(ArithmeticTypes arithmetic)
        {
            switch (arithmetic)
            {
                case ArithmeticTypes.Add:
                    return OpCodes.Add;
                case ArithmeticTypes.Sub:
                    return OpCodes.Sub;
            }
            return null;
        }

        public static List<Instruction> GenerateBody(ArithmeticVT arithmeticVTs, ModuleDef module)
        {
            List<Instruction> instructions = new List<Instruction>();
            if (IsArithmetic(arithmeticVTs.GetArithmetic()))
            {
                instructions.Add(new Instruction(OpCodes.Ldc_R8, arithmeticVTs.GetValue().GetX()));
                instructions.Add(new Instruction(OpCodes.Ldc_R8, arithmeticVTs.GetValue().GetY()));

                if (arithmeticVTs.GetToken().GetOperand() != null)
                {
                    instructions.Add(new Instruction(OpCodes.Call, arithmeticVTs.GetToken().GetOperand()));
                }
                instructions.Add(new Instruction(arithmeticVTs.GetToken().GetOpCode()));
                instructions.Add(new Instruction(OpCodes.Call, module.Import(typeof(Convert).GetMethod("ToInt32", new Type[] { typeof(double) }))));
                //instructions.Add(new Instruction(OpCodes.Conv_I4));
            }
            else if (IsXor(arithmeticVTs.GetArithmetic()))
            {
                instructions.Add(new Instruction(OpCodes.Ldc_I4, (int)arithmeticVTs.GetValue().GetX()));
                instructions.Add(new Instruction(OpCodes.Ldc_I4, (int)arithmeticVTs.GetValue().GetY()));
                instructions.Add(new Instruction(arithmeticVTs.GetToken().GetOpCode()));
                instructions.Add(new Instruction(OpCodes.Conv_I4));
            }
            return instructions;
        }

        public static bool IsArithmetic(ArithmeticTypes arithmetic)
        {
            return arithmetic == ArithmeticTypes.Add || arithmetic == ArithmeticTypes.Sub || arithmetic == ArithmeticTypes.Div || arithmetic == ArithmeticTypes.Mul ||
                arithmetic == ArithmeticTypes.Abs || arithmetic == ArithmeticTypes.Log || arithmetic == ArithmeticTypes.Log10 || arithmetic == ArithmeticTypes.Truncate ||
                arithmetic == ArithmeticTypes.Sin || arithmetic == ArithmeticTypes.Cos || arithmetic == ArithmeticTypes.Floor || arithmetic == ArithmeticTypes.Round ||
                arithmetic == ArithmeticTypes.Tan || arithmetic == ArithmeticTypes.Tanh || arithmetic == ArithmeticTypes.Sqrt || arithmetic == ArithmeticTypes.Ceiling;
        }

        public static bool IsXor(ArithmeticTypes arithmetic)
        {
            return arithmetic == ArithmeticTypes.Xor;
        }
    }
}
