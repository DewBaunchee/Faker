using System;
using System.Collections.Generic;

namespace App.Example
{
    public struct CustomStruct
    {
        public long Long;
        public string AnotherString;

        public override string ToString()
        {
            return GetType().Name + "(" + Long + ", " + AnotherString + ")";
        }
    }

    public class ExampleClass
    {
        public string aString { set; get; }

        public int Int { set; get; }

        public long Long;

        public DateTime aDateTime;

        public List<NestedExample> NestedExamples { set; get; }

        public List<int> Ints;

        public override string ToString()
        {
            return GetType().Name + 
                   "('" + 
                   aString + "', " + 
                   Int + ", " + 
                   aDateTime + ", " +
                   Long + ", List(" + 
                   String.Join(", ", NestedExamples) + "), List(" + 
                   String.Join(", ", Ints) + 
                   "))";
        }
    }

    public class NestedExample
    {
        public ExampleClass ExampleClass { set; get; }

        public bool Bool;

        public char Char;

        public CustomStruct CustomStruct;

        public NestedExample(ExampleClass exampleClass)
        {
            
        }

        public override string ToString()
        {
            return GetType().Name + "(" + Bool + ", '" + Char + "', " + CustomStruct + ")";
        }
    }
}