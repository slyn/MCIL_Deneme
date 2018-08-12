using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace CreateType_004
{
    class Program
    {
        static void Main(string[] args)
        {
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new System.Reflection.AssemblyName("Demo"), System.Reflection.Emit.AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("PersonModule");
            var typeBuilder = moduleBuilder.DefineType("Person", System.Reflection.TypeAttributes.Public);

            var nameField = typeBuilder.DefineField("name", typeof(string), System.Reflection.FieldAttributes.Private);

            var ctor = typeBuilder.DefineConstructor(System.Reflection.MethodAttributes.Public, System.Reflection.CallingConventions.Standard, new[] { typeof(string) });

            var ctorIL = ctor.GetILGenerator();

            ctorIL.Emit(OpCodes.Ldarg_0);
            ctorIL.Emit(OpCodes.Ldarg_1);
            ctorIL.Emit(OpCodes.Stfld, nameField);
            ctorIL.Emit(OpCodes.Ret);

            var nameProperty = typeBuilder.DefineProperty("Name", System.Reflection.PropertyAttributes.HasDefault, typeof(string), null);

            var namePropertyGetMethod = typeBuilder.DefineMethod("get_Name",
                System.Reflection.MethodAttributes.Public |
                System.Reflection.MethodAttributes.SpecialName |
                System.Reflection.MethodAttributes.HideBySig,
                typeof(string), Type.EmptyTypes);

            nameProperty.SetGetMethod(namePropertyGetMethod);

            var namePropertyGetMethodIL = namePropertyGetMethod.GetILGenerator();

            namePropertyGetMethodIL.Emit(OpCodes.Ldarg_0);
            namePropertyGetMethodIL.Emit(OpCodes.Ldfld, nameField);
            namePropertyGetMethodIL.Emit(OpCodes.Ret);

            var t = typeBuilder.CreateType();

            var nProperty = t.GetProperty("Name");

            var instance = Activator.CreateInstance(t, "Filip");
            var result = nProperty.GetValue(instance, null);
            Console.WriteLine(result);
        }
    }
}
