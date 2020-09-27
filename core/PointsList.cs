using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace azloot.core
{
    /// <summary>
    /// Object for a list of points tied to Persons
    /// </summary>
    public class PointsList : IDictionary<Person,float>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Tier { get; set; }

        private static readonly Person searchPerson = new Person();
        public float this[Guid personId] {  get { searchPerson.Id = personId; return this[searchPerson]; } }

        public PointsList() { }

        public PointsList(PointsListDatapack datapack, Configuration config)
        {
            this.Id = datapack.id;
            this.Name = datapack.name;
            this.Tier = datapack.tier;
            foreach (var entry in datapack.points)
            {
                var person = config.Persons[entry.personid];
                this[person] = entry.points;
            }
        }

        public PointsListDatapack ToDatapack()
        {
            var entryDatapacks = new List<PointsListEntryDatapack>();
            foreach (var kv in this.Points) entryDatapacks.Add(new PointsListEntryDatapack() { personid = kv.Key.Id, points = kv.Value });
            return new PointsListDatapack()
            {
                id = this.Id,
                name = this.Name,
                tier = this.Tier,
                points = entryDatapacks
            };
        }

        #region IDictionary Implementation
        private readonly Dictionary<Person, float> Points = new Dictionary<Person, float>();
        public ICollection<Person> Keys => ((IDictionary<Person, float>)Points).Keys;

        public ICollection<float> Values => ((IDictionary<Person, float>)Points).Values;

        public int Count => ((ICollection<KeyValuePair<Person, float>>)Points).Count;

        public bool IsReadOnly => ((ICollection<KeyValuePair<Person, float>>)Points).IsReadOnly;

        public float this[Person key] { get => ((IDictionary<Person, float>)Points)[key]; set => ((IDictionary<Person, float>)Points)[key] = value; }

        public void Add(Person key, float value)
        {
            ((IDictionary<Person, float>)Points).Add(key, value);
        }

        public bool ContainsKey(Person key)
        {
            return ((IDictionary<Person, float>)Points).ContainsKey(key);
        }

        public bool Remove(Person key)
        {
            return ((IDictionary<Person, float>)Points).Remove(key);
        }

        public bool TryGetValue(Person key, [MaybeNullWhen(false)] out float value)
        {
            return ((IDictionary<Person, float>)Points).TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<Person, float> item)
        {
            ((ICollection<KeyValuePair<Person, float>>)Points).Add(item);
        }

        public void Clear()
        {
            ((ICollection<KeyValuePair<Person, float>>)Points).Clear();
        }

        public bool Contains(KeyValuePair<Person, float> item)
        {
            return ((ICollection<KeyValuePair<Person, float>>)Points).Contains(item);
        }

        public void CopyTo(KeyValuePair<Person, float>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<Person, float>>)Points).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<Person, float> item)
        {
            return ((ICollection<KeyValuePair<Person, float>>)Points).Remove(item);
        }

        public IEnumerator<KeyValuePair<Person, float>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<Person, float>>)Points).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Points).GetEnumerator();
        }
        #endregion
    }

    /// <summary>
    /// No-reference data pack for PointsList
    /// </summary>
    public class PointsListDatapack
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public int tier { get; set; }
        public List<PointsListEntryDatapack> points { get; set; }
    }

    /// <summary>
    /// No-reference data pack for a PointsListEntry
    /// </summary>
    public class PointsListEntryDatapack
    {
        public Guid personid { get; set; }
        public float points { get; set; }
    }
}
