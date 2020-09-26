using System;
using System.Collections.Generic;

namespace azloot.core
{
    /// <summary>
    /// Hashset of persons since a persons should be unique.
    /// Includes extra helper functions to help manage things.
    /// </summary>
    public class PersonSet : Dictionary<Guid,Person>
    {
        /// <summary>
        /// Convert this object to a raw datapack suitable for serialisation.
        /// </summary>
        public PersonSetDatapack ToDatapack()
        {
            var personsList = new List<PersonDatapack>();
            foreach (var person in this.Values) personsList.Add(person.ToDatapack());
            return new PersonSetDatapack() { entries = personsList };
        }

        public PersonSet() { }

        public PersonSet(PersonSetDatapack datapack, Configuration config)
        {
            foreach (var person in datapack.entries)
            {
                var newPerson = new Person(person, config);
                this.Add(newPerson);
            }
        }

        public void Add(Person person)
        {
            this[person.Id] = person;
        }
    }

    /// <summary>
    /// No-reference datapack of Persons
    /// </summary>
    public class PersonSetDatapack
    {
        public List<PersonDatapack> entries { get; set; }
    }
}
