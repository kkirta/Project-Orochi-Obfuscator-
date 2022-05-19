using System;
using System.Collections.Generic;
using dnlib.DotNet.Emit;
using dnlib.DotNet;

namespace Orochi.Protections.Mutations.IntMath
{
 public class IntMathProtection
 {

 }
 public class RuntimeIntMathProtection
 {
  public void Protect(MethodDef method)
  {
   DoIntMath(method);
  }


  public void DoIntMath(MethodDef method)
  {

   INTMHelper IMHelper = new INTMHelper();

   for (int i = 0; i < method.Body.Instructions.Count; i++)
   {
    Instruction instruction = method.Body.Instructions[i];
    if (instruction.Operand is int)
    {
     List<Instruction> instructions = IMHelper.Calc(Convert.ToInt32(instruction.Operand));
     instruction.OpCode = OpCodes.Nop;
     foreach (Instruction instr in instructions)
     {
      method.Body.Instructions.Insert(i + 1, instr);
      i++;
     }

    }
   }
  }

 }

}
