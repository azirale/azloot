using System;
using System.Collections.Generic;

namespace azloot.core
{
    /// <summary>
    /// Parent data object for an overall set of data.
    /// </summary>
    public class Configuration
    {

        public readonly PointsListSet PointsLists = new PointsListSet();
        public readonly PersonSet Persons = new PersonSet();
        public readonly RankSet Ranks = new RankSet();
        public readonly LootHistory LootHistory = new LootHistory();
        public readonly PointsHistory PointsHistory = new PointsHistory();
        public readonly ItemSet Items = new ItemSet();

        public ConfigurationDatapack ToDatapack()
        {
            return new ConfigurationDatapack()
            {
                items = this.Items.ToDatapack(),
                pointslists = this.PointsLists.ToDatapack(),
                persons = this.Persons.ToDatapack(),
                ranks = this.Ranks.ToDatapack(),
                loothistory = this.LootHistory.ToDatapack(),
                pointshistory = this.PointsHistory.ToDatapack()
            };
        }

        /// <summary>
        /// Standard empty configuration is valid.
        /// </summary>
        public Configuration() { }

        /// <summary>
        /// Create a Configuration by unpacking data.
        /// </summary>
        /// <param name="datapack"></param>
        public Configuration(ConfigurationDatapack datapack)
        {
            this.Items = new ItemSet(datapack.items);
            this.Ranks = new RankSet(datapack.ranks);
            this.Persons = new PersonSet(datapack.persons, this);
            this.PointsLists = new PointsListSet(datapack.pointslists, this);
            this.PointsHistory = new PointsHistory(datapack.pointshistory, this);
            this.LootHistory = new LootHistory(datapack.loothistory, this);
        }
    }

    public class ConfigurationDatapack
    {
        public PointsListSetDatapack pointslists { get; set; }
        public PersonSetDatapack persons { get; set; }
        public RankSetDatapack ranks { get; set; }
        public LootHistoryDatapack loothistory { get; set; }
        public PointsHistoryDatapack pointshistory { get; set; }
        public ItemSetDatapack items { get; set; }
    }
}
