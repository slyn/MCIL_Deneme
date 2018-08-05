using System;
using System.Linq;
using System.Reflection.Emit;

namespace CallMethod_003
{
    class Program
    {
        static void Main(string[] args)
        {
            var add = Math.GetResult(20, 10, 0);
            var mul = Math.GetResult(20, 10, 1);
            var div = Math.GetResult(20, 10, 2);
            var sub = Math.GetResult(20, 10, 3);
            var zero = Math.GetResult(20, 10, -1);

            Console.WriteLine("Add : {0}",add);
            Console.WriteLine("Mul : {0}",mul);
            Console.WriteLine("Div : {0}",div);
            Console.WriteLine("Sub : {0}",sub);
            Console.WriteLine("Zero : {0}",zero);
            Console.WriteLine();

            var switchMethod = new DynamicMethod("MyMethod", typeof(int), new[] { typeof(int), typeof(int), typeof(int) }, typeof(Program).Module);
            var il = switchMethod.GetILGenerator();

            // jump table
            var jumpTable = new[]
            {
                il.DefineLabel(), // Add
                il.DefineLabel(), // Multiplication
                il.DefineLabel(), // Division
                il.DefineLabel(), // Substruction
                il.DefineLabel(), // Default
            };

            il.Emit(OpCodes.Ldarg_2);
            il.Emit(OpCodes.Switch,jumpTable);
            il.Emit(OpCodes.Br,jumpTable.Last());

            il.MarkLabel(jumpTable[0]);//Add
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Add);
            il.Emit(OpCodes.Ret);

            il.MarkLabel(jumpTable[1]);// mul
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Mul);
            il.Emit(OpCodes.Ret);

            il.MarkLabel(jumpTable[2]);//Div
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Div);
            il.Emit(OpCodes.Ret);

            il.MarkLabel(jumpTable[3]); // Sub
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Sub);
            il.Emit(OpCodes.Ret);

            il.MarkLabel(jumpTable[4]); // default
            il.Emit(OpCodes.Ldc_I4_0);
            il.Emit(OpCodes.Ret);

            var method = (Func<int, int, int, int>)switchMethod.CreateDelegate(typeof(Func<int, int, int, int>));
            var add2 = method(20, 10, 0);
            var mul2 = method(20, 10, 1);
            var div2 = method(20, 10, 2);
            var sub2 = method(20, 10, 3);
            var zero2 = method(20, 10, -1);

            Console.WriteLine("Add : {0}", add2);
            Console.WriteLine("Mul : {0}", mul2);
            Console.WriteLine("Div : {0}", div2);
            Console.WriteLine("Sub : {0}", sub2);
            Console.WriteLine("Zero : {0}", zero2);
            Console.WriteLine();
        }
    }
    public class Math
    {
        public static int GetResult(int a, int b, int operation)
        {
            switch (operation)
            {
                case 0:
                    return a + b;
                case 1:
                    return a * b;
                case 2:
                    return a / b;
                case 3:
                    return a - b;
                default:
                    return 0;
            }
        }
    }
}
