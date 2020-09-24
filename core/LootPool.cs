using System.Collections.Generic;

namespace azloot.core
{
    /// <summary>
    /// When distributing loot this tracks available items and submitted bids.
    /// </summary>
    public class LootPool
    {
        public readonly List<Item> Items = new List<Item>();
        public readonly List<Bid> Bids = new List<Bid>();
    }
}
