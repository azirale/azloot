using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;

namespace azloot.core
{
    /// <summary>
    /// Contains a history of loot that has been handed out.
    /// </summary>
    public class LootHistory : Dictionary<Guid,LootEvent>
    {
        public LootEvent GetLastLootForPerson(Person person)
        {
            LootEvent latestEvent = null;
            foreach (var currentEvent in this.Values)
            {
                if (currentEvent.Person != person) continue;
                if (latestEvent == null) latestEvent = currentEvent;
                else if (latestEvent.Timestamp > currentEvent.Timestamp) latestEvent = currentEvent;
            }
            return latestEvent;
        }

        public LootHistoryDatapack ToDatapack()
        {
            var lootdata = new List<LootEventDatapack>();
            foreach (var lootevent in this.Values) lootdata.Add(lootevent.ToDatapack());
            return new LootHistoryDatapack() { entries = lootdata };
        }

        public LootHistory() { }

        public LootHistory(LootHistoryDatapack datapack, Configuration config)
        {
            foreach (var lootEvent in datapack.entries)
            {
                var newLootEvent = new LootEvent(lootEvent, config);
                this.Add(newLootEvent);
            }
        }

        public void Add(LootEvent lootEvent)
        {
            this[lootEvent.Id] = lootEvent;
        }
    }

    public class LootHistoryDatapack
    {
        public List<LootEventDatapack> entries { get; set; }
    }
}
