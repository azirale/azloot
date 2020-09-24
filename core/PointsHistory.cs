using System;
using System.Collections.Generic;

namespace azloot.core
{
    /// <summary>
    /// The history of points on a pointslist. Tracks changes to the points done over time.
    /// </summary>
    public class PointsHistory
    {
        public readonly Dictionary<Guid, PointsEvent> PointsEvents = new Dictionary<Guid, PointsEvent>();

        public void AddEvent(PointsEvent newEvent)
        {
            if (PointsEvents.ContainsKey(newEvent.Id)) return;
            PointsEvents.Add(newEvent.Id, newEvent);
        }
    }
}
