using Newtonsoft.Json;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace WhiteBot
{

    public class Serializer
    {
        public Serializer()
        {
        }

        public static void SerializeToXML<T>(T objectToSerialize, string filename)
        {
            try
            {
                using (Stream stream = File.Open(filename, FileMode.Create, FileAccess.ReadWrite))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    var writer = new XmlTextWriter(stream, Encoding.Default);
                    writer.Formatting = System.Xml.Formatting.Indented;
                    serializer.Serialize(writer, objectToSerialize);
                    writer.Close();
                }
            }
            catch
            {
                throw;
            }
        }

        public static T DeserializeFromXML<T>(string filename)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                T serializedObject;

                using (Stream stream = File.Open(filename, FileMode.Open, FileAccess.Read))
                {
                    serializedObject = (T)serializer.Deserialize(stream);
                }

                return serializedObject;
            }
            catch
            {
                throw;
            }
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
                XmlSerializer serializer = new XmlSerializer(typeof(T));

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