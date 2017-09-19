using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattlePhase : MonoBehaviour {

    public Image m_PokeSprite;
    public Image m_OpponentShadow;
    public Image m_Fader;
    public Image m_TypeAttack;
    public Image m_TypeRecieve;

    public StatsBox m_StatsBox;
    public AttackBox m_AttackBox;

    public Text m_DamageValuesMade;
    public Text m_MultiplierValuesMade;

    public Text m_DamageValuesTaken;
    public Text m_MultiplierValuesTaken;

    public Text m_TypeText;

    public GameObject offensePanel;
    public GameObject defensePanel;

    public GameObject readyButton;

    public bool battlePhase;

    private bool isPlayer1Faster;
    private int index;
    private int player;

    Card refCard, oppCard;

    public void BattlePhaseStart(int index, int player, bool isPlayer1Faster)
    {
        disableInfo();
        removeListeners();
       
        this.isPlayer1Faster = isPlayer1Faster;
        this.index = index;
        this.player = player;

        battlePhase = true;
        gameObject.SetActive(true);
        m_Fader.color = new Color(0, 0, 0, 0.75f);             

        if (player == 1)
        {
            refCard = GameObject.Find("Gameplay manager").GetComponent<CardMan>().player_1_Stack[index];
            oppCard = GameObject.Find("Gameplay manager").GetComponent<CardMan>().player_2_Stack[index];
        }
        else
        {
            refCard = GameObject.Find("Gameplay manager").GetComponent<CardMan>().player_2_Stack[index];
            oppCard = GameObject.Find("Gameplay manager").GetComponent<CardMan>().player_1_Stack[index];
        }

        if (isPlayer1Faster)
        {
            StartCoroutine(playAttackPhase());
        }
        else
        {
            playDefensePhase();
        }

        m_PokeSprite.sprite = Resources.Load<Sprite>("Sprites/Cards/" + refCard.m_CardSprite);
        m_OpponentShadow.sprite = Resources.Load<Sprite>("Sprites/back");     

        updateStats();

        m_AttackBox.attack_1.transform.GetChild(0).GetComponent<Text>().text = refCard.m_AttackList[0].m_AttackName + " : " + refCard.m_AttackList[0].m_Damage + " (" + refCard.m_AttackList[0].m_AttackType + ")";
        m_AttackBox.attack_2.transform.GetChild(0).GetComponent<Text>().text = refCard.m_AttackList[1].m_AttackName + " : " + refCard.m_AttackList[1].m_Damage + " (" + refCard.m_AttackList[1].m_AttackType + ")";
        m_AttackBox.attack_3.transform.GetChild(0).GetComponent<Text>().text = refCard.m_AttackList[2].m_AttackName + " : " + refCard.m_AttackList[2].m_Damage + " (" + refCard.m_AttackList[2].m_AttackType + ")";
        m_AttackBox.attack_4.transform.GetChild(0).GetComponent<Text>().text = refCard.m_AttackList[3].m_AttackName + " : " + refCard.m_AttackList[3].m_Damage + " (" + refCard.m_AttackList[3].m_AttackType + ")";

        m_AttackBox.attack_1.GetComponent<Image>().color = hexToColor((string)PokeType.instance.m_ColorList[refCard.m_AttackList[0].m_Type]);
        m_AttackBox.attack_2.GetComponent<Image>().color = hexToColor((string)PokeType.instance.m_ColorList[refCard.m_AttackList[1].m_Type]);
        m_AttackBox.attack_3.GetComponent<Image>().color = hexToColor((string)PokeType.instance.m_ColorList[refCard.m_AttackList[2].m_Type]);
        m_AttackBox.attack_4.GetComponent<Image>().color = hexToColor((string)PokeType.instance.m_ColorList[refCard.m_AttackList[3].m_Type]);

        m_AttackBox.attack_1.onClick.AddListener(() => calculateDamage(0));
        m_AttackBox.attack_2.onClick.AddListener(() => calculateDamage(1));
        m_AttackBox.attack_3.onClick.AddListener(() => calculateDamage(2));
        m_AttackBox.attack_4.onClick.AddListener(() => calculateDamage(3));

    }
    
    int calculateDamage(int attackIndex)
    {
        int damage = 0;

        string attacktype;
        float damageMultiplier;

        damageMultiplier = getDamagaMultiplier(oppCard.m_DamageTable[(int)(PokeType.instance.m_TypesList[refCard.m_AttackList[attackIndex].m_Type])]);
        attacktype = refCard.m_AttackList[attackIndex].m_AttackType;

        if (attacktype == "A")
        {
            damage = (int)((refCard.m_AttackList[attackIndex].m_Damage * refCard.m_Attack) * damageMultiplier / (2 * oppCard.m_Defense));
        }
        else
        {
            damage = (int)((refCard.m_AttackList[attackIndex].m_Damage * refCard.m_SpecialAttack) * damageMultiplier / (2 * oppCard.m_SpecialDefense));
        }

        showDamageText(damage, damageMultiplier);
        gameObject.GetPhotonView().RPC("showDamageTextPN", PhotonTargets.OthersBuffered, damage, damageMultiplier);

        showAttackTypeInfo(refCard.m_AttackList[attackIndex].m_Type);
        gameObject.GetPhotonView().RPC("showAttackTypeInfoPN", PhotonTargets.OthersBuffered, refCard.m_AttackList[attackIndex].m_Type);

        applyDamage(damage);
        gameObject.GetPhotonView().RPC("applyDamagePN", PhotonTargets.OthersBuffered, damage);
        return damage;
    }

    void applyDamage(int damage)
    {
        oppCard.m_HP -= damage;

        if(oppCard.m_HP <= 0)
        {
            oppCard.m_HP = 0;
            StartCoroutine(reactivateReadyButton());
            if(player == 1)
            {
                GameObject.Find("Gameplay manager").GetComponent<CardMan>().player_2_Stack[index] = null;             
            }
            else
            {
                GameObject.Find("Gameplay manager").GetComponent<CardMan>().player_1_Stack[index] = null;
            }
            oppCard = null;
            return;
        }
        
        if (isPlayer1Faster)
        {
            playDefensePhase();
        }
        else
        {
            offensePanel.SetActive(true);
            StartCoroutine(reactivateReadyButton());
        }

    }

    [PunRPC]
    void applyDamagePN(int damage)
    {
        refCard.m_HP -= damage;

        if (refCard.m_HP <= 0)
        {
            refCard.m_HP = 0;
            StartCoroutine(reactivateReadyButton());
            updateStats();
            if (player == 1)
            {
                GameObject.Find("Gameplay manager").GetComponent<CardMan>().player_1_Stack[index] = null;
            }
            else
            {
                GameObject.Find("Gameplay manager").GetComponent<CardMan>().player_2_Stack[index] = null;
            }
            refCard = null;
            return;
        }

        updateStats();

        if (isPlayer1Faster)
        {
            StartCoroutine(reactivateReadyButton());
        }
        else
        {
            StartCoroutine(playAttackPhase());
        }

    }

    void showDamageText(int damage, float damageMultiplier)
    {
        m_DamageValuesMade.text = damage.ToString();
        m_MultiplierValuesMade.text = "x" + damageMultiplier.ToString();
    }

    [PunRPC]
    void showDamageTextPN(int damage, float damageMultiplier)
    {
        m_DamageValuesTaken.text = damage.ToString();
        m_MultiplierValuesTaken.text = "x" + damageMultiplier.ToString();
    }

    void showAttackTypeInfo(string type)
    {
        string color = PokeType.instance.m_ColorList[type] as string;

        m_TypeAttack.gameObject.SetActive(true);
        m_TypeRecieve.gameObject.SetActive(false);
        m_TypeAttack.GetComponent<Image>().color = hexToColor(color);

        m_TypeText.gameObject.SetActive(true);
        m_TypeText.text = type;
    }

    [PunRPC]
    void showAttackTypeInfoPN(string type)
    {
        string color = PokeType.instance.m_ColorList[type] as string;

        m_TypeAttack.gameObject.SetActive(false);
        m_TypeRecieve.gameObject.SetActive(true);
        m_TypeRecieve.GetComponent<Image>().color = hexToColor(color);

        m_TypeText.gameObject.SetActive(true);
        m_TypeText.text = type;
    }

    void disableInfo()
    {
        m_TypeAttack.gameObject.SetActive(false);
        m_TypeRecieve.gameObject.SetActive(false);
        m_TypeText.gameObject.SetActive(false);

        m_DamageValuesMade.text = " ";
        m_MultiplierValuesMade.text = " ";

        m_DamageValuesTaken.text = " ";
        m_MultiplierValuesTaken.text = " ";
    }

    void removeListeners()
    {
        m_AttackBox.attack_1.onClick.RemoveAllListeners();
        m_AttackBox.attack_2.onClick.RemoveAllListeners();
        m_AttackBox.attack_3.onClick.RemoveAllListeners();
        m_AttackBox.attack_4.onClick.RemoveAllListeners();
    }

    float getDamagaMultiplier(int index)
    {
        float result = 0;

        switch (index)
        {
            case 0:
                result = 0.25f;
                break;

            case 1:
                result = 0.5f;
                break;

            case 2:
                result = 0f;
                break;

            case 3:
                result = 1f;
                break;

            case 4:
                result = 2f;
                break;

            case 5:
                result = 4f;
                break;
        }

        return result;
    }

    IEnumerator playAttackPhase()
    {
        defensePanel.SetActive(false);
        offensePanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        offensePanel.SetActive(false);
    }

    void playDefensePhase()
    {
        offensePanel.SetActive(false);
        defensePanel.SetActive(true);
    }

    IEnumerator reactivateReadyButton()
    {
        yield return new WaitForSeconds(4f);

        battlePhase = false;
        gameObject.SetActive(false);
        readyButton.SetActive(true);
    }

    Color hexToColor(string hex)
    {       
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        byte a = 255;

        return new Color32(r, g, b, a);
    }

    void updateStats()
    {
        m_StatsBox.hp = refCard.m_HP;
        m_StatsBox.speed = refCard.m_Speed;
        m_StatsBox.attack = refCard.m_Attack;
        m_StatsBox.defense = refCard.m_Defense;
        m_StatsBox.specialAttack = refCard.m_SpecialAttack;
        m_StatsBox.specialDefence = refCard.m_SpecialDefense;

        m_StatsBox.m_StatsTemplate.text = "HP: " + m_StatsBox.hp + "\t" + "Speed: " + m_StatsBox.speed + "\n" +
                                           "Attack: " + m_StatsBox.attack + "\t" + "Special Attack: " + m_StatsBox.specialAttack + "\n" +
                                           "Defense: " + m_StatsBox.defense + "\t" + "Special Defense: " + m_StatsBox.specialDefence;

    }

}

[System.Serializable]
public struct StatsBox
{
    public Text m_StatsTemplate;

    public float hp;
    public float speed;
    public float attack;
    public float defense;
    public float specialAttack;
    public float specialDefence;

}

[System.Serializable]
public struct AttackBox
{
    public Button attack_1;
    public Button attack_2;
    public Button attack_3;
    public Button attack_4;
}