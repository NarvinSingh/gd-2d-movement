using static Com.NarvinSingh.Test.PrivateAccess;

namespace Com.NarvinSingh.UnitTest
{
    public static class PrivateAccessTest
    {
        public static bool ItGets()
        {
            TestClass testClass = new TestClass();

            if ((string)Get("constField", testClass) != "private const field") return false;
            if ((string)Get("staticField", testClass) != "private static field") return false;
            if ((string)Get("field", testClass) != "private field") return false;
            if ((string)Get("StaticProp", testClass) != "private static property") return false;
            if ((string)Get("Prop", testClass) != "private property") return false;
            return true;
        }

        public static bool ItSets()
        {
            TestClass testClass = new TestClass();

            Set("staticField", testClass, "new value 1");
            if ((string)Get("staticField", testClass) != "new value 1") return false;
            
            Set("field", testClass, "new value 2");
            if ((string)Get("field", testClass) != "new value 2") return false;
            
            Set("StaticProp", testClass, "new value 3");
            if ((string)Get("StaticProp", testClass) != "new value 3") return false;
            
            Set("Prop", testClass, "new value 4");
            if ((string)Get("Prop", testClass) != "new value 4") return false;
            
            return true;
        }

        public static bool ItCalls()
        {
            TestClass testClass = new TestClass();

            if ((string)Call("StaticMeth", testClass) != "private static method") return false;
            if ((string)Call("StaticMeth", testClass, new string[] { "1" }) != "private static method 1") return false;
            if ((string)Call("Meth", testClass) != "private method") return false;
            if ((string)Call("Meth", testClass, new string[] { "1" }) != "private method 1") return false;
            return true;
        }

        private class TestClass
        {
            private const string constField = "private const field";
            
            private static readonly string staticField = "private static field";

            private readonly string field = "private field";

            public TestClass()
            {
                // Invoke all of our fields, properties and methods to get rid of warnings about unused members
                if (constField == field && staticField == field && StaticProp == Prop)
                {
                    StaticMeth("");
                    Meth("");
                }
            }
            
            private static string StaticProp { get; set; } = "private static property";

            private string Prop { get; set; } = "private property";
            
            private static string StaticMeth()
            {
                return "private static method";
            }

            private static string StaticMeth(string arg)
            {
                return StaticMeth() + " " + arg;
            }

            private string Meth()
            {
                return "private method";
            }

            private string Meth(string arg)
            {
                return Meth() + " " + arg;
            }
        }
    }
}
