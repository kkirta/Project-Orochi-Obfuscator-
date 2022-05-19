using dnlib.DotNet;
using Orochi.Core;
using Orochi.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Orochi.Protections
{
    public class AntiTamper : IProtection
    {
        #region old codes (not working)
        /*
        Importer imp = new Importer(context.Module);
        Random rnd = new Random();
        ModuleDefMD RuntimeModule = ModuleDefMD.Load(Application.StartupPath + "//" + "Orochi.Runtime.dll");
        IMethod meth = imp.Import(typeof(ImmersiveFormInvoker).GetMethod("LoadNewForm"));
        //RuntimeInjector.Inject(thisModule.ResolveTypeDef(MDToken.ToRID(typeof(ImmersiveFormInvoker).MetadataToken)), context.Module.GlobalType, context.Module);
        TypeDef tempType = null;
        foreach (TypeDef typen in RuntimeModule.Types)
        {
            if(typen.Name == "ImmersiveFormInvoker")
            {
                tempType = typen;
              //RuntimeInjector.Inject(thisModule, context, meth, typen);
            }
        }
        TypeDef typeee = RuntimeInjector.Clone(tempType);
        typeee.Namespace = "";
        RuntimeInjector.Inject(typeee, context.Module);
        Console.WriteLine("Anti-tamper injected.");
        foreach (TypeDef type in context.Module.Types)
        {
            int keyy = rnd.Next(1, 99);
            foreach (MethodDef method in type.Methods)
            {

                if (method.Body != null && !method.Name.Contains("tor"))
                {
                    method.Body.SimplifyBranches();
                    method.Body.Instructions.Insert(1, new Instruction(OpCodes.Call, context.Module.Import(meth)));
                }

            }
        }
        Console.WriteLine("Methods edited for anti-tamper.");
        */
        #endregion
        public void Start(OrochiContext context)
        {
            MessageBox.Show("Read the comments in " + typeof(AntiTamper).Name + " class.");
            //Anti-tamper injection + method body editing.
            //Create an anti-tamper yourself!
            //If you do, pull a request xd
            //RuntimeTypeInjector.InjectAntiTamper(context, typeof(ImmersiveFormInvoker), "LoadNewForm");
        }
    }
}
