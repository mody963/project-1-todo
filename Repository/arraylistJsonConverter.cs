using System.Text.Json.Serialization;
using System.Text.Json;
public class MyArrayListJsonConverter<T> : JsonConverter<MyArrayList<T>> where T : IEquatable<T>
{
    public override MyArrayList<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException();
        }
        var collection = new MyArrayList<T>();
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
            {
                return collection;
            }
            T item = JsonSerializer.Deserialize<T>(ref reader, options);
            collection.Add(item);
        }
        throw new JsonException();
    }
    public override void Write(Utf8JsonWriter writer, MyArrayList<T> value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        var iterator = value.GetIterator();
        while (iterator.HasNext())
        {
            T item = iterator.Next();
            JsonSerializer.Serialize(writer, item, options);
        }
        writer.WriteEndArray();   

    }
}


// public override Dictionary<TKey, TValue> Read(
//                 ref Utf8JsonReader reader,
//                 Type typeToConvert,
//                 JsonSerializerOptions options)
//             {
//                 if (reader.TokenType != JsonTokenType.StartObject)
//                 {
//                     throw new JsonException();
//                 }

//                 var dictionary = new Dictionary<TKey, TValue>();

//                 while (reader.Read())
//                 {
//                     if (reader.TokenType == JsonTokenType.EndObject)
//                     {
//                         return dictionary;
//                     }

//                     // Get the key.
//                     if (reader.TokenType != JsonTokenType.PropertyName)
//                     {
//                         throw new JsonException();
//                     }

//                     string? propertyName = reader.GetString();

//                     // For performance, parse with ignoreCase:false first.
//                     if (!Enum.TryParse(propertyName, ignoreCase: false, out TKey key) &&
//                         !Enum.TryParse(propertyName, ignoreCase: true, out key))
//                     {
//                         throw new JsonException(
//                             $"Unable to convert \"{propertyName}\" to Enum \"{_keyType}\".");
//                     }

//                     // Get the value.
//                     reader.Read();
//                     TValue value = _valueConverter.Read(ref reader, _valueType, options)!;

//                     // Add to dictionary.
//                     dictionary.Add(key, value);
//                 }

//                 throw new JsonException();
//             }

// using System.Globalization;
// using System.Text.Json;
// using System.Text.Json.Serialization;

// namespace SystemTextJsonSamples
// {
//     public class DateTimeOffsetJsonConverter : JsonConverter<DateTimeOffset>
//     {
//         public override DateTimeOffset Read(
//             ref Utf8JsonReader reader,
//             Type typeToConvert,
//             JsonSerializerOptions options) =>
//                 DateTimeOffset.ParseExact(reader.GetString()!,
//                     "MM/dd/yyyy", CultureInfo.InvariantCulture);

//         public override void Write(
//             Utf8JsonWriter writer,
//             DateTimeOffset dateTimeValue,
//             JsonSerializerOptions options) =>
//                 writer.WriteStringValue(dateTimeValue.ToString(
//                     "MM/dd/yyyy", CultureInfo.InvariantCulture));
//     }
// }