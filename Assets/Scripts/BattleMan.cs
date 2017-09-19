using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleMan : Photon.MonoBehaviour {

    public BattlePhase m_BP;
    public Button readyBtn;
    public GameObject shuffleShowPanel;
    public GameObject shufflePanel;

    private int[] battleSetter = { 0, 0, 0, 0, 0, 0 };

    private int readyState = 0;
    [SerializeField]
    private int noOfBattles = 0;
    private int shuffleCounter = 0;
    private int totalShuffles = 0;

    public List<Button> shuffleButtons = new List<Button>();
    private List<Button> shuffleList = new List<Button>();

    private bool shufflePhase;

    void OnJoinedRoom()
    {
        readyBtn.onClick.AddListener(() => setReadyState());
    }

    void Update()
    {
        if(readyState == 2)
        {
            readyState = 0;
            for (int i = 0; i < battleSetter.Length; i++)
            {
                if(battleSetter[i] == 0)
                {
                    if (isBattlePossible(i))
                    {
                        m_BP.battlePhase = true;
                        if (PhotonNetwork.isMasterClient)
                        {
                            if (isPlayer1Faster(i))
                            {
                                callBattleFuncPN(i, 1, true);
                                gameObject.GetPhotonView().RPC("callBattleFuncPN", PhotonTargets.OthersBuffered, i, 2, false);                   
                            }
                            else
                            {
                                callBattleFuncPN(i, 1, false);
                                gameObject.GetPhotonView().RPC("callBattleFuncPN", PhotonTargets.OthersBuffered, i, 2, true);
                            }
                        }
                    }
                    else
                    {
                        battleSetter[i] = 1;
                        noOfBattles++;
                        continue;
                    }
                    battleSetter[i] = 1;
                    noOfBattles++;
                    break;
                }
            }          
        }

        if(noOfBattles == 6)
        {
            shufflePhase = true;
            resetBattleSetter();
        }
        
        if(shuffleCounter == 2)
        {
            shuffleButtonsFunc();
            shuffleCounter = 0;
        }

        if(shufflePhase == true && m_BP.battlePhase == false)
        {
            StartCoroutine(showShuffleMessage());
            shufflePhase = false;
        }
    }

    [PunRPC]
    void callBattleFuncPN(int index, int player, bool isPlayer1Faster)
    {
        m_BP.BattlePhaseStart(index, player, isPlayer1Faster);
    }


    void setReadyState()
    {
        readyState++;
        readyBtn.gameObject.SetActive(false);
        gameObject.GetPhotonView().RPC("setReadyStatePN", PhotonTargets.OthersBuffered);        
    }

    [PunRPC]
    void setReadyStatePN()
    {
        readyState++;
    }

    void resetBattleSetter()
    {
        for (int i = 0; i < battleSetter.Length; i++)
        {
            battleSetter[i] = 0;   
        }
        noOfBattles = 0;
    }

    public void addButtonsforShuffle()
    {
        shuffleList.Add(EventSystem.current.currentSelectedGameObject.GetComponent<Button>());
        shuffleCounter++;
    }

    void shuffleButtonsFunc()
    {
        Vector3 temp;

        temp = shuffleList[0].GetComponent<RectTransform>().anchoredPosition;
        shuffleList[0].GetComponent<RectTransform>().anchoredPosition = shuffleList[1].GetComponent<RectTransform>().anchoredPosition;
        shuffleList[1].GetComponent<RectTransform>().anchoredPosition = temp;

        Card temp1;

        temp1 = gameObject.GetComponent<CardMan>().player_1_Stack[(int.Parse(shuffleList[0].name))];
        gameObject.GetComponent<CardMan>().player_1_Stack[(int.Parse(shuffleList[0].name))] = gameObject.GetComponent<CardMan>().player_1_Stack[(int.Parse(shuffleList[1].name))];
        gameObject.GetComponent<CardMan>().player_1_Stack[(int.Parse(shuffleList[1].name))] = temp1;

        shuffleList.Clear();

        totalShuffles++;

        if(totalShuffles == 2)
        {
            totalShuffles = 0;
            StartCoroutine(hideShufflePanel());          
        }
    }

    bool isPlayer1Faster(int index)
    {
        float speed1 = GameObject.Find("Gameplay manager").GetComponent<CardMan>().player_1_Stack[index].m_Speed;
        float speed2 = GameObject.Find("Gameplay manager").GetComponent<CardMan>().player_2_Stack[index].m_Speed;

        if(speed1 > speed2)
        {
            return true;
        }else
        {
            return false;
        }
    }

    bool isBattlePossible(int index)
    {
        if(gameObject.GetComponent<CardMan>().player_1_Stack[index] == null || gameObject.GetComponent<CardMan>().player_2_Stack[index] == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /*bool shouldPlayer1ShuffleFirst()
    {

    }*/

    IEnumerator showShuffleMessage()
    {
        shuffleShowPanel.SetActive(true);
        shufflePanel.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        shuffleShowPanel.SetActive(false);
    }

    IEnumerator hideShufflePanel()
    {
        shuffleShowPanel.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        shuffleShowPanel.SetActive(false);
        shufflePanel.SetActive(false);
    }
}
