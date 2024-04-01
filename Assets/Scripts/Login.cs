using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Login : MonoBehaviour
{

    public GameObject loginPanel;
    public GameObject signupPanel;

    public TMP_InputField usernameLogIn;
    public TMP_InputField passwordLogIn;
    public TMP_InputField usernameSignUp;
    public TMP_InputField passwordSignUp;
    public TextMeshProUGUI error;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwitchToLogin()
    {
        loginPanel.SetActive(true);
        signupPanel.SetActive(false);
    }

    public void SwitchToSignUp()
    {
        loginPanel.SetActive(false);
        signupPanel.SetActive(true);
    }

    public void SignUp()
    {
        if (usernameSignUp.text != "" && passwordSignUp.text != "")
        {
            if (PlayerPrefs.HasKey(usernameSignUp.text))
            {
                error.text = "Account name already exists!";
            }
            else
            {
                PlayerPrefs.SetString(usernameSignUp.text, passwordSignUp.text);
                PlayerPrefs.SetFloat((usernameSignUp + "Balance"), 100.0f);
                GetComponent<Account>().LoadData(usernameSignUp.text);
                error.text = "";
            }
        }
        else
        {
            error.text = "Username or password cannot be empty!";
        }
    }

    public void LogIn()
    {
        if (usernameLogIn.text != "" && passwordLogIn.text != "")
        {
            if (PlayerPrefs.HasKey(usernameLogIn.text))
            {
                if (PlayerPrefs.GetString(usernameLogIn.text) == passwordLogIn.text)
                {
                    GetComponent<Account>().LoadData(usernameLogIn.text);
                    error.text = "";
                }
                else
                {
                    error.text = "Incorrect password!";
                }
            }
            else
            {
                error.text = "Incorrect username!";
            }
        }
        else
        {
            error.text = "Username or password cannot be empty!";
        }
    }

    public void HidePannels()
    {
		loginPanel.SetActive(false);
		signupPanel.SetActive(false);
	}

    public void ShowPannels()
    {
        SwitchToLogin();
    }
}
