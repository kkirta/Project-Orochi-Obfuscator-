using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Orochi.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Orochi.Core
{
    public class OrochiEngine
    {
        public OrochiEngine(ModuleDef module)
        {
            Module = module;
            Context = new OrochiContext(Module);
        }
        private void Watermark()
        {
            TypeRef attrRef = Module.CorLibTypes.GetTypeRef("System", "Attribute");
            var attrType = new TypeDefUser("", "OrochiAttribute", attrRef);
            Module.Types.Add(attrType);
            var ctor = new MethodDefUser(
                ".ctor",
                MethodSig.CreateInstance(Module.CorLibTypes.Void, Module.CorLibTypes.String),
                dnlib.DotNet.MethodImplAttributes.Managed,
                dnlib.DotNet.MethodAttributes.HideBySig | dnlib.DotNet.MethodAttributes.Public | dnlib.DotNet.MethodAttributes.SpecialName | dnlib.DotNet.MethodAttributes.RTSpecialName);
            ctor.Body = new CilBody();
            ctor.Body.MaxStack = 1;
            ctor.Body.Instructions.Add(OpCodes.Ldarg_0.ToInstruction());
            ctor.Body.Instructions.Add(OpCodes.Call.ToInstruction(new MemberRefUser(Module, ".ctor", MethodSig.CreateInstance(Module.CorLibTypes.Void), attrRef)));
            ctor.Body.Instructions.Add(OpCodes.Ret.ToInstruction());
            attrType.Methods.Add(ctor);
            var attr = new CustomAttribute(ctor);
            attr.ConstructorArguments.Add(new CAArgument(Module.CorLibTypes.String, "Obfuscated with Project Orochi, I LOVE YOU PRINZ EUGEN <3"));
            Module.CustomAttributes.Add(attr);
        }
        Assembly assembly = Assembly.LoadFile(Application.StartupPath + "//Orochi.Protections.dll");
        public void ApplyChangesAndWrite()
        {
            int i = 0;
            foreach (var v in ProtectionsTask)
            {
                Type type = assembly.GetTypes().First(x => x.Name == ProtectionsTask[i]);
                var instance = Activator.CreateInstance(type);
                MethodInfo info = type.GetMethod("Start");
                info.Invoke(instance, new object[] { Context });
                Console.WriteLine($"{type.Name} executed.");
                i++;
            }
            Watermark();
            Module.Write(Path.GetFileNameWithoutExtension(Module.Location) + "_weaved.exe");
            Console.WriteLine($"Your module is saved successfully. {Module.Location}");
        }
        #region Properties
        public OrochiContext Context { get; private set; } = null;
        public ModuleDef Module { get; private set; } = null;
        public List<string> ProtectionsTask { get; private set; } = new List<string>();
        #endregion
    }
}
