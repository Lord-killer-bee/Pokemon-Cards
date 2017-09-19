using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CardMan : Photon.MonoBehaviour {

    List<int> feedArray_1 = new List<int>();
    List<int> feedArray_2 = new List<int>();

    public List<Card> player_1_Stack = new List<Card>();
    public List<Card> player_2_Stack = new List<Card>();

    private int[] pool_1 = { 79 };
    private int[] pool_2 = { 80, 78, 77, 76, 75 };
    private int[] pool_3 = { 1, 2, 3, 25, 40, 65, 73 };

    void OnJoinedRoom()
    {
        if (PhotonNetwork.isMasterClient)
        {
            int[] array1 = { 0, 0, 0, 0, 0, 0 };
            int[] array2 = { 0, 0, 0, 0, 0, 0 };

            feedArray_1 = drawFromPools();
            feedArray_2 = drawFromPools();
            setUpCards(feedArray_1, feedArray_2);

            for (int i = 0; i < feedArray_1.Count; i++)
            {
                array1[i] = feedArray_1[i];
            }
            for (int i = 0; i < feedArray_2.Count; i++)
            {
                array2[i] = feedArray_2[i];
            }
            PhotonNetwork.RPC(gameObject.GetPhotonView(), "setUpCardsPN", PhotonTargets.OthersBuffered, false, array2, array1);
        }
    }

    void Update()
    {
        flipCard();
    }

    void setUpCards(List<int> feed1, List<int> feed2)
    {
        int i = 0;
        foreach (int item in feed1)
        {
            Card card = new Card(item, new Vector3(-3 + i * (1.2f), 0.1f, -3.5f));
            player_1_Stack.Add(card);
            gameObject.GetComponent<BattleMan>().shuffleButtons[i].GetComponentInChildren<Text>().text = card.m_Name;
            i++;
        }

        i = 0;
        foreach (int item in feed2)
        {
            Card card = new Card(item, new Vector3(-3 + i * (1.2f), 0.1f, 3.5f));
            player_2_Stack.Add(card);
            i++;
        }

        dealTheCards();
    }

    [PunRPC]
    public void setUpCardsPN(int[] feed2, int[] feed1)
    {
        int i = 0;
        foreach (int item in feed2)
        {
            Card card = new Card(item, new Vector3(-3 + i * (1.2f), 0.1f, -3.5f));
            player_2_Stack.Add(card);
            gameObject.GetComponent<BattleMan>().shuffleButtons[i].GetComponentInChildren<Text>().text = card.m_Name;
            i++;
        }

        i = 0;
        foreach (int item in feed1)
        {
            Card card = new Card(item, new Vector3(-3 + i * (1.2f), 0.1f, 3.5f));
            player_1_Stack.Add(card);
            i++;
        }
        dealTheCards();
    }

    List<int> drawFromPools()
    {
        List<int> result = new List<int>();

        for (int j = 0; j < 3; j++)
        {
            int[] draws = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            for (int i = 0; i < 10; i++)
            {
                draws[i] = Random.Range(0, 1);
            }

            //case 1
            if(j == 0)
            {
                int count = 0;
                foreach(int item in draws)
                {
                    if(item == 1)
                    {
                        count++;
                    }
                }

                if(count == 10)
                {
                    //add the mewtwo pool
                    int var = pool_1[Random.Range(0, pool_1.Length - 1)];

                    while(!feedArray_1.Contains(var) && !feedArray_2.Contains(var) && !result.Contains(var))
                    {
                        result.Add(var);
                    }
                }

            }

            //case 2
            if(j == 1)
            {
                int count = 0;
                foreach (int item in draws)
                {
                    if (item == 1)
                    {
                        count++;
                    }
                }

                if (count == 9)
                {
                    //add the legendry pool
                    int var = pool_2[Random.Range(0, pool_2.Length - 1)];

                    while (!feedArray_1.Contains(var) && !feedArray_2.Contains(var) && !result.Contains(var))
                    {
                        result.Add(var);
                    }
                }
            }

            //case 3
            if (j == 2)
            {
                int count = 0;
                foreach (int item in draws)
                {
                    if (item == 1)
                    {
                        count++;
                    }
                }

                if (count >= 0)
                {
                    //add the starter pool
                    int var = pool_3[Random.Range(0, pool_3.Length - 1)];

                    while (!feedArray_1.Contains(var) && !feedArray_2.Contains(var) && !result.Contains(var))
                    {
                        result.Add(var);
                    }
                }
            }
        }

        for (int i = result.Count; i < 6; i++)
        {
            int var = Random.Range(1, 80); 
            
            while(pool_1.Contains(var) || pool_2.Contains(var) || pool_3.Contains(var) || result.Contains(var) || feedArray_1.Contains(var) || feedArray_2.Contains(var))
            {
                var = Random.Range(1, 80);
            }

            result.Add(var);
        }

        return result;
    }

    void dealTheCards()
    {       
        GameObject[] deck;
        deck = GameObject.FindGameObjectsWithTag("Card");

        int i = 1;
   
        foreach (GameObject item in deck)
        {
            if (i < 7)
            {
                item.transform.Rotate(-90, 0, 180);
            }
            else
            {
                item.transform.Rotate(-90, 0, 0);
            }
            i++;
        }        
    }

    void flipCard()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit) && Input.GetMouseButtonDown(0))
        {
            if(hit.collider.tag == "Card")
            {

            }
        }
    }
}
