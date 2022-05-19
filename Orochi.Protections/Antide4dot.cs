using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Orochi.Core;
using Orochi.Core.Interfaces;

namespace Orochi.Protections
{
    public class AntiDe4dot : IProtection
    {

        internal InterfaceImpl CreateType(ModuleDef module)
        {
            InterfaceImpl interfaceM = new InterfaceImplUser(module.GlobalType);
            TypeDef typeDef = new TypeDefUser("", "<?!?!?!?!Orochi!?!?!?!?>", module.CorLibTypes.GetTypeRef("System", "Attribute"));
            InterfaceImpl @interface = new InterfaceImplUser(typeDef);
            module.Types.Add(typeDef);
            return @interface;
        }
        public void Start(OrochiContext context)
        {
            InterfaceImpl attrib = CreateType(context.Module);
            foreach (TypeDef type in context.Module.Types)
            {
                type.Interfaces.Add(attrib);
                type.Interfaces.Add(attrib);
            }
        }
    }
}