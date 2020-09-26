using System;
using System.Collections.Generic;

namespace azloot.core
{
    public class PointsListSet : Dictionary<Guid,PointsList>
    {
        public PointsListSetDatapack ToDatapack()
        {
            var pointsLists = new List<PointsListDatapack>(this.Count);
            foreach (var pointsList in this.Values) pointsLists.Add(pointsList.ToDatapack());
            return new PointsListSetDatapack() { entries = pointsLists };
        }

        public PointsListSet() { }

        public PointsListSet(PointsListSetDatapack datapack, Configuration config)
        {
            foreach (var pointsList in datapack.entries)
            {
                var newPointsList = new PointsList(pointsList, config);
                this.Add(newPointsList);
            }
        }

        public void Add(PointsList pointsList)
        {
            this[pointsList.Id] = pointsList;
        }
    }

    public class PointsListSetDatapack
    {
        public List<PointsListDatapack> entries { get; set; }
    }
}