using System;

namespace azloot.core
{
    public class AdminEvent
    {
        public Guid Id { get; set; }
        public string ActionCode { get; set; }
        public string ActionDescription { get; set; }
        public long Timestamp { get; set; }

        #region constructors
        public AdminEvent() { }

        public AdminEvent(string actionCode, string actionDescription)
        {
            this.Id = Guid.NewGuid();
            this.ActionCode = ActionCode;
            this.ActionDescription = actionDescription;
            this.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        public AdminEvent(AdminEventDatapack datapack)
        {
            this.Id = datapack.id;
            this.ActionCode = datapack.actioncode;
            this.ActionDescription = datapack.actiondescription;
            this.Timestamp = datapack.timestamp;
        }
        #endregion

        public AdminEventDatapack ToDatapack()
        {
            return new AdminEventDatapack()
            {
                id = this.Id,
                actioncode = this.ActionCode,
                actiondescription = this.ActionDescription,
                timestamp = this.Timestamp
            };
        }
    }

    public class AdminEventDatapack
    {
        public Guid id { get; set; }
        public string actioncode { get; set; }
        public string actiondescription { get; set; }
        public long timestamp { get; set; }
    }
}
