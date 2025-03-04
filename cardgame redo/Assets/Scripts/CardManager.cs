﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum GameStatus { Start, Ready, End };
public class CardManager : MonoBehaviour
{
	public static GameStatus gameStatus;

	//Card Prefab
	[SerializeField]
	SimpleDrag cardPrefab;
	//UIs
	[SerializeField]
	TextMeshProUGUI text_Message;
	[SerializeField]
	TextMeshProUGUI text_Energy;

	//Important: card database
	CardDatabase cardDatabase;

	//Where to generate all the cards
	RectTransform cardGroup;

	//Player's hand cards
	List<SimpleDrag> cards_Hand = new List<SimpleDrag>();
	//Cards waiting for drawing
	List<CardProperty> cards_DrawPile = new List<CardProperty>();
	List<CardProperty> cards_DiscardPile = new List<CardProperty>();

	//If currently in the process of drawing card from the deck
	bool isDrawingCards;
	int cardDrawingIndex = 0;
	float cardDrawingTimer = 0;
	float cardDrawingTimerTotal = 0.2f;

	[HideInInspector]
	public SimpleDrag target;

	//Getter/Setter with "value"

	private int _drawCardNum = 0;
	public int DrawCardNum {
		get { return _drawCardNum; } 
		set { _drawCardNum = Mathf.Max(1, value); Debug.Log("Player's Draw Card Num is set to " + value); }
	}

	int currentEnergy = -1;
	int defaultEnergy = 23;



	// Use this for initialization
	void Start () {
		cardGroup = GameObject.Find("CardGroup").GetComponent<RectTransform>();
		cardDatabase = GameObject.Find("CardDatabase").GetComponent<CardDatabase>();

		//Create deck pool
		foreach(CardProperty cP in cardDatabase.cardData)
		{
			cards_DrawPile.Add(cP);
		}

		DrawCardNum = 5;

		RoundStart();
	}
	
	// Update is called once per frame
	void Update () {
		//Card shows up one by one
		if (isDrawingCards)
		{
			if (cardDrawingTimer > cardDrawingTimerTotal)
			{
				if(cards_DrawPile.Count == 0)
				{
					//Refill cards from discard pile
					foreach (CardProperty cP in cards_DiscardPile)
					{
						cards_DrawPile.Add(cP);
					}
				}
				CardProperty cardProperty = cards_DrawPile[Random.Range(0, cards_DrawPile.Count)];
				SimpleDrag card;

				//Generate Player card
				cardDrawingTimer = 0;

				card = Instantiate(cardPrefab); //creates a card
				RectTransform cardTrans = card.GetComponent<RectTransform>();
				cardTrans.SetParent(cardGroup);
				//Start point
				cardTrans.anchoredPosition = new Vector3(600, 0);

				card.Init(cardProperty, this);

				//Add the controller to cards_Hand
				cards_Hand.Add(card);
				cards_DrawPile.Remove(cardProperty);

				//Relocation
				RelocateAllCards();


				cardDrawingIndex++;
				if(cardDrawingIndex >= DrawCardNum)
				{
					gameStatus = GameStatus.Ready;
					//Finished drawing cards
					isDrawingCards = false;
				}
			}
			else
			{
				cardDrawingTimer += Time.deltaTime;
			}
		}
	}

	public void RelocateAllCards(SimpleDrag selectedCard = null)
	{
		if (cards_Hand.Count > 0)
		{
			//Player cards
			if (gameStatus == GameStatus.End)
			{
				for (int i = 0; i < cards_Hand.Count; i++)
				{
					//Round end, move all the cards to the left corner
					cards_Hand[i].targetPos = new Vector2(-Screen.width * 0.6f, -100);
					cards_Hand[i].targetRot = Quaternion.Euler(Vector3.zero);
				}
			}
			else if (cards_Hand.Count == 1)
			{
				//If only 1 hand card is left, keep it in the center of bottom screen
				cards_Hand[0].targetPos = Vector2.zero;
				cards_Hand[0].targetRot = Quaternion.Euler(Vector3.zero);
			}
			else
			{
				//Calculate interval
				float cardInterval = (Screen.width - 700) / (cards_Hand.Count - 1);
				//Between 30-150
				cardInterval = Mathf.Clamp(cardInterval, 30, 150);
				float middleIndex = (cards_Hand.Count - 1) * 0.5f;

				//Get current selected card's index
				int selectedCardIndex = 0;
				if (selectedCard != null) selectedCardIndex = cards_Hand.IndexOf(selectedCard);

				for (int i = 0; i < cards_Hand.Count; i++) //loop over all cards
				{
					SimpleDrag SimpleDrag = cards_Hand[i];
					//Get card anchor position
					Vector2 targetPos = new Vector2((i - middleIndex) * cardInterval, Mathf.Abs(middleIndex - i) * cardInterval * -0.2f);
					if (selectedCard != null)
					{
						//Move away from selected card
						if (i < selectedCardIndex)
						{
							targetPos.x -= 20000 / cardInterval;
						}
						else if (i > selectedCardIndex)
						{
							targetPos.x += 20000 / cardInterval;
						}
					}
					SimpleDrag.targetPos = targetPos;
					SimpleDrag.targetRot = Quaternion.Euler(0, 0, (middleIndex - i) * cardInterval * 0.08f);
				}
			}
		}
	}

	//Start Round
	public void RoundStart()
	{
		gameStatus = GameStatus.Start;
		cards_Hand.Clear();
		//Get new cards
		cardDrawingIndex = 0;
		//cards_DrawPile = cards;
		isDrawingCards = true;

		//Display UI text
		text_Message.text = currentEnergy == -1 ? "~ Welcome ~" : "Next Round Starts !";
		currentEnergy = defaultEnergy;
		text_Energy.text = currentEnergy.ToString();
	}
	
	//End Round
	public void RoundEnd()
	{
		if (gameStatus == GameStatus.Ready)
		{
			gameStatus = GameStatus.End;
			if (cards_Hand.Count > 0)
			{
				//Discard all the hand cards
				foreach (SimpleDrag SimpleDrag in cards_Hand)
				{

					cards_DiscardPile.Add(SimpleDrag.cardProperty);
				}
			}

			RelocateAllCards();
			Invoke("CleanHandsAndStartNextRound", 0.6f);
		}
	}

	void CleanHandsAndStartNextRound()
	{
		//Round end of special card ability
		foreach(SimpleDrag SimpleDrag in cards_Hand)
		{
			Destroy(SimpleDrag.gameObject);
		}
		cards_Hand.Clear();

		//Next Round
		RoundStart();
	}

	//Applying card effect
	public bool TakingEffectCheck()
	{
		if (target != null)
		{
			CardProperty cardProperty = target.cardProperty;
			//For special card, destroyed after use
			if (target.rectTrans.anchoredPosition.y > 300 && currentEnergy >= target.cardProperty.cost)
			{
				//Normal card, drag to the center of screen
				currentEnergy -= target.cardProperty.cost;
				//Apply card ability
				switch (cardProperty.name)
				{
					case "Minotaur":
						print("Applying Special Ability 1");
						break;
					case "Dark Protector":
						print("Applying Special Ability 2");
						break;
					case "The Prophecy":
						print("Applying Special Ability 3");
						break;
					case "Space Tunnel":
						print("Applying Special Ability 4");
						break;
					case "Space Secret Army":
						print("Applying Special Ability 5");
						break;
					default:
						print("Applying Normal Card");
						break;
				}
				//This card is used
				//Add this card to the discard pile
				MoveCardToDiscardPile(target);

				text_Message.text = "You just used \"" + target.cardProperty.name + "\" Card";
				text_Energy.text = currentEnergy.ToString();


				return true;
			}
		}
		return false;
	}

	//After using a card, discord it
	void MoveCardToDiscardPile(SimpleDrag SimpleDrag)
	{
		cards_DiscardPile.Add(SimpleDrag.cardProperty);
		cards_Hand.Remove(SimpleDrag);
		Destroy(SimpleDrag.gameObject);

		RelocateAllCards();
	}

}
