using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ClientFile : MonoBehaviour
{
    public Image picture;
    public TMP_Text iDText;
    public TMP_Text nameText;
    public TMP_Text ageText;
    public TMP_Text reasonText;
    public TMP_Text procedureText;
    public TMP_Text moreInfosText;
    public TMP_Text costText;

    public void UpdateClient(Client client)
    {
        // picture.sprite = client.picture;
        costText.text = "$ " + client.cost.ToString();
        iDText.text = client.clientID.ToString();
        nameText.text = client.clientName;
        ageText.text = client.age.ToString();
        reasonText.text = client.reason;
        procedureText.text = client.procedure;
        moreInfosText.text = client.moreInfos;
    }
}
