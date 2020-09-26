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

        public PointsEvent() { }

        public PointsEvent(PointsEventDatapack datapack, Configuration config)
        {
            this.Id = datapack.id;
            this.PointsList = config.PointsLists[datapack.pointslistid];
            this.Timestamp = datapack.timestamp;
            this.Reason = datapack.reason;
            foreach (var personId in datapack.recipientids) this.Recipients.Add(config.Persons[personId]);
        }

        public PointsEventDatapack ToDatapack()
        {
            List<Guid> recipientids = new List<Guid>();
            foreach (var recipient in Recipients) recipientids.Add(recipient.Id);
            return new PointsEventDatapack()
            {
                id = this.Id,
                pointslistid = this.PointsList.Id,
                recipientids = recipientids,
                timestamp = this.Timestamp,
                value = this.Value,
                reason = this.Reason
            };
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }

    /// <summary>
    /// No-reference data pack for PointsEvent
    /// </summary>
    public class PointsEventDatapack
    {
        public Guid id { get; set; }
        public Guid pointslistid { get; set; }
        public List<Guid> recipientids { get; set; }
        public long timestamp { get; set; }
        public float value { get; set; }
        public string reason { get; set; }
    }
}
