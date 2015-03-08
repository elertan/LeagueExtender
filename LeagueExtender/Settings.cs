using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LeagueExtender
{
    public class Settings
    {

        public static void Save(string path, Settings settings)
        {
            using (StreamWriter file = new StreamWriter(path))
            {
                XmlSerializer serializer = new XmlSerializer(settings.GetType());
                serializer.Serialize(file, settings);
            }
        }

        public static Settings Load(string path)
        {
            using (StreamReader file = new StreamReader(path))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                return (Settings)serializer.Deserialize(file);
            }
        }

        public string SummonerName;
        public string Region;
    }
}
