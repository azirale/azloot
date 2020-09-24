using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace azloot
{
    /// <summary>
    /// Parent data object for an overall set of data.
    /// </summary>
    class Dataconfig
    {
        public readonly Dictionary<Guid, PointsList> PointsLists = new Dictionary<Guid, PointsList>();
        public readonly Dictionary<Guid, Person> Persons = new Dictionary<Guid, Person>();
        public readonly Dictionary<Guid, Rank> Ranks = new Dictionary<Guid, Rank>();
        public readonly LootHistory LootHistory = new LootHistory();
        public readonly PointsHistory PointsHistory = new PointsHistory();
    }

    /// <summary>
    /// Represents one person (character) in the data.
    /// </summary>
    class Person
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Rank Rank { get; set; }
    }

    /// <summary>
    /// Ranks have tiers associated with them, in case you want to prio by rank.
    /// </summary>
    class Rank
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Tier { get; set; }
    }

    /// <summary>
    /// Object for a list of points tied to Persons
    /// </summary>
    class PointsList
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Tier { get; set; }
        public Dictionary<Guid,float> Points { get; set; }

        public void AddPerson(Person person, float initialPoints)
        {
            if (Points.ContainsKey(person.Id)) return;
            Points.Add(person.Id, initialPoints);
        }

        public void RemovePerson(Person person)
        {
            if (!Points.ContainsKey(person.Id)) return;
            Points.Remove(person.Id);
        }

        public float GetPointsOfPerson(Person person)
        {
            return Points[person.Id];
        }

        public void AddPointsToPerson(Person person, float addPoints)
        {
            Points[person.Id] = Points[person.Id] + addPoints;
        }

        public void SetPointsOfPerson(Person person, float setPoints)
        {
            Points[person.Id] = setPoints;
        }
    }

    /// <summary>
    /// An Item represents a known item in the data. Auto-fetches data from wowhead xml based on itemid.
    /// </summary>
    class Item
    {
        private readonly static Dictionary<int, Item> knownItems = new Dictionary<int, Item>();
        public int ItemId { get; set; }
        public string Name { get; set; }

        public static Item FromId(int itemId)
        {
            if (knownItems.ContainsKey(itemId)) return knownItems[itemId];
            var newItem = new Item() { ItemId = itemId, Name = "UNKNOWN" };
            var uri = String.Format("https://www.wowhead.com/item={0}&xml", itemId);
            var xmlreader = XmlTextReader.Create(uri);
            while (xmlreader.Read())
            {
                if (xmlreader.NodeType == XmlNodeType.Element && xmlreader.Name == "Name")
                {
                    newItem.Name = xmlreader.Value;
                    break;
                }
            }
            return newItem;
        }
    }

    /// <summary>
    /// Contains a history of loot that has been handed out.
    /// </summary>
    class LootHistory
    {
        public Dictionary<Guid,LootEvent> Events { get; set; }

        public LootEvent GetLastLootForPerson(Person person)
        {
            LootEvent latestEvent = null;
            foreach (LootEvent currentEvent in this.Events.Values)
            {
                if (currentEvent.Person != person) continue;
                if (latestEvent == null) latestEvent = currentEvent;
                else if (latestEvent.Timestamp > currentEvent.Timestamp) latestEvent = currentEvent;
            }
            return latestEvent;
        }

        public void AddEvent(LootEvent lootEvent)
        {
            this.Events.Add(lootEvent.Id, lootEvent);
        }
    }

    /// <summary>
    /// An individual loot distribution event. A person receives an item based on a priolist at a given time.
    /// </summary>
    class LootEvent
    {
        public Guid Id { get; set; }
        public Item Item { get; set; }
        public Person Person { get; set; }
        public PointsList PrioList { get; set; }
        public long Timestamp { get; set; }
    }

    /// <summary>
    /// The history of points on a pointslist. Tracks changes to the points done over time.
    /// </summary>
    class PointsHistory
    {
        public readonly Dictionary<Guid, PointsEvent> PointsEvents = new Dictionary<Guid, PointsEvent>();

        public void AddEvent(PointsEvent newEvent)
        {
            if (PointsEvents.ContainsKey(newEvent.Id)) return;
            PointsEvents.Add(newEvent.Id, newEvent);
        }
    }

    /// <summary>
    /// An individual points changing event for a person (or set of persons) on a given priolist
    /// </summary>
    class PointsEvent
    {
        public Guid Id { get; set; }
        public PointsList PointsList { get; set; }
        public List<Person> Recipients { get; set; }
        public long Timestamp { get; set; }
        public float Value { get; set; }
        public string Reason { get; set; }
    }

    /// <summary>
    /// When distributing out loot this tracks an individual bid.
    /// </summary>
    class Bid
    {
        public Person Person { get; set; }
        public Item Item { get; set; }
        public PointsList PrioList { get; set; }
        public int PersonalPriority { get; set; }
    }

    /// <summary>
    /// When distributing loot this tracks available items and submitted bids.
    /// </summary>
    class LootPool
    {
        public readonly List<Item> Items = new List<Item>();
        public readonly List<Bid> Bids = new List<Bid>();
    }

    /// <summary>
    /// This is the custom part. The loot engine should work with the data structure available to maintain points,
    /// and to distribute loot from a pool.
    /// </summary>
    class LootEngine
    {
        private Dataconfig config { get; set; }

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
                PrioList = pointsList,
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };
            this.config.LootHistory.AddEvent(newLootEvent);
            var currentPoints = pointsList.GetPointsOfPerson(person);
            var newPointsEvent = new PointsEvent()
            {
                Id = new Guid(),
                PointsList = pointsList,
                Recipients = new List<Person>() { person },
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Value = -currentPoints,
                Reason = String.Format("Won item {0} ({1}) on list {2}", item.ItemId, item.Name, pointsList.Name)
            };
            this.config.PointsHistory.AddEvent(newPointsEvent);
            pointsList.SetPointsOfPerson(person, 0);
        }

        public void AddPoints(PointsList pointsList, List<Person> persons, float addValue, string reason)
        {
            foreach (Person person in persons)
            {
                pointsList.AddPointsToPerson(person, addValue);
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
            this.config.PointsHistory.AddEvent(newPointsEvent);
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
            var thisPersonPoints = one.PrioList.GetPointsOfPerson(one.Person);
            var otherPersonPoints = two.PrioList.GetPointsOfPerson(two.Person);
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
            return one.Item.ItemId.CompareTo(two.Item.ItemId);
        }

        public void CreatePointslist(string Name)
        {
            
        }

        /* 
         * 
         */

    }


}
