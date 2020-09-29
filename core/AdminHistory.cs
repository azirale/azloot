using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace azloot.core
{
    public class AdminHistory : Dictionary<Guid,AdminEvent>
    {
        public AdminHistory() { }

        public void Add(AdminEvent adminEvent)
        {
            this[adminEvent.Id] = adminEvent;
        }

        public AdminHistory(AdminHistoryDatapack datapack)
        {
            foreach (var entryPack in datapack.entries)
            {
                var newEvent = new AdminEvent(entryPack);
                this.Add(newEvent);
            }
        }

        public AdminHistoryDatapack ToDatapack()
        {
            List<AdminEventDatapack> newEntries = new List<AdminEventDatapack>(this.Count);
            foreach (var adminEvent in this.Values) newEntries.Add(adminEvent.ToDatapack());
            return new AdminHistoryDatapack()
            {
                entries = newEntries
            };
        }
    }

    public class AdminHistoryDatapack
    {
        public List<AdminEventDatapack> entries { get; set; }
    }
}
