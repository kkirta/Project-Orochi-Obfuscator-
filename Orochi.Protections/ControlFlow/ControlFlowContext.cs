using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eddy_Protector_Core.Core;

namespace Eddy_Protector_Protections.Protections.ControlFlow1
{
	class Blocks
	{
		public List<Block> blocks = new List<Block>();
		public Block getBlock(int id)
		{
			return blocks.Single(block => block.ID == id);
		}

		public void Scramble(out Blocks incGroups)
		{
			var Random = new Random();
			Blocks groups = new Blocks();
			foreach (var group in blocks)
				groups.blocks.Insert(Random.Next(groups.blocks.Count), group);
			incGroups = groups;
		}
	}

	class Block
	{
		public int ID = 0;
		public int nextBlock = 0;
		public List<Instruction> instructions = new List<Instruction>();
	}

	class CFHelper
	{
		static Random rnd = new Random();
  public Context ctx;

		public bool HasUnsafeInstructions(MethodDef methodDef)
		{
			if (methodDef.HasBody)
			{
				if (methodDef.Body.HasVariables)
					return methodDef.Body.Variables.Any(x => x.Type.IsPointer);
			}
			return false;
		}
		public Blocks GetBlocks(MethodDef method)
		{
			Blocks blocks = new Blocks();
			Block block = new Block();
			int Id = 0;
			int usage = 0;
			foreach (Instruction instruction in method.Body.Instructions)
			{
				int pops = 0;
				int stacks;
				instruction.CalculateStackUsage(out stacks, out pops);
				block.instructions.Add(instruction);
				usage += stacks - pops;
				if (stacks == 0)
				{
					if (instruction.OpCode != OpCodes.Nop)
					{
						if (usage == 0 || instruction.OpCode == OpCodes.Ret)
						{

							block.ID = Id;
							Id++;
							block.nextBlock = block.ID + 1;
							blocks.blocks.Add(block);
							block = new Block();
						}
					}
				}
			}
			return blocks;
		}
		public List<Instruction> Calc(int value)
		{
			List<Instruction> instructions = new List<Instruction>();

   uint num = ctx.generator.RandomUint();

   bool once = Convert.ToBoolean(new Random().Next(2));

   uint num1 = ctx.generator.RandomUint();

   long initial = value - num + (once ? (0 - num1) : num1);

			instructions.Add(Instruction.Create(OpCodes.Ldc_I8, initial));
   instructions.Add(Instruction.Create(OpCodes.Ldc_I8, num));
			instructions.Add(Instruction.Create(OpCodes.Add));
			instructions.Add(Instruction.Create(OpCodes.Ldc_I8, num1));
			instructions.Add(Instruction.Create(once ? OpCodes.Add : OpCodes.Sub));
   return instructions;

		}
	}

	class DnlibUtils
	{
		public bool hasExceptionHandlers(MethodDef methodDef)
		{
			if (methodDef.Body.HasExceptionHandlers)
				return true;
			return false;
		}

		public bool Optimize(MethodDef methodDef)
		{
			if (methodDef.Body == null)
				return false;
			methodDef.Body.OptimizeMacros();
			methodDef.Body.OptimizeBranches();
			return true;
		}

		public bool Simplify(MethodDef methodDef)
		{
			if (methodDef.Parameters == null)
				return false;
			methodDef.Body.SimplifyMacros(methodDef.Parameters);
			return true;
		}

	}
}
