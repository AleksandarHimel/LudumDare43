using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class GameFileConfig
{
    private static GameFileConfig _instance;
    public Assets.Scripts.Configuration.ShipConfig ShipConfig;

    public static GameFileConfig GetInstance()
    {
        if (_instance == null)
        {
            _instance = new GameFileConfig();
        }

        return _instance;
    }

    private GameFileConfig()
    {
        TextAsset configXML = Resources.Load("ShipConfig") as TextAsset;

        StringReader reader = new StringReader(configXML.text);
        XmlSerializer xs = new XmlSerializer(typeof(Assets.Scripts.Configuration.ShipConfig));
        ShipConfig = xs.Deserialize(reader) as Assets.Scripts.Configuration.ShipConfig;
    }
}

