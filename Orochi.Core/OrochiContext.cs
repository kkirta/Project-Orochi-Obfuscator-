using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orochi.Core
{
    public class OrochiContext
    {
        public OrochiContext(ModuleDef currentModule)
        {
            Module = currentModule;
            ModulePath = Module.Location;
            Assembly = Module.Assembly;
        }
        #region Properties
        public AssemblyDef Assembly { get; private set; } = null;
        public MethodDef Method { get; private set; } = null;
        public ModuleDef Module { get; private set; } = null;
        public string ModulePath { get; private set; } = null;
        #endregion
    }
}
