using dnlib.DotNet;
using Orochi.Core;
using Orochi.Core.Interfaces;
using Orochi.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Orochi.Protections
{
    public class JunkAdder : IProtection
    {
        public void Start(OrochiContext context)
        {
            //Doesn't work
            //Stuck in old injection method
            //Pull a request if you fix
            Importer imp = new Importer(context.Module);
            ModuleDefMD thisModule = ModuleDefMD.Load(Application.StartupPath + "//Orochi.Protections.dll");
            IMethod meth = imp.Import(typeof(JunkMethods).GetMethod("first"));
            TypeDef typeDef = thisModule.ResolveTypeDef(MDToken.ToRID(typeof(JunkMethods).MetadataToken));
            IEnumerable<IDnlibDef> members = RuntimeInjector.Inject(typeDef, context.Module.GlobalType, context.Module);
            MethodDef init = (MethodDef)members.Single(method => method.Name == "first");
            MethodDef init2 = (MethodDef)members.Single(method => method.Name == "second");
            foreach (TypeDef type in context.Module.Types)
            {
                type.Methods.Add(init);
                type.Methods.Add(init2);
                type.Methods.Add(init);
                type.Methods.Add(init2);
                type.Methods.Add(init);
                type.Methods.Add(init2);
            }
            Console.WriteLine("Junk codes injected");
        }
    }
}
