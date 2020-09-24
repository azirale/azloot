using System;
using System.Collections.Generic;

namespace azloot.core
{
    /// <summary>
    /// Contains a history of loot that has been handed out.
    /// </summary>
    public class LootHistory
    {
        public readonly Dictionary<Guid, LootEvent> Events = new Dictionary<Guid, LootEvent>();

        public LootEvent GetLastLootForPerson(Person person)
        {
            LootEvent latestEvent = null;
            foreach (LootEvent currentEvent in this.Events.Values)
            {
                if (currentEvent.Person != person) continue;
                if (latestEvent == null) latestEvent = currentEvent;
                else if (latestEvent.Timestamp > currentEvent.Timestamp) latestEvent = currentEvent;
            }
            return latestEvent;
        }

        public void AddEvent(LootEvent lootEvent)
        {
            this.Events.Add(lootEvent.Id, lootEvent);
        }
    }
}
