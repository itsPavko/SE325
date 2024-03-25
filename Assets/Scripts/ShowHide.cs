using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
public class ShowHide : MonoBehaviour
{
    public TMP_InputField password;
    public bool show;
    // Start is called before the first frame update
    void Start()
    {
        //password = transform.GetComponent<TMP_InputField>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Toggle()
    {
        show = !show;
        if (show)
        {
            password.contentType = TMP_InputField.ContentType.Standard;
		}
        else
        {
            password.contentType = TMP_InputField.ContentType.Password;
		}

        password.ForceLabelUpdate();

	}
}
