using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace azloot.core
{
    /// <summary>
    /// This is the custom part. The loot engine should work with the data structure available to maintain points,
    /// and to distribute loot from a pool.
    /// </summary>
    public class LootEngine
    {
        private readonly Configuration config;
        private readonly LootPool activePool = new LootPool();

        #region constructors
        public LootEngine()
        {
            this.config = new Configuration();
        }

        public LootEngine(Configuration config)
        {
            this.config = config;
        }

        public LootEngine(ConfigurationDatapack datapack)
        {
            this.config = new Configuration(datapack);
        }
        #endregion

        #region admin tasks
        public void AddPerson(string name, Rank rank, string className, string roleName)
        {
            var newPerson = new Person(name, rank, className, roleName);
            var newAdminEvent = new AdminEvent("newperson", string.Format("Added new person {0} at rank {1} as {2} {3}", name, rank, roleName, className));
            this.config.Persons.Add(newPerson);
            this.config.AdminHistory.Add(newAdminEvent);
        }

        public void AddRank(string name, int tier)
        {
            var newRank = new Rank(name, tier);
            var newAdminEvent = new AdminEvent("newrank", string.Format("Added new rank {0} with tier {1}", name, tier));
            config.Ranks.Add(newRank);
            config.AdminHistory.Add(newAdminEvent);
        }

        public void AddPointsList(string name, int tier)
        {
            var newPointsList = new PointsList(name, tier);
            var newAdminEvent = new AdminEvent("newpointslist", string.Format("Added new points list {0} with tier {1}", name, tier));
            config.PointsLists.Add(newPointsList);
            config.AdminHistory.Add(newAdminEvent);
        }

        public void AddPersonToPointsList(Person person, PointsList pointsList)
        {
            if (pointsList.ContainsKey(person)) return;
            float initialPoints = 0f;
            var newAdminEvent = new AdminEvent("addpersontolist", string.Format("Added person {0} to list {1}", person.Name, pointsList.Name));
            pointsList.Add(person, initialPoints);
        }

        public void RemovePersonFromPointsList(Person person, PointsList pointsList)
        {
            if (!pointsList.ContainsKey(person)) return;
            var newAdminEvent = new AdminEvent("removePersonfromlist", string.Format("Removed person {0} from list {1}", person.Name, pointsList.Name));
            pointsList.Remove(person);
            config.AdminHistory.Add(newAdminEvent);
        }

        public void AddItem(int itemId)
        {
            if (config.Items.ContainsKey(itemId)) return;
            var newItem = new Item(itemId);
            newItem.FetchNameFromWowhead();
            config.Items.Add(new Item());
        }
        #endregion


        #region loot and points tasks
        public void ResetPool()
        {
            activePool.Bids.Clear();
            activePool.Items.Clear();
        }

        public void AddItemToPool(Item item)
        {
            activePool.Items.Add(item);
        }

        public void RemoveItemFromPool(Item item)
        {
            activePool.Items.Remove(item);
        }

        public void AddBidToPool(Person person, Item item, PointsList pointsList, int personalPriority)
        {
            var newBid = new Bid(person, item, pointsList, personalPriority);
            activePool.Bids.Add(newBid);
        }

        public void AddPoints(PointsList pointsList, List<Person> persons, float addValue, string reason)
        {
            foreach (Person person in persons)
            {
                pointsList[person] += addValue;
            }
            var newPointsEvent = new PointsEvent()
            {
                Id = Guid.NewGuid(),
                PointsList = pointsList,
                Recipients = persons,
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Value = addValue,
                Reason = reason
            };
            this.config.PointsHistory.Add(newPointsEvent);
        }

        public void ZeroPoints(PointsList pointsList, Person person, string reason)
        {
            var currentPoints = pointsList[person];
            AddPoints(pointsList, new List<Person>(new Person[] { person }), -currentPoints, reason);
        }

        public void AssignLootFromPool(LootPool pool)
        {
            // nothing to do if there are no bids
            if (pool.Bids.Count == 0) return;
            // sort the bids in-place to see which is highest priority
            SortBids(pool.Bids);
            var winningBid = pool.Bids[0];
            // assign loot based on the winning bid
            AssignLootDirectly(winningBid.Item, winningBid.Person, winningBid.PointsList);
            // this item should no longer be in the pool
            RemoveItemFromPool(winningBid.Item);
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
                Id = Guid.NewGuid(),
                Item = item,
                Person = person,
                PointsList = pointsList,
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };
            this.config.LootHistory.Add(newLootEvent);
            var currentPoints = pointsList[person];
            ZeroPoints(pointsList, person, string.Format("Won item {0} ({1}) on list {2}", item.Id, item.Name, pointsList.Name));
        }

        public void SortBids(List<Bid> allBids)
        {
            // first we need to align personal priorities to be simply ordered regardless of original numbers used
            // group by person
            var personBids = new Dictionary<Guid, List<Bid>>();
            foreach (var bid in allBids)
            {
                if (!personBids.ContainsKey(bid.Person.Id)) personBids[bid.Person.Id] = new List<Bid>();
                personBids[bid.Person.Id].Add(bid);
            }
            // for each person sort their bids by their personal priority, then alter the priority to be 0-indexed
            foreach (var bidList in personBids.Values)
            {
                bidList.Sort((a, b) => { return a.PersonalPriority.CompareTo(b.PersonalPriority); });
                for (int i = 0; i < bidList.Count; i++) bidList[i].PersonalPriority = i;
            }
            // now that personal priority values are aligned we can sort the data
            allBids.Sort(BidComparator);
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
            var priolistTierOrder = one.PointsList.Tier.CompareTo(two.PointsList.Tier);
            if (priolistTierOrder != 0) return priolistTierOrder;
            // if above is equal then go by person rank
            var personrankTierOrder = one.Person.Rank.Tier.CompareTo(two.Person.Rank.Tier);
            if (personrankTierOrder != 0) return personrankTierOrder;
            // if above is all equal then we go by points of the persons
            var thisPersonPoints = one.PointsList[one.Person];
            var otherPersonPoints = two.PointsList[two.Person];
            var personPointsOrder = thisPersonPoints.CompareTo(otherPersonPoints);
            if (personPointsOrder != 0) return personPointsOrder;
            // if points are equal as well then whoever has gone longest without loot
            var thisPersonLastLoot = this.config.LootHistory.GetLastLootForPerson(one.Person)?.Timestamp ?? long.MinValue;
            var otherPersonLastLoot = this.config.LootHistory.GetLastLootForPerson(one.Person)?.Timestamp ?? long.MinValue;
            var personLastLootOrder = thisPersonLastLoot.CompareTo(otherPersonLastLoot);
            if (personLastLootOrder != 0) return personLastLootOrder;
            // if all that is equal, if one person has prioritised an item higher than the other they can get it first
            var personalPriorityOrder = one.PersonalPriority.CompareTo(two.PersonalPriority);
            if (personalPriorityOrder != 0) return personalPriorityOrder;
            // still the same? consistent 'coin flip' it on guid
            var personCoinflip = one.Person.Id.CompareTo(two.Person.Id);
            if (personCoinflip != 0) return personCoinflip;
            // if the same player has submitted multiple bids on equal lists at equal prio coinflip the items
            return one.Item.Id.CompareTo(two.Item.Id);
        }
        #endregion
    }
}
