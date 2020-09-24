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
    }
}
