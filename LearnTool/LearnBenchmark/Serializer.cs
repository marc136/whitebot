using Newtonsoft.Json;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;


namespace LearnBenchmark
{
    public class Serializer
    {
        public Serializer()
        {
        }

        public static void Serialize<T>(T objectToSerialize, string filename)
        {
            try
            {
                var json = JsonConvert.SerializeObject(objectToSerialize, Newtonsoft.Json.Formatting.Indented);

                using (Stream stream = File.Open(filename, FileMode.Create, FileAccess.ReadWrite))
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.Write(json);
                        writer.Flush();
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public static T Deserialize<T>(string filename)
        {
            try
            {
                var text = File.ReadAllText(filename);
                T serializedObject = JsonConvert.DeserializeObject<T>(text);

                return serializedObject;
            }
            catch
            {
                throw;
            }
        }
    }
}