using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayLoop : MonoBehaviour
{
    public int dayNumber;
    public float moneyGoal;
    public float suspicionLimit;
    public float moneyGained;
    public float moneySpent;
    public float suspicionGained;
    public List<Client> clients;
    public int clientCount = 1;
    public  Client curClient;
    public  GameObject curFile;
    public GameObject clientFilePrefab;
    public Vector3 fileSize = new Vector3(1f, 1f, 1f);
    public Transform todoDeck;
    public Transform HandDeck;
    public Transform deniedDeck;
    public Transform approvedDeck;
    public List<GameObject> clientFiles;
    public GameObject recapPanel;
    public BarManager barManager;
    private GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
        gm.dayLoop = this;
        barManager.maxMoney = moneyGoal;
        barManager.maxSuspicion = suspicionLimit;
        RandomizeListOrder();
        InstantiateAllClientFiles();
        SetNextClient();
    }

    public void SetNextClient()
    {
        curClient = clients[clientCount];
        StartCoroutine(LerpPositionAndRotation(clientFiles[clientCount - 1], Vector3.zero, new Quaternion(0,0,0,1), HandDeck));
        curFile = clientFiles[clientCount - 1];
        clientCount += 1;
    }
    
    public void DeniedPressed()
    {
        Debug.Log("DENIED !");
        moneyGained += curClient.commission;
        barManager.AddMoney(curClient.commission);
        suspicionGained += suspicionCalculator(curClient.suspicion);
        barManager.AddSuspicion(suspicionCalculator(curClient.suspicion));
        StartCoroutine(LerpPositionAndRotation(curFile, Vector3.zero, new Quaternion(0,0,0,1), deniedDeck));
        CheckEndingDay();
        SetNextClient();
    }


    public void ApprovedPressed()
    {
        Debug.Log("Approved.");
        moneySpent -= curClient.cost;
        if (curClient.reward)
        {
            moneyGained += curClient.rewardAmount;
            barManager.AddMoney(curClient.rewardAmount);
            barManager.AddSuspicion(suspicionCalculator(curClient.suspicion));
        } else 
        {
            barManager.AddSuspicion(-suspicionCalculator(curClient.suspicion));
        }
        StartCoroutine(LerpPositionAndRotation(curFile, Vector3.zero, new Quaternion(0,0,0,1), approvedDeck));
        CheckEndingDay();
        SetNextClient();
    }
    public void InstantiateAllClientFiles()
    {
        int i = 0;
        foreach (Client client in clients)
        {
            GameObject tmp = Instantiate(clientFilePrefab, todoDeck, false);
            tmp.GetComponent<ClientFile>().UpdateClient(client);
            tmp.transform.localPosition = Vector3.zero;
            tmp.transform.localScale = fileSize;
            clientFiles.Add(tmp);
        }
    }

    private void CheckEndingDay()
    {
        if (suspicionGained >= suspicionLimit)
        {
            EndDay(false);
        }
        if (moneyGained >= moneyGoal)
        {
            EndDay(true);
        }
    }

    public int suspicionCalculator(Suspicion suspicion)
    {
        switch(suspicion)
        {
            case Suspicion.None:
                return 0;
            case Suspicion.Low:
                return 20;
            case Suspicion.Medium:
                return 40;
            case Suspicion.High:
                return 60;
            case Suspicion.Extreme:
                return 80;
            default:
                return 0;
        }

    }
    public void RandomizeListOrder()
    {
        // Use Fisher-Yates shuffle algorithm to randomize the list
        int count = clients.Count;
        for (int i = count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            // Swap elements
            Client temp = clients[i];
            clients[i] = clients[randomIndex];
            clients[randomIndex] = temp;
        }
    }

    private void EndDay(bool success)
    {
        Debug.Log("Day ended");
        recapPanel.SetActive(true);
        Time.timeScale = 0;
        // Trigger Recap Panel
    }
    // Coroutine to lerp the position
    private IEnumerator LerpPositionAndRotation(GameObject stuff, Vector3 targetLocalPosition, Quaternion targetLocalRotation, Transform newParent)
    {
        float elapsedTime = 0f;
        stuff.transform.parent = newParent;
        Vector3 startingPosition = stuff.transform.localPosition;
        Quaternion startingRotation = stuff.transform.localRotation;

        while (elapsedTime < 1.0f)
        {
            stuff.transform.localPosition = Vector3.Lerp(startingPosition, targetLocalPosition, elapsedTime / 1.0f);
            stuff.transform.localRotation = Quaternion.Slerp(startingRotation, targetLocalRotation, elapsedTime / 1.0f);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final position and rotation are exactly the target position and rotation
        stuff.transform.localPosition = Vector3.zero;
        stuff.transform.localRotation = new Quaternion(0,0,0,1);
    }
}
