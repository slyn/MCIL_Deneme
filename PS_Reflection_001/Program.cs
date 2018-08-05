using System;
using System.Reflection.Emit;

namespace PS_Reflection_001
{
    class Program
    {

        static double Divider(int a , int b)
        {
            return a / b;
        }

        delegate double DivideDelegate(int a, int b);


        static void Main(string[] args)
        {
            ////#region 1
            //////var result = Divider(10, 2);
            //////Console.WriteLine(result); 
            ////#endregion

            ////#region 2
            ////var myMethod = new DynamicMethod("DivideMethod", typeof(double), new[] { typeof(int), typeof(int) }, typeof(Program).Module);

            ////var il = myMethod.GetILGenerator();
            ////il.Emit(OpCodes.Ldarg_0);
            ////il.Emit(OpCodes.Ldarg_1);
            ////il.Emit(OpCodes.Div);

            ////il.Emit(OpCodes.Ret);

            ////var result = myMethod.Invoke(null, new object[] { 10, 2 });
            ////Console.WriteLine(result);
            ////#endregion

            ////var method = (DivideDelegate)myMethod.CreateDelegate(typeof(DivideDelegate));
            ////var result2 = method.Invoke(10, 2);
            ////Console.WriteLine(result2);

            //var myMethod = new DynamicMethod("CalcMethod",
            //    typeof(int), 
            //    new[] { typeof(int), typeof(int),typeof(int) },
            //    typeof(Program).Module);
            //var il = myMethod.GetILGenerator();
            //il.DeclareLocal(typeof(int));
            //il.Emit(OpCodes.Ldarg_0);
            //il.Emit(OpCodes.Ldarg_1);
            //il.Emit(OpCodes.Mul);
            //il.Emit(OpCodes.Stloc_0);// local variable alanında tutmak için 
            //// pop from evoliationstack
            //il.Emit(OpCodes.Ldloc_0); // tekrar evaluation stack'e ekler
            //il.Emit(OpCodes.Ldarg_2);
            //il.Emit(OpCodes.Sub);

            //il.Emit(OpCodes.Ret);


            //var method = (Func<int, int, int, int>)myMethod.CreateDelegate(typeof(Func<int, int, int, int>));
            //var result = method(10, 10, 5);
            //Console.WriteLine(result);

            var calc = new DynamicMethod("CalcMethod",
                typeof(int),
                new[] {  typeof(int) },
                typeof(Program).Module);
            var il = calc.GetILGenerator();

            var loopStart = il.DefineLabel();
            var methodEnd = il.DefineLabel();

            // variables 0 : result = 0
            il.DeclareLocal(typeof(int));

            il.Emit(OpCodes.Ldc_I4_0);
            il.Emit(OpCodes.Stloc_0);
            // variables 1 : result = 0
            il.DeclareLocal(typeof(int));

            il.Emit(OpCodes.Ldc_I4_0);
            il.Emit(OpCodes.Stloc_1);

            il.MarkLabel(loopStart);
            il.Emit(OpCodes.Ldloc_1);
            il.Emit(OpCodes.Ldc_I4, 10);
            il.Emit(OpCodes.Bge, methodEnd);
            // i*x
            il.Emit(OpCodes.Ldloc_1);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Mul);
            // result +=
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Add);
            il.Emit(OpCodes.Stloc_0);

            il.Emit(OpCodes.Ldloc_1);
            il.Emit(OpCodes.Ldc_I4_1);
            il.Emit(OpCodes.Add);
            il.Emit(OpCodes.Stloc_1);

            il.Emit(OpCodes.Br, loopStart);// end of loop


            il.MarkLabel(methodEnd);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ret);

            var method = (Func<int, int>)calc.CreateDelegate(typeof(Func<int, int>));
            var result = method(1000);
            Console.WriteLine(result);
        }
        static int Calculate(int x)
        {
            int result = 0;
            for (int i = 0; i < 10; i++)
            {
                result += i * x;
            }

            return result;
        }
        static int Calculate(int first,int second,int third)
        {
            var result = first * second;

            return result - third;
        }
    }
}
