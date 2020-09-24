using System;

namespace azloot.devcheck
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Trying demo data serialisation");
            var printme = JsonStuff.GetDemoSerialisedJsonText();
            Console.Write(printme);
            Console.ReadLine();
        }
    }
}
