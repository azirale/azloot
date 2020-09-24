using System;

namespace azloot.core
{
    /// <summary>
    /// Ranks have tiers associated with them, in case you want to prio by rank.
    /// </summary>
    public class Rank
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Tier { get; set; }
    }
}
