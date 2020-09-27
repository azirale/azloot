﻿using System;

namespace azloot.core
{
    /// <summary>
    /// Represents one person (character) in the data.
    /// </summary>
    public class Person
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ClassName { get; set; }
        public string RoleName { get; set; }
        public Rank Rank { get; set; }

        public PersonDatapack ToDatapack()
        {
            return new PersonDatapack()
            {
                id = this.Id,
                name = this.Name,
                classname = this.ClassName,
                rolename = this.RoleName,
                rankid = this.Rank.Id
            };
        }

        public Person() { }

        public Person(PersonDatapack datapack, Configuration config)
        {
            this.Id = datapack.id;
            this.Name = datapack.name;
            this.ClassName = datapack.name;
            this.RoleName = datapack.rolename;
            this.Rank = config.Ranks[datapack.rankid];
        }
    }

    /// <summary>
    /// No reference data pack for Person
    /// </summary>
    public class PersonDatapack
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string rolename { get; set; }
        public string classname { get; set; }
        public Guid rankid { get; set; }
    }
}
