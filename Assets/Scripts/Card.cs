using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{

    [HideInInspector] public int id; // Identificador que a carta recebe
    public Sprite cardBack;
    [HideInInspector] public Sprite cardFront;

    private Image image;
    private Button button;

    private bool isFlippingOpen;
    private bool isFlippingClose;
    private bool flipped; // true == cardFront
    private float FlipAmount = 1;

    public float flipSpeed = 4; // Velocidade de giro da carta

    // Start is called before the first frame update
    void Start()
    {   //Inicialização da imagens e dos botões.
        image = GetComponent<Image>();
        button = GetComponent<Button>();
    }

    //Vira a carta através do click
    public void FlipCard()
    {
        if(CardManager.instance.choise1 == 0) // Verifica se tem alguma carta apresentada
        {
            CardManager.instance.choise1 = id;
            CardManager.instance.AddChoosenCard(this.gameObject);

            isFlippingOpen = true;
            StartCoroutine(FlipOpen());

            //Bloqueia o clique sobre uma mesma carta.
            button.interactable = false;

            //Apresenta a dica de onde está a carta a ser combinada.
            CardManager.instance.ApresentarDica();

            //Registra o tempo da virada da primeira carta
            CardManager.instance.RegistraTempoPrimeiraCarta();

            //Apresenta a descrição da carta no personagem
            CardManager.instance.ApresentaDescricaoCartaSelecionada(CardManager.instance.choise1.ToString());

        }
        else if (CardManager.instance.choise2 == 0) // Verifica se tem alguma carta apresentada
        {
            CardManager.instance.choise2 = id;
            CardManager.instance.AddChoosenCard(this.gameObject);

            isFlippingOpen = true;
            StartCoroutine(FlipOpen());

            //Bloqueia o clique sobre uma mesma carta.
            button.interactable = false;

            //Compara as cartas
            StartCoroutine(CardManager.instance.CompareCards());

            //Registra o tempo da virada da segunda carta
            CardManager.instance.RegistraTempoSegundaCarta();
        }
    }

    //Apresenta a carta conforme o tempo
    IEnumerator FlipOpen()
    {
        while (isFlippingOpen && FlipAmount > 0)
        {
            FlipAmount -= Time.deltaTime * flipSpeed;
            FlipAmount = Mathf.Clamp01(FlipAmount);
            transform.localScale = new Vector3(FlipAmount, transform.localScale.y, transform.localScale.z);
            if(FlipAmount <= 0)
            {
                image.sprite = cardFront;
                isFlippingOpen = false;
                isFlippingClose = true;
            }

            yield return null;
        }

        while (isFlippingClose && FlipAmount < 1)
        {
            FlipAmount += Time.deltaTime * flipSpeed;
            FlipAmount = Mathf.Clamp01(FlipAmount);
            transform.localScale = new Vector3(FlipAmount, transform.localScale.y, transform.localScale.z);
            if (FlipAmount >= 1)
            {
                isFlippingClose = false;
            }

            yield return null;
        }
    }

    //Esconde a Carta
    IEnumerator FlipClose()
    {
        while (isFlippingOpen && FlipAmount > 0)
        {
            FlipAmount -= Time.deltaTime * flipSpeed;
            FlipAmount = Mathf.Clamp01(FlipAmount);
            transform.localScale = new Vector3(FlipAmount, transform.localScale.y, transform.localScale.z);
            if (FlipAmount <= 0)
            {
                image.sprite = cardBack;
                isFlippingOpen = false;
                isFlippingClose = true;
            }

            yield return null;
        }

        while (isFlippingClose && FlipAmount < 1)
        {
            FlipAmount += Time.deltaTime * flipSpeed;
            FlipAmount = Mathf.Clamp01(FlipAmount);
            transform.localScale = new Vector3(FlipAmount, transform.localScale.y, transform.localScale.z);
            if (FlipAmount >= 1)
            {
                isFlippingClose = false;
            }

            yield return null;
        }

        button.interactable = true;
    }

    //Fecha a carta conforme o tempo
    public void CloseCard()
    {
        isFlippingOpen = true;
        StartCoroutine(FlipClose());
    }
}


