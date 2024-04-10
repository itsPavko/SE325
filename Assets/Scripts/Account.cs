using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Account : MonoBehaviour
{

    public string accountName;
    public TextMeshProUGUI accountNameText;
    public float balance;
    public TextMeshProUGUI BalanceText;

    public GameObject accountHolder;
    public GameObject menuMask;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateBalance(float n)
    {
        balance += n;
        BalanceText.text = "$" + balance.ToString("0.0");
        SaveData(accountName);
    }

    public void LoadData(string username)
    {
        Debug.Log("Logged in as " + username);
        balance = PlayerPrefs.GetFloat(username + "Balance");
        UpdateBalance(0);
        accountName = username;
        accountNameText.text = accountName;
        accountHolder.SetActive(true);
		menuMask.SetActive(false);
		GetComponent<Login>().HidePannels();

	}

	public void SaveData(string username)
    {
        PlayerPrefs.SetFloat((username + "Balance"), balance);
    }
    
    public void LogOut()
    {
        SaveData(accountName);
		accountHolder.SetActive(false);
        menuMask.SetActive(true);
		GetComponent<Login>().ShowPannels();

    }
}
