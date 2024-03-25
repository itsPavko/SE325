using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Login : MonoBehaviour
{

    public GameObject loginPanel;
    public GameObject signupPanel;
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
}
