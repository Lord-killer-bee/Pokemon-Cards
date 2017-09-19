using UnityEngine;
using System.Collections;

public class PokeType : MonoBehaviour {

    public static PokeType instance;

    public Hashtable m_TypesList = new Hashtable();
    public Hashtable m_ColorList = new Hashtable();//convert these hex to Color object and use it.
    
    
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }

        SetTypeValues();
        SetColorValues();
    }

    void SetTypeValues()
    {
        m_TypesList.Add("Fire", 0);
        m_TypesList.Add("Water", 1);
        m_TypesList.Add("Grass", 2);
        m_TypesList.Add("Electric", 3);
        m_TypesList.Add("Psychic", 4);
        m_TypesList.Add("Ice", 5);
        m_TypesList.Add("Dragon", 6);
        m_TypesList.Add("Dark", 7);
        m_TypesList.Add("Fairy", 8);
        m_TypesList.Add("Normal", 9);
        m_TypesList.Add("Fight", 10);
        m_TypesList.Add("Flying", 11);
        m_TypesList.Add("Poison", 12);
        m_TypesList.Add("Ground", 13);
        m_TypesList.Add("Rock", 14);
        m_TypesList.Add("Bug", 15);
        m_TypesList.Add("Ghost", 16);
        m_TypesList.Add("Steel", 17);
    }

    void SetColorValues()
    {
        m_ColorList.Add("Fire", "D50000");
        m_ColorList.Add("Water", "0095FF");
        m_ColorList.Add("Grass", "00D51C");
        m_ColorList.Add("Electric", "F0FF00");
        m_ColorList.Add("Psychic", "FF008E");
        m_ColorList.Add("Ice", "C4E9FF");
        m_ColorList.Add("Dragon", "7200FF");
        m_ColorList.Add("Dark", "3A3A3A");
        m_ColorList.Add("Fairy", "FF9FE0");
        m_ColorList.Add("Normal", "C6C6C6");
        m_ColorList.Add("Fight", "810000");
        m_ColorList.Add("Flying", "8E8FFF");
        m_ColorList.Add("Poison", "C600FF");
        m_ColorList.Add("Ground", "D7AA67");
        m_ColorList.Add("Rock", "A36A14");
        m_ColorList.Add("Bug", "BEDD2F");
        m_ColorList.Add("Ghost", "7D508A");
        m_ColorList.Add("Steel", "BABBD2");
    }

}
