using System;
using System.Collections.Generic;
using System.Text.Json;
using azloot.core;

namespace azloot.devcheck
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Checking round trip serialisation of datapack...");
            var result1 = SerialisationChecker.DatapackRoundTripCheck();
            Console.WriteLine("Success state: {0}", result1);
            Console.WriteLine("Checking serialisation roundtrip...");
            var result2 = SerialisationChecker.ConfigRoundTripCheck();
            Console.WriteLine("Success State: {0}", result2);



            //Console.WriteLine("Trying demo data serialisation");
            //var printme = JsonStuff.GetDemoSerialisedJsonText();
            //Console.Write(printme);
            //JsonStuff.CheckDemoDataconfigRoundTripSerialisation();
            Console.ReadLine();
        }
    }
}
