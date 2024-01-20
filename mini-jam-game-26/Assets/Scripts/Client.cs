using UnityEngine;

public enum Suspicion {None, Low, Medium, High, Extreme}
[CreateAssetMenu(menuName = "Client")]
public class Client : ScriptableObject
{
    public string clientName;
    public int clientID;
    public int age;
    public string reason;
    public string procedure;
    public string moreInfos;
    public Suspicion suspicion;
    public float cost;
    public float commission;
    public bool reward;
    public float rewardAmount;
    public Sprite picture;
}
