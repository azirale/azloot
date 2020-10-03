namespace azloot.core
{
    /// <summary>
    /// When distributing out loot this tracks an individual bid.
    /// This data is transient -- no need to serialise.
    /// </summary>
    public class Bid
    {
        public Bid() { }

        public Bid(Person person, Item item, PointsList pointsList, int personalPriority)
        {
            this.Person = person;
            this.Item = item;
            this.PointsList = PointsList;
            this.PersonalPriority = personalPriority;
        }

        public Person Person { get; set; }
        public Item Item { get; set; }
        public PointsList PointsList { get; set; }
        public int PersonalPriority { get; set; }
    }
}
