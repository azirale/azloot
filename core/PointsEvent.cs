using System;
using System.Collections.Generic;

namespace azloot.core
{
    /// <summary>
    /// An individual points changing event for a person (or set of persons) on a given priolist
    /// </summary>
    public class PointsEvent
    {
        public Guid Id { get; set; }
        public PointsList PointsList { get; set; }
        public List<Person> Recipients { get; set; }
        public long Timestamp { get; set; }
        public float Value { get; set; }
        public string Reason { get; set; }
    }
}
