using System;
using App.Example;
using App.Strategy;
using Lib;
using Lib.Config;

namespace App
{
    public static class Program
    {
        public static void Main()
        {
            FakerConfig fakerConfig = new FakerConfig();
            fakerConfig.Add<ExampleClass, string, CustomStrategy>(example => example.aString);
            Faker faker = new Faker(fakerConfig);
            for (int i = 0; i < 10; i++)
                Console.WriteLine("--------------------------------------------------\n" +
                                  LittleFormatter.Format(faker.Create<ExampleClass>().ToString())
                );
        }
    }
}