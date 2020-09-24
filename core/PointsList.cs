using System;
using System.Collections.Generic;

namespace azloot.core
{
    /// <summary>
    /// Object for a list of points tied to Persons
    /// </summary>
    public class PointsList
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Tier { get; set; }
        public readonly Dictionary<Guid, float> Points = new Dictionary<Guid, float>();

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
}
