using System;
using System.Collections.Generic;

namespace azloot.core
{
    /// <summary>
    /// This is the custom part. The loot engine should work with the data structure available to maintain points,
    /// and to distribute loot from a pool.
    /// </summary>
    public class LootEngine
    {
        private Configuration config { get; set; }

        public List<Bid> GetSortedBids(List<Bid> existingBids)
        {
            var sortedBids = new List<Bid>(existingBids);
            sortedBids.Sort(BidComparator);
            return sortedBids;
        }

        public void AssignLootFromPool(LootPool pool)
        {
            var sortedBids = GetSortedBids(pool.Bids);
            if (sortedBids.Count == 0) return; // nothing to do
            var winningBid = sortedBids[0];
            // assign loot based on the winning bid
            AssignLootDirectly(winningBid.Item, winningBid.Person, winningBid.PrioList);
            // this item should no longer be in the pool
            pool.Items.Remove(winningBid.Item);
            // if this item is totally gone from the pool we can remove all bids for it
            if (pool.Items.FindIndex((checkItem) => { return checkItem == winningBid.Item; }) == -1)
            {
                pool.Bids.RemoveAll((bid) => { return bid.Item == winningBid.Item; });
            }
            // otherwise remove ONLY the winning bid
            else
            {
                pool.Bids.Remove(winningBid);
            }
        }

        public void AssignLootDirectly(Item item, Person person, PointsList pointsList)
        {
            var newLootEvent = new LootEvent()
            {
                Id = new Guid(),
                Item = item,
                Person = person,
                PointsList = pointsList,
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };
            this.config.LootHistory.Add(newLootEvent);
            var currentPoints = pointsList[person];
            var newPointsEvent = new PointsEvent()
            {
                Id = new Guid(),
                PointsList = pointsList,
                Recipients = new List<Person>() { person },
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Value = -currentPoints,
                Reason = String.Format("Won item {0} ({1}) on list {2}", item.Id, item.Name, pointsList.Name)
            };
            this.config.PointsHistory.Add(newPointsEvent);
            pointsList[person] = 0;
        }

        public void AddPoints(PointsList pointsList, List<Person> persons, float addValue, string reason)
        {
            foreach (Person person in persons)
            {
                pointsList[person] += addValue;
            }
            var newPointsEvent = new PointsEvent()
            {
                Id = new Guid(),
                PointsList = pointsList,
                Recipients = persons,
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Value = addValue,
                Reason = reason
            };
            this.config.PointsHistory.Add(newPointsEvent);
        }

        /// <summary>
        /// Compare two bids to determine priority order.
        /// </summary>
        /// <param name="one">First bid to compare.</param>
        /// <param name="two">Second bid to compare.</param>
        /// <returns>-1 if first bid has priority, +1 if second bid does. 0 for same priority (should not happen)</returns>
        public int BidComparator(Bid one, Bid two)
        {
            // if the priolist tier indicates an order use that immediately
            var priolistTierOrder = one.PrioList.Tier.CompareTo(two.PrioList.Tier);
            if (priolistTierOrder != 0) return priolistTierOrder;
            // if above is equal then go by person rank
            var personrankTierOrder = one.Person.Rank.Tier.CompareTo(two.Person.Rank.Tier);
            if (personrankTierOrder != 0) return personrankTierOrder;
            // if above is all equal then we go by points of the persons
            var thisPersonPoints = one.PrioList[one.Person];
            var otherPersonPoints = two.PrioList[two.Person];
            var personPointsOrder = thisPersonPoints.CompareTo(otherPersonPoints);
            if (personPointsOrder != 0) return personPointsOrder;
            // if points are equal as well then whoever has gone longest without loot
            var thisPersonLastLoot = this.config.LootHistory.GetLastLootForPerson(one.Person)?.Timestamp ?? long.MinValue;
            var otherPersonLastLoot = this.config.LootHistory.GetLastLootForPerson(one.Person)?.Timestamp ?? long.MinValue;
            var personLastLootOrder = thisPersonLastLoot.CompareTo(otherPersonLastLoot);
            if (personLastLootOrder != 0) return personLastLootOrder;
            // if all that is equal, if on person has prioritised an item higher than the other they can get it first
            var personalPriorityOrder = one.PersonalPriority.CompareTo(two.PersonalPriority);
            if (personalPriorityOrder != 0) return personalPriorityOrder;
            // still the same? consistent 'coin flip' it on guid
            var personCoinflip = one.Person.Id.CompareTo(two.Person.Id);
            if (personCoinflip != 0) return personCoinflip;
            // if the same player has submitted multiple builds on equal lists at equal prio coinflip the items
            return one.Item.Id.CompareTo(two.Item.Id);
        }

        public void CreatePointslist(string Name)
        {

        }

        /* 
         * 
         */

    }
}
