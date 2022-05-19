using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;

namespace Orochi.Protections.Mutations.IntMath
{
 public class INTMHelper
 {
  private Random rnd = new Random();
  public List<Instruction> Calc(int value)
  {
   List<Instruction> instructions = new List<Instruction>();
   int num = rnd.Next(int.MaxValue);
   bool once = Convert.ToBoolean(rnd.Next(1));
   int num1 = rnd.Next(int.MaxValue);

   instructions.Add(Instruction.Create(OpCodes.Ldc_I4, value - num + (once ? (0 - num1) : num1)));
   instructions.Add(Instruction.Create(OpCodes.Ldc_I4, num));
   instructions.Add(Instruction.Create(OpCodes.Add));
   instructions.Add(Instruction.Create(OpCodes.Ldc_I4, num1));
   instructions.Add(Instruction.Create(once ? OpCodes.Add : OpCodes.Sub));

   return instructions;
  }
 }
}
