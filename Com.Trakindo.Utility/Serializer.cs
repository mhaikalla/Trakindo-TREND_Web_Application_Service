using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace Com.Trakindo.Utility
{
    public class Serializer
    {
        public class XML
        {

            public static string Serialize(object o)
            {
                var serializer = new XmlSerializer(o.GetType());
                using (StringWriter textWriter = new StringWriter())
                {
                    serializer.Serialize(textWriter, o);
                    return textWriter.ToString();
                }
            }

            public static T Deserialize<T>(string xml)
            {
                var serializer = new XmlSerializer(typeof(T));
                using (TextReader textReader = new StringReader(xml))
                {
                    T o = (T)serializer.Deserialize(textReader);
                    return o;
                }
            }
        }

        public class Json
        {

            public static string Serialize(object o)
            {
                //var serializer = new JavaScriptSerializer();
                // return serializer.Serialize(o);
                string ss = Newtonsoft.Json.JsonConvert.SerializeObject(o, Formatting.Indented,
                     new JsonSerializerSettings
                     {
                         ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                     });
                ss = ss.Replace("\u000d\u000a", "");
                return ss;
            }

            public static T Deserialize<T>(string json)
            {
                var serializer = new JavaScriptSerializer();
                return serializer.Deserialize<T>(json);
            }

        }
    }
}
