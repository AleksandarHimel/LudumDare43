using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class ShipConfig
{
    private static ShipConfig _instance;
    private XmlDocument xmlConfig;

    public static ShipConfig GetInstance()
    {
        if (_instance == null)
        {
            _instance = new ShipConfig();
        }

        return _instance;
    }

    public IEnumerable<CrewMemberAttribute> GetAttributesForCrewMember(string name)
    {
        XmlNodeList itemNodes = xmlConfig.SelectNodes(
            String.Format("//Ship/ShipCrew/CrewMember[@name='{0}']/CrewMemberAttribute", name));
        foreach (XmlNode itemNode in itemNodes)
        {
            string atrName = itemNode.Attributes["name"].Value;
            string atrValue = itemNode.Attributes["value"].Value;
            float atrFloatValue = float.Parse(atrValue);

            yield return CrewMemberAttribute.CreateAttribute(atrName, atrFloatValue);
        }
    }

    public string GetColorForCrewMember(string name)
    {
        XmlNode itemNode = xmlConfig.SelectSingleNode(
            String.Format("//Ship/ShipCrew/CrewMember[@name='{0}']", name));
        return itemNode.Attributes["color"].Value;
    }

    private ShipConfig()
    {
        TextAsset configXML = Resources.Load("ShipConfig") as TextAsset;
        xmlConfig = new XmlDocument();
        xmlConfig.LoadXml(configXML.text);
    }
}

