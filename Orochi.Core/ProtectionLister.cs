using dnlib.DotNet;
using Orochi.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace Orochi.Core
{
    public static class ProtectionLister
    {
        public static List<TypeDef> listTypes()
        {
            List<TypeDef> protections = new List<TypeDef>();
            ModuleDefMD module = ModuleDefMD.Load(Environment.CurrentDirectory + "//Orochi.Protections.dll");
            Importer imp = new Importer(module);
            foreach (TypeDef type in module.Types)
            {
                foreach(InterfaceImpl iImpl in type.Interfaces)
                {
                    if(iImpl.Interface.Name == "IProtection")
                    {
                        protections.Add(type);
                    }
                }
            }
            return protections;
        }

    }
}
