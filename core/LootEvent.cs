using System;

namespace azloot.core
{
    /// <summary>
    /// An individual loot distribution event. A person receives an item based on a priolist at a given time.
    /// </summary>
    public class LootEvent
    {
        public Guid Id { get; set; }
        public Item Item { get; set; }
        public Person Person { get; set; }
        public PointsList PointsList { get; set; }
        public long Timestamp { get; set; }

        public LootEvent() { }

        public LootEvent(LootEventDatapack datapack,Configuration config)
        {
            this.Id = datapack.id;
            this.Item = config.Items[datapack.itemid];
            this.Person = config.Persons[datapack.personid];
            this.PointsList = config.PointsLists[datapack.pointslistid];
            this.Timestamp = datapack.timestamp;
        }

        public LootEventDatapack ToDatapack()
        {
            return new LootEventDatapack()
            {
                id = this.Id,
                itemid = this.Item.Id,
                personid = this.Person.Id,
                pointslistid = this.PointsList.Id,
                timestamp = this.Timestamp
            };
        }
    }

    /// <summary>
    /// No-reference data pack for loot event
    /// </summary>
    public class LootEventDatapack
    {
        public Guid id { get; set; }
        public int itemid { get; set; }
        public Guid personid { get; set; }
        public Guid pointslistid { get; set; }
        public long timestamp { get; set; }
    }
}
