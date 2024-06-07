using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using TMPro;
using System;

public class LogicBlackjack : MonoBehaviour
{
	[Serializable]
	public class Card
	{
		public string value;
		public string suit;
		public int valueInt;
	}

	public Sprite[] suits;
	public Color[] colors;
	private Color curColor;

	private Card curCard;
	public List<Card> dealerCards = new List<Card>();
	public List<Card> playerCards = new List<Card>();

	public float bet;

	public int dealerScore;
	public int playerScore;

	public GameObject cardPrefab;

	private GameObject cover;

	public GameObject dealerCardsHolder;
	public GameObject playerCardsHolder;

	public GameObject startButton;
	public GameObject twoButtonHolder;
	/*
	public GameObject splitHolder;
	public GameObject doubleHolder;
	*/

	public TextMeshProUGUI dealerScoreText;
	public TextMeshProUGUI playerScoreText;

	public TMP_InputField betInput;
	private bool canBet = true;
	public GameObject warningText;
	public GameObject warningImage;
	public GameObject lockImage;
	public Image betHolder;

	public TextMeshProUGUI outcome;

	public Account account;


	void Start()
	{
		outcome.text = "";
		bet = 1;
		UpdateBetHolder();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.N))
		{
			ClearBoard();
			StartRound();
		}
	}

	public void ClearBoard()
	{
		playerScore = 0;
		dealerScore = 0;
		playerScoreText.text = "";
		dealerScoreText.text = "";

		foreach (Transform child in playerCardsHolder.transform)
		{
			GameObject.Destroy(child.gameObject);
		}

		foreach (Transform child in dealerCardsHolder.transform)
		{
			GameObject.Destroy(child.gameObject);
		}

		dealerCards.Clear();
		playerCards.Clear();

	}

	public void StartRound()
	{
		if (canBet)
		{
			outcome.text = "";
			account.UpdateBalance(-bet);
			startButton.SetActive(false);
			lockImage.SetActive(true);
			StartCoroutine(FirstDraw());
		}
	}

	public IEnumerator FirstDraw()
	{


		bool h = true;
		curCard = GenerateNewCard(h);
		dealerCards.Add(curCard);
		DealCard(h, 0, curCard);

		h = false;
		yield return new WaitForSeconds(0.4f);

		curCard = GenerateNewCard(h);
		playerCards.Add(curCard);
		DealCard(h, 1, curCard);

		yield return new WaitForSeconds(0.4f);

		curCard = GenerateNewCard(h);
		dealerCards.Add(curCard);
		DealCard(h, 0, curCard);

		yield return new WaitForSeconds(0.4f);

		curCard = GenerateNewCard(h);
		playerCards.Add(curCard);
		DealCard(h, 1, curCard);

		yield return new WaitForSeconds(0.4f);

		twoButtonHolder.SetActive(true);

		int n = playerCards[0].valueInt + playerCards[1].valueInt;

		/*
		if (playerCards[0].valueInt == playerCards[1].valueInt)
		{
			splitHolder.SetActive(true);
		}

		
		if (n == 9 || n == 10 || n == 11) 
		{
			doubleHolder.SetActive(true);
		}
		*/

		dealerScoreText.text = "?";
		playerScore = n;
		CheckForAces(1);
		playerScoreText.text = playerScore.ToString();
		if (playerScore == 21)
		{
			playerScoreText.text = "Blackjack";
			Win();
		}
	}

	public void CheckForAces(int p)
	{
		if (p == 0)
		{
			if (dealerScore > 21)
			{
				foreach (var x in dealerCards)
				{
					if (x.value == "A")
					{
						dealerScore -= 10;
						x.value = "1";
						break;
					}
				}
			}
		}
		else if (p == 1)
		{
			if (playerScore > 21)
			{
				foreach (var x in playerCards)
				{
					if (x.value == "A")
					{
						x.value = "1";
						playerScore -= 10;
						break;
					}
				}
			}
		}

	}

	public void Hit()
	{
		curCard = GenerateNewCard(false);
		playerCards.Add(curCard);
		DealCard(false, 1, curCard);
		playerScore += curCard.valueInt;
		CheckForAces(1);
		playerScoreText.text = playerScore.ToString();
		if (playerScore == 21)
		{
			playerScoreText.text = "Blackjack";
		}
		if (playerScore > 21)
		{
			Lose();
		}
	}

	public void Stand()
	{
		twoButtonHolder.SetActive(false);
		cover.SetActive(false);
		dealerScore = dealerCards[0].valueInt + dealerCards[1].valueInt;
		CheckForAces(0);
		dealerScoreText.text = dealerScore.ToString();
		if (dealerScore == 21)
		{
			dealerScoreText.text = "Blackjack";
		}
		StartCoroutine(CheckState());
	}

	public IEnumerator CheckState()
	{
		if (dealerScore == 21)
		{
			dealerScoreText.text = "Blackjack";
			if (playerScore == 21)
			{
				Draw();
			}
			else
			{
				Lose();
			}
		}
		else if (dealerScore > 21)
		{
			Win();
		}
		else if (dealerScore < 17)
		{
			yield return new WaitForSeconds(0.6f);
			curCard = GenerateNewCard(false);
			dealerCards.Add(curCard);
			DealCard(false, 0, curCard);
			dealerScore += curCard.valueInt;
			CheckForAces(0);
			dealerScoreText.text = dealerScore.ToString();
			StartCoroutine(CheckState());
		}
		else
		{
			if(dealerScore > playerScore)
			{
				Lose();
			}
			else if (dealerScore < playerScore)
			{
				Win();
			}
			else if(dealerScore == playerScore)
			{
				Draw();
			}
		}
	}

	public void Draw()
	{
		twoButtonHolder.SetActive(false);
		startButton.SetActive(true);
		account.UpdateBalance(bet);
		outcome.text = "Draw";
	}

	public void Lose()
	{
		twoButtonHolder.SetActive(false);
		startButton.SetActive(true);
		lockImage.SetActive(false);
		outcome.text = "You Lose";
	}

	public void Win()
	{
		twoButtonHolder.SetActive(false);
		startButton.SetActive(true);
		lockImage.SetActive(false);
		account.UpdateBalance(bet * 2f);
		outcome.text = "You Win";
	}
	public void DealCard(bool hidden, int p, Card card) // p - 0=dealer 1=player
	{
		GameObject go = Instantiate(cardPrefab);

		if (p == 0)
		{
			go.transform.SetParent(dealerCardsHolder.transform);
		}
		else
		{
			go.transform.SetParent(playerCardsHolder.transform);
		}

		go.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = card.value;
		switch (card.suit)
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

		if(hidden)
		{
			cover = go.transform.GetChild(0).GetChild(2).gameObject;
			cover.SetActive(true);
		}


	}

	public Card GenerateNewCard(bool hidden)
	{
		Card card = new Card();
		int n = Random.Range(0, 13);
		int m = Random.Range(0, 4);

		switch (n)
		{
			case 0:
				card.value = "A";
				card.valueInt = 11;
				break;
			case 1:
				card.value = "2";
				card.valueInt = 2;
				break;
			case 2:
				card.value = "3";
				card.valueInt = 3;
				break;
			case 3:
				card.value = "4";
				card.valueInt = 4;
				break;
			case 4:
				card.value = "5";
				card.valueInt = 5;
				break;
			case 5:
				card.value = "6";
				card.valueInt = 6;
				break;
			case 6:
				card.value = "7";
				card.valueInt = 7;
				break;
			case 7:
				card.value = "8";
				card.valueInt = 8;
				break;
			case 8:
				card.value = "9";
				card.valueInt = 9;
				break;
			case 9:
				card.value = "10";
				card.valueInt = 10;
				break;
			case 10:
				card.value = "J";
				card.valueInt = 10;
				break;
			case 11:
				card.value = "Q";
				card.valueInt = 10;
				break;
			case 12:
				card.value = "K";
				card.valueInt = 10;
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
