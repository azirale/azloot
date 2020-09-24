using System;
using System.Collections.Generic;
using azloot.core;

namespace azloot.devcheck
{
    static class JsonStuff
    {
        private static Dataconfig CreateDemoDataConfig()
        {
            Dataconfig config = new Dataconfig();
            // items have no dependencies
            var item1 = Item.CreateKnownItem(19351, "Maladath, Runed Blade of the Black Flight");
            var item2 = Item.CreateKnownItem(21520, "Ravencrest's Legacy");
            // ranks have no dependencies
            var rank1 = new Rank() { Id = Guid.NewGuid(), Name = "TestRank1", Tier = 1 };
            var rank2 = new Rank() { Id = Guid.NewGuid(), Name = "TestRank2", Tier = 2 };
            config.Ranks.Add(rank1.Id, rank1);
            config.Ranks.Add(rank2.Id, rank2);
            // persons only depend on ranks
            var person1 = new Person() { Id = Guid.NewGuid(), Name = "DangerousDave", Rank = rank1 };
            var person2 = new Person() { Id = Guid.NewGuid(), Name = "SalaciousSally", Rank = rank2 };
            config.Persons.Add(person1.Id, person1);
            config.Persons.Add(person2.Id, person2);
            // pointslists contain persons
            var pointslist1 = new PointsList() { Id = Guid.NewGuid(), Name = "Primary", Tier = 1 };
            pointslist1.Points.Add(person1.Id, 17.4f);
            pointslist1.Points.Add(person2.Id, 21.3f);
            var pointslist2 = new PointsList() { Id = Guid.NewGuid(), Name = "Secondary", Tier = 2 };
            pointslist2.Points.Add(person1.Id, 21.3f);
            pointslist2.Points.Add(person2.Id, 33.4f);
            config.PointsLists.Add(pointslist1.Id, pointslist1);
            config.PointsLists.Add(pointslist2.Id, pointslist2);
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
            config.PointsHistory.AddEvent(pointsevent1);
            config.PointsHistory.AddEvent(pointsevent2);
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
            config.LootHistory.AddEvent(loot1);
            config.LootHistory.AddEvent(loot2);
            // done -- we can give it back
            return config;
        }
        public static string GetDemoSerialisedJsonText()
        {
            var config = CreateDemoDataConfig();
            // data is prepped -- now serialise it
            var jsonText = CustomSerialisation.Serialise(config);
            return jsonText;
        }
    }
}
