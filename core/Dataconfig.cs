using System;
using System.Collections.Generic;

namespace azloot.core
{
    /// <summary>
    /// Parent data object for an overall set of data.
    /// </summary>
    public class Dataconfig
    {
        public readonly Dictionary<Guid, PointsList> PointsLists = new Dictionary<Guid, PointsList>();
        public readonly Dictionary<Guid, Person> Persons = new Dictionary<Guid, Person>();
        public readonly Dictionary<Guid, Rank> Ranks = new Dictionary<Guid, Rank>();
        public readonly LootHistory LootHistory = new LootHistory();
        public readonly PointsHistory PointsHistory = new PointsHistory();
    }
}
