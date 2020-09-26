using System.Collections.Generic;

namespace azloot.core
{
    public class ItemSet : Dictionary<int,Item>
    {

        public ItemSetDatapack ToDatapack()
        {
            var itemEntries = new List<ItemDatapack>(this.Count);
            foreach (var itemEntry in this.Values) itemEntries.Add(itemEntry.ToDatapack());
            return new ItemSetDatapack() { entries = itemEntries };
        }

        public ItemSet() { }

        public ItemSet(ItemSetDatapack datapack)
        {
            foreach (var item in datapack.entries)
            {
                var newItem = new Item(item);
                this.Add(newItem);
            }
        }

        public void Add(Item newItem)
        {
            this[newItem.Id] = newItem;
        }
    }

    public class ItemSetDatapack
    {
        public List<ItemDatapack> entries { get; set; }
    }
}
