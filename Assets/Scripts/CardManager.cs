using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{

    public static CardManager instance; // Cria uma instância do objeto de jogo cardManager para ser invocado a qualquer hora do jogo.

    //Lista das imagens a serem usadas no jogo
    public List<Sprite> SpriteList = new List<Sprite>();

    [SerializeField]private List<GameObject> buttonList = new List<GameObject>();
    [SerializeField] private List<GameObject> hiddenButtonList = new List<GameObject>();

    private List<GameObject> choosenCards = new List<GameObject>();

    private int lastMatchId;
    [SerializeField]private bool choosen;

    [Header("Quantos pares você deseja jogar?")]
    public int pairs;
    [Header("Card Prefab Button")]
    public GameObject cardPrefab;
    [Header("The Parent Spacer to sort Cards in")]
    public Transform spacer;

    //particle FX
    [Header("Pontutação básica por partida")]
    public int matchScore = 100;

    public int choise1;
    public int choise2;

    [Header("Effects")]
    public GameObject fxExplosion;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        FillPlayField();    
    }

    //Preenche e criar as cartas no espaço de jogo conforme a quantidade de pares.
    void FillPlayField()
    {
        for (int i = 0; i < (pairs*2); i++)
        {
            GameObject newCard = Instantiate(cardPrefab, spacer);
            buttonList.Add(newCard);
            hiddenButtonList.Add(newCard);
        }

        ShuffleCards();

    }

    void ShuffleCards()
    {
        int num = 0;
        int cardPairs = buttonList.Count / 2;

        for (int i = 0; i < cardPairs; i++)
        {
            num++;
            for (int j = 0; j < 2; j++)// contagem de a quantidade cartas por partidas
            {
                int cardIndex = Random.Range(0, buttonList.Count);
                Card tempCard = buttonList[cardIndex].GetComponent<Card>();
                tempCard.id = num;
                tempCard.cardFront = SpriteList[num - 1];

                buttonList.Remove(buttonList[cardIndex]);
            }
        }
    }

    public void AddChoosenCard(GameObject card)
    {
        choosenCards.Add(card);
    }

    public IEnumerator CompareCards()
    {
        if(choise2 == 0 || choosen)
        {
            yield break;
        }
        choosen = true;
        yield return new WaitForSeconds(1.5f);//tempo que a carta é exibida

        //No Match! =(
        if((choise1 != 0 && choise2 != 0) && (choise1 != choise2))
        {
            //vira as cartas abertas
            FlipAllBack();

            //reinicia o combo e a ponutação.
            ScoreManager.instance.ResetCombo();

        }
        else if ((choise1 != 0 && choise2 != 0) && (choise1 == choise2))
        {
            lastMatchId = choise1;
            //Adiciona a pontuação
            ScoreManager.instance.AddScore(matchScore);

            //Remove os pares da partida
            RemoveMatch();

            //limpa a lista de cartas
            choosenCards.Clear();
        }
        //Reinicia todas as escolhas
        choise1 = 0;
        choise2 = 0;
        choosen = false;

        //Checa se venceu a partida
        CheckWin();
    }

    // Vira todas as cartas
    void FlipAllBack()
    {
        foreach(GameObject card in choosenCards)
        {
            card.GetComponent<Card>().CloseCard();
        }
        choosenCards.Clear();
    }

    void RemoveMatch()
    {
        for (int i = hiddenButtonList.Count - 1; i >= 0; i--)
        {
            Card tempCard = hiddenButtonList[i].GetComponent<Card>();
            if(tempCard.id == lastMatchId) // compara os Id das cartas encontradas para removê-las
            {
                //Particle FX
                Instantiate(fxExplosion, hiddenButtonList[i].transform.position + new Vector3(0,0,-1), Quaternion.identity);

                //Oculta a carta que teve o par combinado...
                hiddenButtonList[i].GetComponent<UnityEngine.UI.Image>().enabled = false;

                //Remove a carta
                hiddenButtonList.RemoveAt(i);
            }
        }
    }

    void CheckWin()
    {
        if(hiddenButtonList.Count < 1)
        {
            //Para a contagem de tempo
            ScoreManager.instance.StopTime();
            //Show UI

            //Fireworks

            //Show Stars
            Debug.Log("Você venceu!");
        }

    }
}
