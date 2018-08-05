using System;
using System.Reflection.Emit;

namespace BuildRunTime
{
    class Program
    {
        static void Main(string[] args)
        {
            //Print(5);

            var myMethod = new DynamicMethod("MyMethod", typeof(void), null, typeof(Program).Module);
            var il = myMethod.GetILGenerator();

            il.Emit(OpCodes.Ldc_I4, 5);
            il.Emit(OpCodes.Call, typeof(Program).GetMethod("Print"));
            il.Emit(OpCodes.Ret);

            var delege = (Action)myMethod.CreateDelegate(typeof(Action));
            delege();
        }
        public static void Print(int i)
        {
            Console.WriteLine("i Value : "+i);
        }
    }
    
}
