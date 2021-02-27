using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Com.Trakindo.Utility
{
    public class Assembly
    {
        public static Dictionary<string, System.Reflection.Assembly> assmeblies = new Dictionary<string, System.Reflection.Assembly>();
        public static object LoadObject(string filename, string className)
        {
            System.Reflection.Assembly asm = null;
            if (assmeblies.ContainsKey(filename))
                asm = assmeblies[filename];
            else
            {
                asm = System.Reflection.Assembly.LoadFrom(filename);
                assmeblies.Add(filename, asm);
            }
            var type = asm.GetType(className);
            object o = Activator.CreateInstance(type);
            return o;
        }

        public static PropertyInfo GetKeyProperty(Type type)
        {
            foreach(PropertyInfo prop in type.GetProperties())
            {
                KeyAttribute keyAttribute = type.GetProperty(prop.Name).GetCustomAttributes(typeof(KeyAttribute), false).Cast<KeyAttribute>().Single();
                if (keyAttribute != null)
                    return prop;
            }

            return null;
        }

        public static object ExecuteCode(string scode, dynamic param)
        {
            ICodeCompiler compiler = (new CSharpCodeProvider()).CreateCompiler();
            CompilerParameters compilerParameters = new CompilerParameters();
            compilerParameters.ReferencedAssemblies.Add("system.dll");
            compilerParameters.GenerateExecutable = false;
            compilerParameters.GenerateInMemory = true;

            StringBuilder code = new StringBuilder();
            code.Append("using System; \n");
            code.Append("namespace Eval { \n");
            code.Append("  public class Evaluator { \n");
            code.Append("       public bool Execute(dynamic x) {\n");
            code.Append("            " + scode + ";\n");
            code.Append("       }\n");
            code.Append("   }\n");
            code.Append("}\n");

            CompilerResults cr = compiler.CompileAssemblyFromSource(compilerParameters, code.ToString());
            System.Reflection.Assembly assembly = cr.CompiledAssembly;
            object compiled = assembly.CreateInstance("Eval.Evaluator");

            MethodInfo mi = compiled.GetType().GetMethod("Execute");
            return mi.Invoke(compiled, new object[] { param });

        }
    }
}
