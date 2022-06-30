using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomUnitsNormalButtons
{
    public class Vector2Converter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(Vector2);
        public override bool CanRead => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Vector3 typedValue = (Vector2)value;


            serializer.Serialize(writer, new
            {
                typedValue.x,
                typedValue.y,
                typedValue.z

            });
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) =>
            throw new NotImplementedException();
    }
}
