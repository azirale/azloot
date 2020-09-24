using System;
using System.Collections.Generic;
using System.Xml;

namespace azloot.core
{
    /// <summary>
    /// An Item represents a known item in the data. Auto-fetches data from wowhead xml based on itemid.
    /// </summary>
    public class Item
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
            knownItems[itemId] = newItem;
            return newItem;
        }

        public static Item CreateKnownItem(int id, string name)
        {
            if (knownItems.ContainsKey(id)) return knownItems[id];
            var newItem = new Item() { ItemId = id, Name = name };
            knownItems[id] = newItem;
            return newItem;
        }

        public static IEnumerable<Item> GetKnownItems()
        {
            foreach (var item in knownItems.Values)
            {
                yield return item;
            }
        }
    }
}
