using UnityEngine;
using System.Collections;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class XMLSerializer : MonoBehaviour {

    public ATList list;

    XmlSerializer serializer;
    FileStream stream;

    XmlDocument doc = new XmlDocument();

    // Use this for initialization
    void Start () {
        //serializer = new XmlSerializer(typeof(ATList));
        //stream = new FileStream(Application.dataPath + "/XML files/" + "attackdata.xml", FileMode.OpenOrCreate);
        //SaveItems();
        //LoadObject();
        Card card1 = new Card(2, new Vector3(0,0,0));
        //Card card2 = new Card(2);

        print(card1.m_AttackList[0].m_Damage);
        print(card1.m_AttackList[1].m_Damage);
        print(card1.m_AttackList[2].m_Damage);
        print(card1.m_AttackList[3].m_Damage);

        /*print(card2.m_AttackList[0].m_Damage);
        print(card2.m_AttackList[1].m_Damage);
        print(card2.m_AttackList[2].m_Damage);
        print(card2.m_AttackList[3].m_Damage);
        */
    }

    public void SaveItems()
    {
        SaveObjects();

        serializer.Serialize(stream, list);

        stream.Flush();

    }

    void LoadObject()
    {
        doc.Load(Application.dataPath + "/XML files/" + "pokedata.xml");
        XmlNodeList xNodes = doc.SelectNodes("/CardList/Items/Card[@Name = 'Charizard']");
        
        foreach (XmlNode xn in xNodes)
        {
            string x = xn.Attributes[9].InnerText;
            string[] substrings = x.Split(' ');

            foreach (string item in substrings)
            {
                print(item);
            }

        }
        doc = null;
    }


    void SaveObjects()
    {
        /*Card card1 = new Card();

        card1.m_Index = 1;
        card1.m_Name = "Charizard";
        card1.m_CardSprite = "1_Charizard";
        card1.m_HP = 100;
        card1.m_Speed = 100;
        card1.m_Attack = 100;
        card1.m_Defense = 100;
        card1.m_SpecialAttack = 100;
        card1.m_SpecialDefense = 100;
        card1.m_DamageTableXML = "0 1 2 3 4 2 2 3 3 3 2 3 2 3 2 3 2 1";        
        card1.m_AttackListXML = "FS DC DC DC";
        list.items.Add(card1);

        Card card2 = new Card();

        card2.m_Index = 2;
        card2.m_Name = "Blastoise";
        card2.m_CardSprite = "2_Blastoise";
        card2.m_HP = 100;
        card2.m_Speed = 100;
        card2.m_Attack = 100;
        card2.m_Defense = 100;
        card2.m_SpecialAttack = 100;
        card2.m_SpecialDefense = 100;
        card2.m_DamageTableXML = "0 1 2 3 4 2 2 3 3 3 2 3 2 3 2 3 2 1";
        card2.m_AttackListXML = "FS DC DC DC";
        list.items.Add(card2);*/

        AttackInfo af1 = new AttackInfo();

        af1.m_AttackCode = "AC";
        af1.m_AttackName = "Attack";
        af1.m_Damage = 50;
        af1.m_Type = "Type";
        af1.m_AttackType = "A";

        list.items.Add(af1);

        AttackInfo af2 = new AttackInfo();

        af2.m_AttackCode = "AC";
        af2.m_AttackName = "Attack";
        af2.m_Damage = 50;
        af2.m_Type = "Type";
        af2.m_AttackType = "A";

        list.items.Add(af2);
    }
}
