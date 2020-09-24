namespace azloot.core
{
    /// <summary>
    /// When distributing out loot this tracks an individual bid.
    /// </summary>
    public class Bid
    {
        public Person Person { get; set; }
        public Item Item { get; set; }
        public PointsList PrioList { get; set; }
        public int PersonalPriority { get; set; }
    }
}
