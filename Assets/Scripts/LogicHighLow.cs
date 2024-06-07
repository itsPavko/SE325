using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;
using UnityEngine.UI;



public class Card
{
    public string value;
	public string suit;
    public int valueInt;
}

public class LogicHighLow : MonoBehaviour
{
	public float bet;
	public float outcome;
	private bool isFirst = true;

	public Card curCard;
	public Card newCard;
	public int choice; // 0-lower, 1-higher



	public TextMeshProUGUI value;
	public Image suit1;
	public Image suit2;
	public Sprite[] suits;
	public Color[] colors;
	private Color curColor;
	public TMP_InputField betInput;
	public Account account;

	private bool canBet = true;
	public GameObject warningText;
	public GameObject warningImage;
	public Image betHolder;

	public GameObject historyItem;
	public GameObject historyHolder;
	public Sprite high;
	public Sprite low;

	// Start is called before the first frame update
	void Start()
	{
		bet = 1;
		curCard = GenerateNewCard();
		newCard = curCard;
		UpdateUI();
		GenHistoryCard(isFirst);
		UpdateBetHolder();
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void MakeChoice(int n)
	{
		if (!canBet)
		{
			return;
		}
		choice = n;
		newCard = GenerateNewCard();
		account.UpdateBalance(-bet);

		UpdateUI();

		if (choice == 0) //Picked lower
		{
			if (newCard.valueInt >= curCard.valueInt)
			{
				Lose();
			}
			else
			{
				Win();
			}
		}
		else if (choice == 1) //Picked higher
		{
			if (newCard.valueInt <= curCard.valueInt)
			{
				Lose();
			}
			else
			{
				Win();
			}
		}

		UpdateBetHolder();
		curCard = newCard;
		GenHistoryCard(isFirst);
	}

	public void GenHistoryCard(bool first)
	{
		GameObject go = Instantiate(historyItem);
		go.transform.SetParent(historyHolder.transform);

		go.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = curCard.value;

		switch (curCard.suit)
		{
			case "H":
				go.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = suits[0];
				break;
			case "D":
				go.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = suits[1];
				break;
			case "C":
				go.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = suits[2];
				break;
			case "S":
				go.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = suits[3];
				break;
		}

		if (first)
		{
			go.transform.GetChild(1).gameObject.SetActive(false);
			go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Start";
		}
		else 
		{
			if(choice == 0)
			{
				go.transform.GetChild(1).GetComponent<Image>().sprite = low;
			}
			else
			{
				go.transform.GetChild(1).GetComponent<Image>().sprite = high;
			}
			
			if (outcome > 0)
			{
				go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "$" + outcome;
			}
			else
			{
				go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "-$" + MathF.Abs(outcome);
			}
		}

		isFirst = false;
	}

	public void UpdateUI()
	{
		switch (newCard.suit)
		{
			case "H":
				suit1.sprite = suits[0];
				suit2.sprite = suits[0];
				break;
			case "D":
				suit1.sprite = suits[1];
				suit2.sprite = suits[1];
				break;
			case "C":
				suit1.sprite = suits[2];
				suit2.sprite = suits[2];
				break;
			case "S":
				suit1.sprite = suits[3];
				suit2.sprite = suits[3];
				break;
		}
		suit1.color = curColor;
		suit2.color = curColor;
		value.color = curColor;
		value.text = newCard.value;


	}

	public void Lose()
	{
		outcome = -bet;
		Debug.Log("You lose " + bet);
	}

	public void Win()
	{
		outcome = bet * 1.8f;
		Debug.Log("You win " + (bet * 1.8f));
		account.UpdateBalance(bet * 1.8f);
	}


	public Card GenerateNewCard()
	{
		Card card = new Card();
		int n = Random.Range(0, 13);
		int m = Random.Range(0, 4);

		switch (n)
		{
			case 0:
				card.value = "A";
				break;
			case 1:
				card.value = "2";
				break;
			case 2:
				card.value = "3";
				break;
			case 3:
				card.value = "4";
				break;
			case 4:
				card.value = "5";
				break;
			case 5:
				card.value = "6";
				break;
			case 6:
				card.value = "7";
				break;
			case 7:
				card.value = "8";
				break;
			case 8:
				card.value = "9";
				break;
			case 9:
				card.value = "10";
				break;
			case 10:
				card.value = "J";
				break;
			case 11:
				card.value = "Q";
				break;
			case 12:
				card.value = "K";
				break;
		}

		switch (m)
		{
			case 0:
				card.suit = "H";
				curColor = colors[0];
				break;
			case 1:
				card.suit = "D";
				curColor = colors[0];
				break;
			case 2:
				card.suit = "C";
				curColor = colors[1];
				break;
			case 3:
				card.suit = "S";
				curColor = colors[1];
				break;
		}

		card.valueInt = n;
		Debug.Log("Drew card: " + card.value + card.suit);
		return card;
	}

	public void UpdateBet()
	{

		if (float.TryParse(betInput.text, out float newBet))
		{
			bet = newBet;
		}
		else
		{
			bet = 0;
			betInput.text = bet.ToString("0.00");
			betInput.ReleaseSelection();
		}

		UpdateBetHolder();
	}

	public void EndEditBet()
	{
		//string formated = String.Format("{0:F2}", betInput.text);
		if (float.TryParse(betInput.text, out float newBet))
		{
			bet = newBet;
			betInput.text = bet.ToString("0.00");
		}
		else
		{
			betInput.ReleaseSelection();
			bet = 0;
			betInput.text = bet.ToString("0.00");
			//betInput.ReleaseSelection();
		}

		UpdateBetHolder();
	}

	public void UpdateBetHolder()
	{
		if (bet > account.balance)
		{
			canBet = false;
			warningImage.SetActive(true);
			warningText.SetActive(true);
			betHolder.color = colors[2];
			warningText.GetComponent<TextMeshProUGUI>().text = "Insufficient balance!";
		}
		else if (bet <= 0)
		{
			canBet = false;
			warningImage.SetActive(true);
			warningText.SetActive(true);
			betHolder.color = colors[2];
			warningText.GetComponent<TextMeshProUGUI>().text = "Bet cannot be less than zero!";
		}
		else
		{
			canBet = true;
			warningImage.SetActive(false);
			warningText.SetActive(false);
			betHolder.color = colors[1];
		}
	}

}
