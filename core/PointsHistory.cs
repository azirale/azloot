using System;
using System.Collections.Generic;

namespace azloot.core
{
    /// <summary>
    /// The history of points on a pointslist. Tracks changes to the points done over time.
    /// </summary>
    public class PointsHistory : Dictionary<Guid,PointsEvent>
    {
        public PointsHistoryDatapack ToDatapack()
        {
            var eventsPack = new List<PointsEventDatapack>(this.Count);
            foreach (var pointEvent in this.Values) eventsPack.Add(pointEvent.ToDatapack());
            return new PointsHistoryDatapack() { entries = eventsPack };
        }

        public PointsHistory() { }

        public PointsHistory(PointsHistoryDatapack datapack, Configuration config)
        {
            foreach (var pointsEvent in datapack.entries)
            {
                var newEvent = new PointsEvent(pointsEvent, config);
                this.Add(newEvent);
            }
        }

        public void Add(PointsEvent pointsEvent)
        {
            this[pointsEvent.Id] = pointsEvent;
        }
    }

    public class PointsHistoryDatapack
    {
        public List<PointsEventDatapack> entries { get; set; }
    }
}
