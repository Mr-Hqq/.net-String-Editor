using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace WindowsFormsApplication1
{
    public static class StringHelper
    {
        public static void ReplaceString(string old, string replacement, AssemblyDefinition asm)
        {
            try
            {
                foreach (ModuleDefinition mod in asm.Modules)
                {
                    foreach (TypeDefinition td in mod.Types)
                    {
                        IterateType(td, old, replacement);
                    }
                }
            }
            catch { }
        }
        public static void IterateType(TypeDefinition td, string old, string replacement)
        {
            foreach (TypeDefinition ntd in td.NestedTypes)
            {
                IterateType(ntd, old, replacement);
            }

            foreach (MethodDefinition md in td.Methods)
            {
                if (md.HasBody)
                {
                    for (int i = 0; i < md.Body.Instructions.Count - 1; i++)
                    {
                        Instruction inst = md.Body.Instructions[i];
                        if (inst.OpCode == OpCodes.Ldstr)
                        {
                            if (inst.Operand.ToString().Contains(old))
                            {
                                inst.Operand = replacement;
                            }
                        }
                    }
                }
            }
        }
    }
}
