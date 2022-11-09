// Old style

using System;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Code in Main()");
            Test();
        }

        static void Test()
        {
            TestStruct testStruct1 = new TestStruct { Test = 10 };
            TestStruct testStruct2 = testStruct1;

            testStruct1.Test = 2;

            Console.WriteLine(testStruct2.Test);
        }


        struct TestStruct
        {
            public int Test;
        }
    }
}