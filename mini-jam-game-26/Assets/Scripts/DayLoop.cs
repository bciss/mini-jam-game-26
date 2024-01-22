using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public  GameObject curFile = null;
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
    public bool canTamp = false;
    public List<GameObject> preStarts;
    private int countPre;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
        gm.dayLoop = this;
        barManager.maxMoney = moneyGoal;
        barManager.maxSuspicion = suspicionLimit;
        RandomizeListOrder();
        InstantiateAllClientFiles();
        clientCount = clients.Count - 1;
        countPre = preStarts.Count;
    }

    public void SetNextClient()
    {
        if (countPre > 0)
        {
            Destroy(preStarts[countPre - 1].gameObject);
            countPre -= 1;
            return ;
        }
        if (canTamp && curFile == null)
        {
            Debug.Log(clientCount);
            curClient = clients[clientCount];
            clientFiles[clientCount].GetComponent<ClientFile>().PlayPaperSound();
            StartCoroutine(LerpPositionAndRotation(clientFiles[clientCount], Vector3.zero, new Quaternion(0,0,0,1), HandDeck));
            curFile = clientFiles[clientCount];
            clientCount -= 1;
        }
    }
    
    public void DeniedPressed()
    {
        if (!canTamp || curFile == null) { return ;}
        canTamp = false;
        Debug.Log("DENIED !");
        moneyGained += curClient.commission;
        barManager.AddMoney(curClient.commission);
        suspicionGained += suspicionCalculator(curClient.suspicion);
        barManager.AddSuspicion(suspicionCalculator(curClient.suspicion));
        curFile.GetComponent<ClientFile>().PlayPaperSound();
        StartCoroutine(LerpPositionAndRotation(curFile, Vector3.zero, new Quaternion(0,0,0,1), deniedDeck));
        CheckEndingDay();
    }


    public void ApprovedPressed()
    {

        if (!canTamp || curFile == null) { return ;}
        canTamp = false;
        Debug.Log("Approved.");
        moneySpent -= curClient.cost;
        if (curClient.reward)
        {
            moneyGained += curClient.rewardAmount;
            // suspicionGained += suspicionCalculator(curClient.suspicion);
            barManager.AddMoney(curClient.rewardAmount);
            // barManager.AddSuspicion(suspicionCalculator(curClient.suspicion));
        } else 
        {
            suspicionGained -= suspicionCalculator(curClient.suspicion);
            if (suspicionGained < 0) {suspicionGained = 0 ;}
            barManager.AddSuspicion(-suspicionCalculator(curClient.suspicion));
        }
        curFile.GetComponent<ClientFile>().PlayPaperSound();
        StartCoroutine(LerpPositionAndRotation(curFile, Vector3.zero, new Quaternion(0,0,0,1), approvedDeck));
        CheckEndingDay();
    }
    public void InstantiateAllClientFiles()
    {
        // int i = 0;
        foreach (Client client in clients)
        {
            GameObject tmp = Instantiate(clientFilePrefab, todoDeck, false);
            tmp.GetComponent<ClientFile>().UpdateClient(client);
            tmp.transform.localPosition = new Vector3(0,0,-3f);
            tmp.transform.localScale = fileSize;
            clientFiles.Add(tmp);
            StartCoroutine(LerpPositionAndRotation(tmp, Vector3.zero, new Quaternion(0,0,0,1), todoDeck));
        }
        canTamp = true;
    }

    private void CheckEndingDay()
    {
        if ((suspicionGained >= suspicionLimit) || (clientCount < 0))
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
        Debug.Log("Day ended, gain : " + moneyGained + " limit : " + suspicionGained);
        // recapPanel.SetActive(true);
        Time.timeScale = 0;
        
        StartCoroutine(DelayedEnd());
        Destroy(GameManager.Instance);
        if (success)
        {
            SceneManager.LoadScene("WinEndScene");
        }
        else
        {
            SceneManager.LoadScene("LoseEndScene");
        }

        // Trigger Recap Panel
    }
    

    IEnumerator DelayedEnd()
    {
        // Print a message before waiting
        Debug.Log("Waiting for 1.5 seconds...");

        // Wait for 2 seconds
        yield return new WaitForSeconds(1.5f);

        // Print a message after waiting
        Debug.Log("Finished waiting for 1.5 seconds.");

        // Perform any other actions you want to do after the wait
    }
    // Coroutine to lerp the position
    private IEnumerator LerpPositionAndRotation(GameObject stuff, Vector3 targetLocalPosition, Quaternion targetLocalRotation, Transform newParent)
    {
        canTamp = false;
        float elapsedTime = 0f;
        stuff.transform.parent = newParent;
        Vector3 pileTargetPosition = Vector3.zero;
        Vector3 startingPosition = stuff.transform.localPosition;
        Quaternion startingRotation = stuff.transform.localRotation;
        if (newParent.gameObject.tag != "Hand")
        {
            pileTargetPosition = new Vector3(targetLocalPosition.x, targetLocalPosition.y, targetLocalPosition.y - ((float)newParent.childCount / 100));
        }

        while (elapsedTime < 1.0f)
        {
            stuff.transform.localPosition = Vector3.Lerp(startingPosition, pileTargetPosition, elapsedTime / 1.0f);
            stuff.transform.localRotation = Quaternion.Slerp(startingRotation, targetLocalRotation, elapsedTime / 1.0f);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final position and rotation are exactly the target position and rotation
        stuff.transform.localPosition = pileTargetPosition;
        stuff.transform.localRotation = new Quaternion(0,0,0,1);
        if (newParent.gameObject.tag != "Hand")
        {
            curFile = null;
        }
        canTamp = true;
    }
}
