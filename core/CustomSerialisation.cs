using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace azloot.core
{
    public static class CustomSerialisation
    {
        //TODO put me in the dataconfig class as ToJson()
        public static string Serialise(Configuration config)
        {
            var jsonText = JsonSerializer.Serialize(config.ToDatapack());
            return jsonText;
        }

        public static Configuration Deserialise(string jsonText)
        {
            ConfigurationDatapack pack = JsonSerializer.Deserialize<ConfigurationDatapack>(jsonText);

            throw new NotImplementedException();
        }

        private static void ReadConfig(ref Utf8JsonReader reader, Configuration config)
        {

            throw new NotImplementedException();
        }

        private static JsonWriterOptions CreateDefaultWriterOptions()
        {
            var options = new JsonWriterOptions();
            // default to a pretty-print style
            options.Indented = true;
            return options;
        }

        #region serialisation methods
        /*
        private static void WriteConfig(ref Utf8JsonWriter writer, Configuration value)
        {
            writer.WriteStartObject();
            WriteKnownItems(ref writer, Item.GetCachedItems());
            WriteRanks(ref writer, value.Ranks.Values);
            WritePersons(ref writer, value.Persons.Values);
            WritePointsLists(ref writer, value.PointsLists.Values);
            WritePointsHistory(ref writer, value.PointsHistory.PointsEvents.Values);
            WriteLootHistory(ref writer, value.LootHistory);
            writer.WriteEndObject();
        }

        private static void WritePointsHistory(ref Utf8JsonWriter writer, IEnumerable<PointsEvent> values)
        {
            writer.WriteStartArray("pointshistory");
            foreach (var value in values) WritePointsEvent(ref writer, value);
            writer.WriteEndArray();
        }

        private static void WritePointsEvent(ref Utf8JsonWriter writer, PointsEvent value)
        {
            writer.WriteStartObject();
            writer.WriteString("id",value.Id);
            writer.WriteString("pointslistid",value.PointsList.Id);
            writer.WriteString("reason", value.Reason);
            writer.WriteNumber("timestamp", value.Timestamp);
            writer.WriteNumber("value", value.Value);
            writer.WriteStartArray("recipients");
            foreach (var person in value.Recipients) writer.WriteStringValue(person.Id);
            writer.WriteEndArray();
            writer.WriteEndObject();
        }

        private static void WritePointsLists(ref Utf8JsonWriter writer, IEnumerable<PointsList> values)
        {
            writer.WriteStartArray("pointslists");
            foreach (var value in values) WritePointsList(ref writer, value);
            writer.WriteEndArray();
        }

        private static void WritePointsList(ref Utf8JsonWriter writer, PointsList value)
        {
            writer.WriteStartObject();
            writer.WriteString("id", value.Id);
            writer.WriteString("name", value.Name);
            writer.WriteNumber("tier", value.Tier);
            writer.WriteStartArray("entries");
            foreach (var entry in value.Points) WritePointsEntry(ref writer, entry.Key, entry.Value);
            writer.WriteEndArray();
            writer.WriteEndObject();
        }

        private static void WritePointsEntry(ref Utf8JsonWriter writer, Guid personId, float points)
        {
            writer.WriteStartObject();
            writer.WriteString("personid", personId);
            writer.WriteNumber("points", points);
            writer.WriteEndObject();
        }

        private static void WritePersons(ref Utf8JsonWriter writer, IEnumerable<Person> values)
        {
            writer.WriteStartArray("persons");
            foreach (var value in values) WritePerson(ref writer, value);
            writer.WriteEndArray();
        }

        private static void WritePerson(ref Utf8JsonWriter writer, Person value)
        {
            writer.WriteStartObject();
            writer.WriteString("id", value.Id);
            writer.WriteString("name", value.Name);
            writer.WriteString("rankid", value.Rank.Id);
            writer.WriteEndObject();
        }

        private static void WriteRanks(ref Utf8JsonWriter writer, IEnumerable<Rank> values)
        {
            writer.WriteStartArray("ranks");
            foreach (var value in values) WriteRank(ref writer, value);
            writer.WriteEndArray();
        }

        private static void WriteRank(ref Utf8JsonWriter writer, Rank value)
        {
            writer.WriteStartObject();
            writer.WriteString("id", value.Id);
            writer.WriteString("name", value.Name);
            writer.WriteNumber("tier", value.Tier);
            writer.WriteEndObject();
        }

        private static void WriteKnownItems(ref Utf8JsonWriter writer, IEnumerable<Item> values)
        {
            writer.WriteStartArray("knownitems");
            foreach (var value in values) WriteKnownItem(writer, value);
            writer.WriteEndArray();
        }

        private static void WriteKnownItem(Utf8JsonWriter writer, Item value)
        {
            writer.WriteStartObject();
            writer.WriteNumber("id", value.Id);
            writer.WriteString("name", value.Name);
            writer.WriteEndObject();
        }

        private static void WriteLootHistory(ref Utf8JsonWriter writer, LootHistory value)
        {
            writer.WriteStartArray("loothistory");
            foreach (var lootEvent in value.Events.Values)
            {
                WriteLootEvent(ref writer, lootEvent);
            }
            writer.WriteEndArray();
        }

        private static void WriteLootEvent(ref Utf8JsonWriter writer, LootEvent value)
        {
            writer.WriteStartObject();
            writer.WriteString("id", value.Id);
            writer.WriteNumber("itemid", value.Item.Id);
            writer.WriteString("personid", value.Person.Id);
            writer.WriteString("priolistid", value.PointsList.Id);
            writer.WriteNumber("timestamp", value.Timestamp);
            writer.WriteEndObject();
        }
        */
        #endregion
        

        #region deserialisation methods
        // do stuff
        #endregion
    }
}
