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

        public Rank() { }

        public Rank(RankDatapack datapack)
        {
            this.Id = datapack.id;
            this.Name = datapack.name;
            this.Tier = datapack.tier;
        }

        public RankDatapack ToDatapack()
        {
            return new RankDatapack()
            {
                id = this.Id,
                name = this.Name,
                tier = this.Tier
            };
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    /// <summary>
    /// No-reference data pack for Rank
    /// </summary>
    public class RankDatapack
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public int tier { get; set; }

    }
}
