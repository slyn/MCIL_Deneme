using System;
using System.Reflection.Emit;

namespace CallingMethod_002
{
    class Program
    {
        static void Main(string[] args)
        {
            var myMethod = new DynamicMethod(
                "MultiplyMethod", 
                typeof(int), 
                new[] { typeof(int)},
                typeof(Program).Module);
            var il = myMethod.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldc_I4,42);
            il.Emit(OpCodes.Mul);
            il.Emit(OpCodes.Ret);

            var multiplyMethod = (Func<int,int>)myMethod.CreateDelegate(typeof(Func<int, int>));
            var result = multiplyMethod(10);
            Console.WriteLine(result);


            var calculateMethod = new DynamicMethod("CalculateMethod", typeof(int), new[] { typeof(int), typeof(int) }, typeof(Program).Module);
            var calcMethodIL = calculateMethod.GetILGenerator();

            calcMethodIL.Emit(OpCodes.Ldarg_0);
            calcMethodIL.Emit(OpCodes.Ldarg_1);
            calcMethodIL.Emit(OpCodes.Mul);
            calcMethodIL.Emit(OpCodes.Call,myMethod);
            calcMethodIL.Emit(OpCodes.Ret);

            var cMethodDelegate = (Func<int, int, int>)calculateMethod.CreateDelegate(typeof(Func<int,int,int>));
            var result2 = cMethodDelegate(100,100);
            Console.WriteLine(result2);
        }
    }
}
