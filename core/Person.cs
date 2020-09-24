using System;

namespace azloot.core
{
    /// <summary>
    /// Represents one person (character) in the data.
    /// </summary>
    public class Person
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Rank Rank { get; set; }
    }
}
