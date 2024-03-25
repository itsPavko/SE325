using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public CustomButton[] customButtons;

    // Start is called before the first frame update
    void Start()
    {
        customButtons[0].Click();

		//Change(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Change(int n)
    {
		foreach (Transform child in transform)
		{
			int index = child.transform.GetSiblingIndex();
			if(index == n)
            {
                child.transform.gameObject.SetActive(true);
            }
            else
            {
                customButtons[index].ResetButton();
				child.transform.gameObject.SetActive(false);
			}
		}
	}
}
