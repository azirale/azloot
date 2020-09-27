using System;
using System.Collections.Generic;

namespace azloot.core
{
    public class RankSet : Dictionary<Guid,Rank>
    {
        public RankSetDatapack ToDatapack()
        {
            var ranksList = new List<RankDatapack>();
            foreach (var rank in this.Values) ranksList.Add(rank.ToDatapack());
            return new RankSetDatapack() { entries = ranksList };
        }

        public RankSet() { }

        public RankSet(RankSetDatapack datapack)
        {
            foreach (var rankData in datapack.entries)
            {
                var newRank = new Rank(rankData);
                this[newRank.Id] = newRank;
            }
        }

        public void Add(Rank rank)
        {
            this[rank.Id] = rank;
        }
    }

    public class RankSetDatapack
    {
        public List<RankDatapack> entries { get; set; }
    }
}
