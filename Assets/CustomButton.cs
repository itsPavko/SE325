using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public UnityEvent onClick;
	public Color Base;
	public Color Hover;
	public Color Selected;
	public bool isSelected = false;
	public bool isHover = false;


	void Awake()
	{
		if (onClick == null)
		{
			onClick = new UnityEvent();
		}

		Base = GetComponent<Image>().color;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			if (isHover && !isSelected)
			{
				Click();
			}
		}
	}

	public void Click()
	{
		onClick.Invoke();
		isSelected = true;
		GetComponent<Image>().color = Selected;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if(!isSelected)
		{
			GetComponent<Image>().color = Hover;
			isHover = true;
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (!isSelected)
		{
			GetComponent<Image>().color = Base;
			isHover = false;
		}
	}

	public void ResetButton()
	{
		isHover = false;
		isSelected = false;
		GetComponent<Image>().color = Base;
	}
}
