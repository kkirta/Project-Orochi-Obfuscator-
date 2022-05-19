using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Orochi.Core;
using Orochi.Core.Interfaces;
using System;
using System.Windows.Forms;

namespace Orochi.Protections
{
    public class StringEncryption : IProtection
    {
        #region old codes (not working)
        /*
            Importer imp = new Importer(context.Module);
            Random rnd = new Random();
            ModuleDefMD thisModule = ModuleDefMD.Load(Application.StartupPath + "//" + AppDomain.CurrentDomain.FriendlyName);
            IMethod meth = imp.Import(typeof(GlobalTypeIdentitation).GetMethod("Load"));
            RuntimeInjector.Inject(thisModule.ResolveTypeDef(MDToken.ToRID(typeof(GlobalTypeIdentitation).MetadataToken)), context.Module.GlobalType, context.Module);
            Console.WriteLine("String encryption is injected");
            foreach (TypeDef type in context.Module.Types)
            {
                int keyy = rnd.Next(1, 99);
                foreach (MethodDef method in type.Methods)
                {
                    if (method.Body == null) continue;
                    method.Body.SimplifyBranches();
                    for (int i = 0; i < method.Body.Instructions.Count; i++)
                    {
                        if (method.Body.Instructions[i].OpCode == OpCodes.Ldstr)
                        {
                            string willencrypted = method.Body.Instructions[i].Operand.ToString();
                            int newKey = keyy;
                            willencrypted = GlobalTypeIdentitation.Load(willencrypted, newKey);
                            method.Body.Instructions[i].OpCode = OpCodes.Nop;
                            method.Body.Instructions.Insert(i + 1, new Instruction(OpCodes.Ldstr, willencrypted));
                            method.Body.Instructions.Insert(i + 2, new Instruction(OpCodes.Ldc_I4, newKey));
                            method.Body.Instructions.Insert(i + 3, new Instruction(OpCodes.Call, context.Module.Import(meth)));
                            i += 4;
                        }
                    }
                }
            }
        */
        #endregion
        public void Start(OrochiContext context)
        {
            //Doesn't work without mutations protection
            //Pull a request if you fix
            foreach(TypeDef type in context.Module.Types)
            {
                foreach(MethodDef method in type.Methods)
                {
                    RuntimeTypeInjector.InjectStringsProtection(context, typeof(GlobalTypeIdentitation), "Load", method);
                }
            }
        }
    }
}
