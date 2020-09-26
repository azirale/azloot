using azloot.core;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace azloot.devcheck
{
    static class SerialisationChecker
    {
        public static bool DatapackRoundTripCheck()
        {
            var jsonOptions = new JsonSerializerOptions() { WriteIndented = true };
            var initialConfig = CreateDemoDataConfig();
            var initialDatapack = initialConfig.ToDatapack();
            var initialJson = JsonSerializer.Serialize(initialDatapack, jsonOptions);
            Console.WriteLine(initialJson);
            var returnConfig = JsonSerializer.Deserialize<ConfigurationDatapack>(initialJson, jsonOptions);
            var returnJson = JsonSerializer.Serialize(returnConfig, jsonOptions);
            var isSame = initialJson == returnJson;
            return isSame;
        }

        public static Configuration CreateDemoDataConfig()
        {
            Configuration config = new Configuration();
            // items have no dependencies
            var item1 = new Item(19351, "Maladath, Runed Blade of the Black Flight");
            var item2 = new Item(21520, "Ravencrest's Legacy");
            // ranks have no dependencies
            var rank1 = new Rank() { Id = Guid.NewGuid(), Name = "TestRank1", Tier = 1 };
            var rank2 = new Rank() { Id = Guid.NewGuid(), Name = "TestRank2", Tier = 2 };
            config.Ranks[rank1] = rank1;
            config.Ranks[rank2] = rank2;
            // persons only depend on ranks
            var person1 = new Person() { Id = Guid.NewGuid(), Name = "DangerousDave", Rank = rank1 };
            var person2 = new Person() { Id = Guid.NewGuid(), Name = "SalaciousSally", Rank = rank2 };
            config.Persons.Add(person1);
            config.Persons.Add(person2);
            // pointslists contain persons
            var pointslist1 = new PointsList() { Id = Guid.NewGuid(), Name = "Primary", Tier = 1 };
            pointslist1.Add(person1, 17.4f);
            pointslist1.Add(person2, 21.3f);
            var pointslist2 = new PointsList() { Id = Guid.NewGuid(), Name = "Secondary", Tier = 2 };
            pointslist2.Add(person1, 21.3f);
            pointslist2.Add(person2, 33.4f);
            config.PointsLists.Add(pointslist1);
            config.PointsLists.Add(pointslist2);
            // points events to go into history
            var pointsevent1 = new PointsEvent()
            {
                Id = Guid.NewGuid(),
                Reason = "Giggles",
                PointsList = pointslist1,
                Value = 1.1f,
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Recipients = new List<Person>(new Person[] { person1, person2 })
            };
            var pointsevent2 = new PointsEvent()
            {
                Id = Guid.NewGuid(),
                Reason = "Wibbles",
                PointsList = pointslist2,
                Value = 2.2f,
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Recipients = new List<Person>(new Person[] { person1, person2 })
            };
            config.PointsHistory.Add(pointsevent1);
            config.PointsHistory.Add(pointsevent2);
            // loot events go into history
            var loot1 = new LootEvent()
            {
                Id = Guid.NewGuid(),
                Person = person1,
                PointsList = pointslist1,
                Item = item1,
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };
            var loot2 = new LootEvent()
            {
                Id = Guid.NewGuid(),
                Person = person2,
                PointsList = pointslist2,
                Item = item2,
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };
            config.LootHistory.Add(loot1);
            config.LootHistory.Add(loot2);
            // done -- we can give it back
            return config;
        }
    }
}
