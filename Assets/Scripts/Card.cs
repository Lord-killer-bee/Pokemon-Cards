using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

using System.Xml;
using System;

[System.Serializable]
public class Card {

    [XmlAttribute("Index")]
    public int m_Index;
    [XmlAttribute("Name")]
    public string m_Name;
    [XmlAttribute("ImageName")]
    public string m_CardSprite;
    [XmlAttribute("HP")]
    public float m_HP;
    [XmlAttribute("Speed")]
    public float m_Speed;
    [XmlAttribute("Attack")]
    public float m_Attack;
    [XmlAttribute("Defense")]
    public float m_Defense;
    [XmlAttribute("Sp.Attack")]
    public float m_SpecialAttack;
    [XmlAttribute("Sp.Defense")]
    public float m_SpecialDefense;
    [XmlAttribute("DamageTable")]
    public string m_DamageTableXML;
    [XmlAttribute("AttackList")]
    public string m_AttackListXML;

    [XmlIgnore]
    public Material m_BackSprite;
    [XmlIgnore]
    public GameObject m_CardBase;
    [XmlIgnore]
    public List<int> m_DamageTable = new List<int>();
    [XmlIgnore]
    public List<AttackInfo> m_AttackList = new List<AttackInfo>();

    public Card() { }

    public Card(int index, Vector3 position)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(Application.dataPath + "/XML files/" + "pokemon.xml");

        XmlNodeList xNodes = doc.SelectNodes("/CardList/Items/Card[@Index = " + "'" + index +"'"+ "]");

        this.m_Index = index;
        this.m_Name = xNodes.Item(0).Attributes[1].InnerText;

        this.m_CardSprite = xNodes.Item(0).Attributes[2].InnerText;

        m_BackSprite = Resources.Load("Materials/back sprite") as Material;

        this.m_HP = int.Parse(xNodes.Item(0).Attributes[3].InnerText);
        this.m_Speed = int.Parse(xNodes.Item(0).Attributes[4].InnerText);
        this.m_Attack = int.Parse(xNodes.Item(0).Attributes[5].InnerText);
        this.m_Defense = int.Parse(xNodes.Item(0).Attributes[6].InnerText);
        this.m_SpecialAttack = int.Parse(xNodes.Item(0).Attributes[7].InnerText);
        this.m_SpecialDefense = int.Parse(xNodes.Item(0).Attributes[8].InnerText);

        this.m_DamageTableXML = xNodes.Item(0).Attributes[9].InnerText;
        this.m_AttackListXML = xNodes.Item(0).Attributes[10].InnerText;

        getTables();

        m_CardBase = Resources.Load("Card prefab") as GameObject;
        GameObject card = GameObject.Instantiate(m_CardBase, position, Quaternion.identity) as GameObject;
        card.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = Resources.Load("Materials/" + m_CardSprite) as Material;
        card.transform.GetChild(0).GetChild(1).GetComponent<MeshRenderer>().material = m_BackSprite;

    }

    void getTables()
    {
        string[] dmgStrings = m_DamageTableXML.Split(' ');
        string[] atkStrings = m_AttackListXML.Split(' ');

        for (int i = 0; i < dmgStrings.Length; i++)
        {
            m_DamageTable.Add(int.Parse(dmgStrings[i]));
        }

        for (int i = 0; i < atkStrings.Length; i++)
        {
            m_AttackList.Add(getAttackInfo(atkStrings[i]));    
        }

    }

    public AttackInfo getAttackInfo(string info)
    {
        AttackInfo atkInfo = new AttackInfo();

        XmlDocument doc = new XmlDocument();
        doc.Load(Application.dataPath + "/XML files/" + "attacks.xml");

        XmlNodeList xNodes = doc.SelectNodes("/ATList/Items/AttackInfo[@AttackCode = " + "'" + info + "'" + "]");

        atkInfo.m_AttackCode = xNodes.Item(0).Attributes[0].InnerText;
        atkInfo.m_AttackName = xNodes.Item(0).Attributes[1].InnerText;
        atkInfo.m_Damage = float.Parse(xNodes.Item(0).Attributes[2].InnerText);
        atkInfo.m_Type = xNodes.Item(0).Attributes[3].InnerText;
        atkInfo.m_AttackType = xNodes.Item(0).Attributes[4].InnerText;

        return atkInfo;
    }
}

[System.Serializable]
public struct AttackInfo
{
    [XmlAttribute("AttackCode")]
    public string m_AttackCode;
    [XmlAttribute("AttackName")]
    public string m_AttackName;
    [XmlAttribute("Damage")]
    public float m_Damage;
    [XmlAttribute("Type")]
    public string m_Type;
    [XmlAttribute("AttackType")]
    public string m_AttackType;

}

[System.Serializable]
public class CardList
{
    [XmlArray("Items")]
    public List<Card> items = new List<Card>();
}

[System.Serializable]
public class ATList
{
    [XmlArray("Items")]
    public List<AttackInfo> items = new List<AttackInfo>();
}