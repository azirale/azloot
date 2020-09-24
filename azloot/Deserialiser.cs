using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using azloot.serialisation;

namespace azloot
{
    class Deserialiser
    {
        public Dataconfig FromJson(string jsontext)
        {
            var jsondoc = JsonDocument.Parse(jsontext);
        }
    }

    static class Serialiser
    {
        public static void BeginSerialisation(Dataconfig config)
        {
            // reorganise all the classes into dicts
            var configDict = new Dictionary<string, object>();
            var history = config.LootHistory;
            
        }
    }

    
    class Serialisation
    {

        public void Serialise(Dataconfig config)
        {
            // set options and register the converters
            var options = CreateOptions();



        }

        private JsonSerializerOptions CreateOptions()
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.Converters.Add(new LootHistoryConverter());
            return options;
        }

 
    }

    
}

namespace azloot.serialisation
{
    class LootHistoryConverter : JsonConverter<LootHistory>
    {
        public override LootHistory Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return null;
        }

        public override void Write(Utf8JsonWriter writer, LootHistory value, JsonSerializerOptions options)
        {
            writer.WriteStartArray("loothistory");
            foreach (var lootEvent in value.Events)
            {
                JsonSerializer.Serialize(writer, lootEvent, typeof(LootEvent), options);
            }
            writer.WriteEndArray();
        }
    }

    class LootEventConverter : JsonConverter<LootEvent>
    {
        public override LootEvent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return null;
        }

        public override void Write(Utf8JsonWriter writer, LootEvent value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("id", value.Id);
            writer.WriteNumber("itemid", value.Item.ItemId);
            writer.WriteString("personid", value.Person.Id);
            writer.WriteString("priolistid", value.PrioList.Id);
            writer.WriteNumber("timestamp", value.Timestamp);
            writer.WriteEndObject();
        }
    }
}
