using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Assets.Scripts.Configuration
{
    [Serializable()]
    public class CrewMemberAttributeConfig
    {
        [XmlAttribute(AttributeName ="name")]
        public string Name;

        [XmlAttribute(AttributeName ="value")]
        public float Value;
    }

    [Serializable()]
    public class CrewMemberConfig
    {
        [XmlArray(ElementName ="Attributes")]
        [XmlArrayItem("CrewMemberAttribute")]
        public CrewMemberAttributeConfig[] AttributesArray;

        [XmlElement("InitShipPart")]
        public string InitShipPart;

        [XmlAttribute(AttributeName ="name")]
        public string PirateName;

        [XmlAttribute(AttributeName ="color")]
        public string Color;

        public Dictionary<string, CrewMemberAttributeConfig> GetAttributes()
        {
            
                var result = new Dictionary<string, CrewMemberAttributeConfig>();
                foreach (var attr in AttributesArray)
                {
                    result.Add(attr.Name, attr);
                }

                return result;
            
        }
    }

    [Serializable()]
    [XmlRoot("Ship")]
    public class ShipConfig
    {
        [XmlArray(ElementName = "ShipCrew")]
        [XmlArrayItem("CrewMember")]
        public CrewMemberConfig[] ShipCrew;

        public Dictionary<string, CrewMemberConfig> GetCrewMembers()
        {
            
                var result = new Dictionary<string, CrewMemberConfig>();
                foreach (var crew in ShipCrew)
                {
                    result.Add(crew.PirateName, crew);
                }

                return result;
            
        }
    }
}
