using System;

namespace azloot.core
{
    public class AdminEvent
    {
        public Guid Id { get; set; }
        public string Action { get; set; }
        public long Timestamp { get; set; }

        #region constructors
        public AdminEvent() { }

        public AdminEvent(string action)
        {
            this.Id = Guid.NewGuid();
            this.Action = action;
            this.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        public AdminEvent(AdminEventDatapack datapack)
        {
            this.Id = datapack.id;
            this.Action = datapack.action;
            this.Timestamp = datapack.timestamp;
        }
        #endregion

        public AdminEventDatapack ToDatapack()
        {
            return new AdminEventDatapack()
            {
                id = this.Id,
                action = this.Action,
                timestamp = this.Timestamp
            };
        }
    }

    public class AdminEventDatapack
    {
        public Guid id { get; set; }
        public string action { get; set; }
        public long timestamp { get; set; }
    }
}
