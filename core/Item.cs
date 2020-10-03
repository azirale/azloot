using System;
using System.Xml;

namespace azloot.core
{
    /// <summary>
    /// An Item represents a known item in the data. Auto-fetches data from wowhead xml based on itemid.
    /// </summary>
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Item() { this.Id = 0; }

        public Item(int itemId)
        {
            this.Id = itemId;
            this.Name = "UNKNOWN";
        }

        public Item(int id, string Name)
        {
            this.Id = id;
            this.Name = Name;
        }

        public Item(ItemDatapack datapack)
        {
            this.Id = datapack.id;
            this.Name = datapack.name;
        }

        public ItemDatapack ToDatapack()
        {
            return new ItemDatapack()
            {
                id = this.Id,
                name = this.Name
            };
        }

        public void FetchNameFromWowhead()
        {
            var uri = String.Format("https://www.wowhead.com/item={0}&xml", this.Id);
            using (var xmlreader = XmlTextReader.Create(uri))
            {
                while (xmlreader.Read())
                {
                    if (xmlreader.NodeType == XmlNodeType.Element && xmlreader.Name == "Name")
                    {
                        this.Name = xmlreader.Value;
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Dereferenced data packing object for Item
    /// </summary>
    public class ItemDatapack
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}
