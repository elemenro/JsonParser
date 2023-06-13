using System;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace JsonParser.Parser
{
    public static class AttributeFirstJsonTransformer
    {
        
        public static Stream Transform(Stream source)
        {
            var text = new StreamReader(source).ReadToEnd();
            
            var jsonObject = JToken.Parse(text);

            ReorderProperties(jsonObject);
            
            source = new MemoryStream(Encoding.ASCII.GetBytes(jsonObject.ToString()));
            
            return source;
        }

       public static void ReorderProperties(JToken token)
        {
            if (token is JObject obj)
            {
                var properties = obj.Properties().ToArray();

                Array.Sort(properties, (p1, p2) =>
                {
                    bool IsPrimitive(JToken t) =>
                        t.Type == JTokenType.String ||
                        t.Type == JTokenType.Integer ||
                        t.Type == JTokenType.Float ||
                        t.Type == JTokenType.Boolean ||
                        t.Type == JTokenType.Null;

                    bool IsPrimitiveProperty(JProperty p) => IsPrimitive(p.Value);

                    if (!IsPrimitiveProperty(p1) && IsPrimitiveProperty(p2))
                        return 1;
                    if (IsPrimitiveProperty(p1) && !IsPrimitiveProperty(p2))
                        return -1;
                    return string.CompareOrdinal(p1.Name, p2.Name);
                });

                obj.RemoveAll();

                foreach (var property in properties)
                {
                    obj.Add(property);
                    ReorderProperties(property.Value);
                }
            }
            else if (token is JArray array)
            {
                foreach (var item in array)
                {
                    ReorderProperties(item);
                }
            }
        }
    }
}
