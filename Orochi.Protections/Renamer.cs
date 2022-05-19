using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Orochi.Core;
using Orochi.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Orochi.Protections
{
    public interface IRenaming
    {
        ModuleDefMD Rename(ModuleDefMD module);
    }

    
    static class Generator
    {
        static readonly char[] unicodeCharset = new char[] { }
      .Concat(Enumerable.Range(0x200b, 5).Select(ord => (char)ord))
      .Concat(Enumerable.Range(0x2029, 6).Select(ord => (char)ord))
      .Concat(Enumerable.Range(0x206a, 6).Select(ord => (char)ord))
      .Except(new[] { '\u2029' })
      .ToArray();

        public static string GenerateString()
        {
            int lengt = 2;
            byte[] buffer = new byte[lengt];
            new RNGCryptoServiceProvider().GetBytes(buffer);

            string str_result = null;

            //for (int i = 0; i < 200; i++)
            //{
            // str_result += "_";
            //}

            str_result += EncodeString(buffer, unicodeCharset);
            //int counter = int.MaxValue;
            //foreach (byte b in buffer)
            //{
            // counter--;
            // str_result += (char)((int)b ^ (int)counter);
            //}
            return str_result;
        }

        static string EncodeString(byte[] buff, char[] charset)
        {
            int current = buff[0];
            var ret = new StringBuilder();
            for (int i = 1; i < buff.Length; i++)
            {
                current = (current << 8) + buff[i];
                while (current >= charset.Length)
                {
                    ret.Append(charset[current % charset.Length]);
                    ret.Append("<OrochiObfuscator>");
                    current /= charset.Length;
                }
            }
            if (current != 0)
                ret.Append(charset[current % charset.Length]);
            return ret.ToString();
        }


        public static byte[] GetBytes(int lenght)
        {
            byte[] b = new byte[lenght];
            new RNGCryptoServiceProvider().GetBytes(b);
            return b;
        }

    }
    class Renamer : IProtection
    {
        public void Start(OrochiContext context)
        {
            ModuleDefMD module = (ModuleDefMD)context.Module;

            IRenaming rnm = new NamespacesRenaming();

            module = rnm.Rename(module);

            rnm = new ModuleRenaming();

            module = rnm.Rename(module);

            rnm = new ClassesRenaming();

            module = rnm.Rename(module);

            rnm = new MethodsRenaming();

            module = rnm.Rename(module);

            rnm = new PropertiesRenaming();

            module = rnm.Rename(module);

            rnm = new FieldsRenaming();

            module = rnm.Rename(module);
        }
    }
    public class ModuleRenaming : IRenaming
    {
        private static Dictionary<string, string> _names = new Dictionary<string, string>();

        public ModuleDefMD Rename(ModuleDefMD module)
        {
            ModuleDefMD moduleToRename = module;
            moduleToRename.Name = Generator.GenerateString();
            moduleToRename.Assembly.Name = Generator.GenerateString();

            return ApplyChangesToResources(moduleToRename);
        }

        private static ModuleDefMD ApplyChangesToResources(ModuleDefMD module)
        {
            ModuleDefMD moduleToRename = module;

            foreach (TypeDef type in moduleToRename.GetTypes())
            {
                if (type.IsGlobalModuleType)
                    continue;

                foreach (MethodDef method in type.Methods)
                {
                    if (method.Name != "InitializeComponent")
                        continue;

                    var instr = method.Body.Instructions;

                    for (int i = 0; i < instr.Count - 3; i++)
                    {
                        if (instr[i].OpCode == OpCodes.Ldstr)
                        {
                            foreach (var item in _names)
                            {
                                if (item.Key == instr[i].Operand.ToString())
                                {
                                    instr[i].Operand = item.Value;
                                }
                            }
                        }
                    }
                }
            }

            return moduleToRename;
        }
    }
    public class FieldsRenaming : IRenaming
    {
        private static Dictionary<string, string> _names = new Dictionary<string, string>();

        public ModuleDefMD Rename(ModuleDefMD module)
        {
            ModuleDefMD moduleToRename = module;

            foreach (TypeDef type in moduleToRename.GetTypes())
            {
                if (type.IsGlobalModuleType)
                    continue;

                foreach (var field in type.Fields)
                {
                    string nameValue;
                    if (_names.TryGetValue(field.Name, out nameValue))
                        field.Name = nameValue;
                    else
                    {
                        string newName = Generator.GenerateString();

                        _names.Add(field.Name, newName);
                        field.Name = newName;
                    }
                }
            }

            return ApplyChangesToResources(moduleToRename);
        }

        private static ModuleDefMD ApplyChangesToResources(ModuleDefMD module)
        {
            ModuleDefMD moduleToRename = module;

            foreach (TypeDef type in moduleToRename.GetTypes())
            {
                if (type.IsGlobalModuleType)
                    continue;

                foreach (MethodDef method in type.Methods)
                {
                    if (method.Name != "InitializeComponent")
                        continue;

                    var instr = method.Body.Instructions;

                    for (int i = 0; i < instr.Count - 3; i++)
                    {
                        if (instr[i].OpCode == OpCodes.Ldstr)
                        {
                            foreach (var item in _names)
                            {
                                if (item.Key == instr[i].Operand.ToString())
                                {
                                    instr[i].Operand = item.Value;
                                }
                            }
                        }
                    }
                }
            }

            return moduleToRename;
        }
    }
    public class ClassesRenaming : IRenaming
    {
        private static Dictionary<string, string> _names = new Dictionary<string, string>();

        public ModuleDefMD Rename(ModuleDefMD module)
        {
            ModuleDefMD moduleToRename = module;

            foreach (TypeDef type in moduleToRename.GetTypes())
            {
                if (type.IsGlobalModuleType)
                    continue;
                if (type.Name == "GeneratedInternalTypeHelper" || type.Name == "Resources" || type.Name == "Settings")
                    continue;

                string nameValue;
                if (_names.TryGetValue(type.Name, out nameValue))
                    type.Name = nameValue;
                else
                {
                    string newName = Generator.GenerateString();

                    _names.Add(type.Name, newName);
                    type.Name = newName;
                }
            }

            return ApplyChangesToResources(moduleToRename);
        }

        private static ModuleDefMD ApplyChangesToResources(ModuleDefMD module)
        {
            ModuleDefMD moduleToRename = module;

            foreach (var resource in moduleToRename.Resources)
            {
                foreach (var item in _names)
                {
                    if (resource.Name.Contains(item.Key))
                    {
                        resource.Name = resource.Name.Replace(item.Key, item.Value);
                    }
                }
            }

            foreach (TypeDef type in moduleToRename.GetTypes())
            {
                foreach (var property in type.Properties)
                {
                    if (property.Name != "ResourceManager")
                        continue;

                    var instr = property.GetMethod.Body.Instructions;

                    for (int i = 0; i < instr.Count; i++)
                    {
                        if (instr[i].OpCode == OpCodes.Ldstr)
                        {
                            foreach (var item in _names)
                            {
                                if (instr[i].Operand.ToString().Contains(item.Key))
                                    instr[i].Operand = instr[i].Operand.ToString().Replace(item.Key, item.Value);
                            }
                        }
                    }
                }
            }

            return moduleToRename;
        }
    }

    public class MethodsRenaming : IRenaming
    {
        public ModuleDefMD Rename(ModuleDefMD module)
        {
            ModuleDefMD moduleToRename = module;

            foreach (TypeDef type in moduleToRename.GetTypes())
            {
                if (type.IsGlobalModuleType)
                    continue;

                if (type.Name == "GeneratedInternalTypeHelper")
                    continue;

                foreach (MethodDef method in type.Methods)
                {
                    if (!method.HasBody)
                        continue;

                    if (method.Name == ".ctor" || method.Name == ".cctor")
                        continue;

                    method.Name = Generator.GenerateString();
                }
            }

            return moduleToRename;
        }
    }

    public class NamespacesRenaming : IRenaming
    {
        private static Dictionary<string, string> _names = new Dictionary<string, string>();

        public ModuleDefMD Rename(ModuleDefMD module)
        {
            ModuleDefMD moduleToRename = module;

            foreach (TypeDef type in moduleToRename.GetTypes())
            {
                if (type.IsGlobalModuleType)
                    continue;

                if (type.Namespace == "")
                    continue;

                string nameValue;
                if (_names.TryGetValue(type.Namespace, out nameValue))
                    type.Namespace = nameValue;
                else
                {
                    string newName = Generator.GenerateString();

                    _names.Add(type.Namespace, newName);
                    type.Namespace = newName;
                }
            }

            return ApplyChangesToResources(moduleToRename);
        }

        private static ModuleDefMD ApplyChangesToResources(ModuleDefMD module)
        {
            ModuleDefMD moduleToRename = module;

            foreach (var resource in moduleToRename.Resources)
            {
                foreach (var item in _names)
                {
                    if (resource.Name.Contains(item.Key))
                    {
                        resource.Name = resource.Name.Replace(item.Key, item.Value);
                    }
                }
            }

            foreach (TypeDef type in moduleToRename.GetTypes())
            {
                foreach (var property in type.Properties)
                {
                    if (property.Name != "ResourceManager")
                        continue;

                    var instr = property.GetMethod.Body.Instructions;

                    for (int i = 0; i < instr.Count; i++)
                    {
                        if (instr[i].OpCode == OpCodes.Ldstr)
                        {
                            foreach (var item in _names)
                            {
                                if (instr[i].ToString().Contains(item.Key))
                                    instr[i].Operand = instr[i].Operand.ToString().Replace(item.Key, item.Value);
                            }
                        }
                    }
                }
            }

            return moduleToRename;
        }
    }

    public class PropertiesRenaming : IRenaming
    {
        public ModuleDefMD Rename(ModuleDefMD module)
        {
            ModuleDefMD moduleToRename = module;

            foreach (TypeDef type in moduleToRename.GetTypes())
            {
                if (type.IsGlobalModuleType)
                    continue;

                foreach (var property in type.Properties)
                {
                    property.Name = Generator.GenerateString();
                }
            }

            return moduleToRename;
        }
    }
}
