using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Orochi.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orochi.Core
{
    public static class RuntimeTypeInjector
    {
        //This one is not working
        /*
        public static TypeDef Inject(TypeDef type, OrochiContext context)
        {
            Console.WriteLine($"Starting injection of {type.Name} to {context.Module.Name}");
            context.Module.Types.Add(type);
            Console.WriteLine($"{type.Name} injected to {context.Module.Name} with token {type.MDToken.ToInt32().ToString()} succesfully.");
            return type;
        }
        */
        public static void InjectAntiTamper(OrochiContext context, Type type, string methodName)
        {
            ModuleDefMD typeModule = ModuleDefMD.Load(type.Module);
            MethodDef injectMethod;
            injectMethod = null;
            injectMethod = context.Module.GlobalType.FindOrCreateStaticConstructor();

            TypeDef typeDef = typeModule.ResolveTypeDef(MDToken.ToRID(type.MetadataToken));
            IEnumerable<IDnlibDef> members = RuntimeInjector.Inject(typeDef, context.Module.GlobalType, context.Module);

            MethodDef init = (MethodDef)members.Single(method => method.Name == methodName);

            injectMethod.Body.Instructions.Insert(0, Instruction.Create(OpCodes.Call, init));

            foreach (TypeDef typex in context.Module.Types)
            {
                if (typex.IsGlobalModuleType || typex.Name == "Resources" || typex.Name == "Settings" || typex.Name.Contains("Form"))
                    continue;
                foreach (MethodDef method in typex.Methods)
                {
                    if (!method.HasBody) continue;
                    if (method.IsConstructor)
                    {
                        method.Body.Instructions.Insert(0, Instruction.Create(OpCodes.Nop));
                        method.Body.Instructions.Insert(0, Instruction.Create(OpCodes.Call, init));
                    }
                }
            }

            foreach (MethodDef md in context.Module.GlobalType.Methods)
            {
                if (md.Name != ".ctor") continue;
                context.Module.GlobalType.Remove(md);
                break;
            }
        }
        internal static string xor(string str, int key)
        {
            StringBuilder builder = new StringBuilder();
            foreach (char c in str.ToCharArray())
                builder.Append((char)(c + key));
            return builder.ToString();
        }
        public static void InjectStringsProtection(OrochiContext context, Type type, string methodName, MethodDef encryptMethod)
        {
            ModuleDefMD typeModule = ModuleDefMD.Load(type.Module);
            MethodDef injectMethod;
            injectMethod = null;
            injectMethod = encryptMethod;

            TypeDef typeDef = typeModule.ResolveTypeDef(MDToken.ToRID(type.MetadataToken));
            IEnumerable<IDnlibDef> members = RuntimeInjector.Inject(typeDef, context.Module.GlobalType, context.Module);

            MethodDef init = (MethodDef)members.Single(method => method.Name == methodName);

            injectMethod.Body.Instructions.Insert(0, Instruction.Create(OpCodes.Call, init));
            int keyy = 1;
            foreach (TypeDef typex in context.Module.Types)
            {
                if (typex.IsGlobalModuleType || typex.Name == "Resources" || typex.Name == "Settings" || typex.Name.Contains("Form"))
                    continue;

                foreach (MethodDef method in typex.Methods)
                {
                    if (!method.Name.Contains("tor"))
                    {
                        method.Body.KeepOldMaxStack = false;
                        method.Body.SimplifyBranches();
                        for (int i = 0; i < method.Body.Instructions.Count; i++)
                        {
                            if (method.Body.Instructions[i].OpCode == OpCodes.Ldstr)
                            {
                                string willencrypted = method.Body.Instructions[i].Operand.ToString();
                                int newKey = keyy;
                                willencrypted = xor(willencrypted, newKey);
                                method.Body.Instructions.Insert(i, new Instruction(OpCodes.Ldstr, willencrypted));
                                method.Body.Instructions.Insert(i + 1, new Instruction(OpCodes.Ldc_I4, newKey));
                                method.Body.Instructions.Insert(i + 2, new Instruction(OpCodes.Call, init));
                                i += 4;
                            }
                        }
                    }            
                }
            }

            foreach (MethodDef md in context.Module.GlobalType.Methods)
            {
                if (md.Name != ".ctor") continue;
                context.Module.GlobalType.Remove(md);
                break;
            }
        }
    }
}
