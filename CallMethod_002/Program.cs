using System;
using System.Reflection.Emit;

namespace CallMethod_002
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = Factorial.Calculate(3);
            Console.WriteLine(result);

            #region [ Dynamic Method ]
            var factorial = new DynamicMethod("Factorial", typeof(int), new[] { typeof(int) }, typeof(Program).Module);

            var il = factorial.GetILGenerator();

            var methodEnd = il.DefineLabel();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_0);

            il.Emit(OpCodes.Ldc_I4_1);
            il.Emit(OpCodes.Beq,methodEnd);

            il.Emit(OpCodes.Ldc_I4_1);
            il.Emit(OpCodes.Sub);

            il.Emit(OpCodes.Call,factorial);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Mul);

            il.MarkLabel(methodEnd);
            il.Emit(OpCodes.Ret);

            var facDelegate = (Func<int, int>)factorial.CreateDelegate(typeof(Func<int, int>));
            var res = facDelegate(3);
            Console.WriteLine(res);
            #endregion

        }
    }

    public class Factorial
    {
        // 5! = 5x4x3x2x1
        public static int Calculate(int i)
        {
            if (i == 1) return i;
            return i * Calculate(i - 1);
        }
    }
}
