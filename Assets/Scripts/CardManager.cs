using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{

    public static CardManager instance; // Cria uma instância do objeto de jogo cardManager para ser invocado a qualquer hora do jogo.

    //Lista das imagens a serem usadas no jogo
    public List<Sprite> SpriteList = new List<Sprite>();

    [SerializeField] private List<GameObject> buttonList = new List<GameObject>();
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
    public GameObject fxExplosionSquare; // Efeito que indica a dica do par correto.

    private int iQuantidadeCartas;
    private GridLayoutGroup glg;

    private string sSugeridoPar;

    //Registra as sequencias de ID das cartas apresentadas na partida
    List<string> sequenciaCartasID = new List<string>();

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        FillPlayField();

        RedimensionarGridCartas(pairs);

        //Desabilita a mensagem do jogo
        ScoreManager.instance.mensagemText.enabled = true;
        ScoreManager.instance.mensagemText.text = "Clique sobre as cartas para combinar os pares.";

        //Variável para registrar o Evento se o jogo não for finalizado, clicando em Reiniciar.
        ScoreManager.instance.jogoFinalizadoText.enabled = false;

        //Apresenta todas as cartas no início do jogo
        StartCoroutine(FlipAllCardsFront());

        //Inicializa a variável da dica de Par sugerido:
        sSugeridoPar = "";
    }

    //Preenche e criar as cartas no espaço de jogo conforme a quantidade de pares.
    void FillPlayField()
    {
        //Informe aqui a quantidade de pares a serem utilizados pelo usuário durante o jogo - níveis de dificuldade
        int valorDropdown = 0;
        //pairs = 2;        

        valorDropdown = PlayerPrefs.GetInt("ValueDropdownQtdeCartas"); //MenuConfiguracao.instance.DropdownQuantidadeCartas.value;      
        /*        
        for (int i = 0; i <= valorDropdown; i++)
        {
            Debug.Log("valorDropdown "+ valorDropdown.ToString());
            //Atribui a quantidade de pares no tabuleiro e divide por 2 por causa das opções do dropdown
            pairs = int.Parse(MenuConfiguracao.instance.DropdownQuantidadeCartas.options[i].text) / 2;
        }*/
        //Amostra para o ML do KNN - Ver aula 66 do Udemy Classificação com o K-Nearest Neighbors
        //$samples = [ [1, 2 ], [2, 4 ], [4, 8], [8, 16 ] ]; // 1 par de carta podendo ter apenas 2 erros, 2 pares de carta podendo ter apenas 4 erros...
        //$labels  = [0, 1, 2, 3]; // 0 - Super Fácil, 1 - Fácil, 2 - Médio e 3 - Difícil

        switch (valorDropdown)
        {
            case 0:
                pairs = 1;
                break;
            case 1:
                pairs = 2;
                break;
            case 2:
                pairs = 4;
                break;
            case 3:
                pairs = 8;
                break;
        }

        for (int i = 0; i < (pairs*2); i++)
        {
            GameObject newCard = Instantiate(cardPrefab, spacer);
            buttonList.Add(newCard);
            hiddenButtonList.Add(newCard);  
        }

        // Armezena a quantidade de cartas para apresentar as mensagens pelo personagem.
        iQuantidadeCartas = pairs * 2;

        ShuffleCards();
    }

    //Redimensiona as cartas conforme a quantidade de pares.
    public void RedimensionarGridCartas(int iQuantidadePares)
    {
        glg = spacer.GetComponent<GridLayoutGroup>();
        //Informar a dimensão do GridLayout
        //Para 3 pares: [Top = 10; Cell Size {X = 160, Y = 200}; Spacing {X = 20, Y = 20}
        if (iQuantidadePares == 3)
        {
            glg.padding.top = 10;
            glg.cellSize = new Vector2(160, 200);
            glg.spacing = new Vector2(20, 20);
        }
    }

    //Embaralha as cartas do jogo
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

    //função que obtém a posição dos pares selecionados.
    //Verificar se vale a pena buscar a posição no vetor dos IDs das cartas. 
    //Nos primeiros testes como a comparação das cartas é feita por ID, a função abaixo localizava duas vezes.
    //E o retorno ficava errado porque o ID retornado não era da da carta clicada, mas de todas que possuem o mesmo ID.
    //Verificar a possibilidade de verificar pelo botão que foi clicado.
    /*
    public void ObterPosicaoParSelecionado(int primeiraCartaID, int segundaCartaID)
    {
        int iPosicaoVetorPrimeiraCarta = 0; // Posição da primeira carta
        int iPosicaoVetorSegundaCarta = 0; // Posição da segunda carta

        for (int i = 0; i < hiddenButtonList.Count; i++)
        {
            Card tempCard = hiddenButtonList[i].GetComponent<Card>();

            if ((tempCard.id == primeiraCartaID) && (iPosicaoVetorPrimeiraCarta == 0) ) // compara os Id das cartas encontradas para indicar a posição delas no jogo.
            {
                iPosicaoVetorPrimeiraCarta = i;
                Debug.Log("iPosicaoVetorPrimeiraCarta: " + iPosicaoVetorPrimeiraCarta.ToString());
            }

            if ((tempCard.id == segundaCartaID) && (iPosicaoVetorSegundaCarta == 0))
            {
                iPosicaoVetorSegundaCarta = i;
                Debug.Log("iPosicaoVetorSegundaCarta: " + iPosicaoVetorSegundaCarta.ToString());
            }

        }

        if ((iPosicaoVetorPrimeiraCarta != 0) && (iPosicaoVetorSegundaCarta != 0))
        {
            //ScoreManager.instance.AddPosicaoParSugeridoID("[" + iPosicaoVetorPrimeiraCarta.ToString() + "," + iPosicaoVetorSegundaCarta.ToString() + "]");
            Debug.Log("Posição Par selecionado: [" + iPosicaoVetorPrimeiraCarta.ToString() + "," + iPosicaoVetorSegundaCarta.ToString() + "]");
            iPosicaoVetorPrimeiraCarta = 0;
            iPosicaoVetorSegundaCarta = 0;
        }
    }
    */

    public void ApresentarDica()
    {
        //Funcionou, mas é uma boa utilizar variáveis para zerar os acertos / erros aqui neste procedimento
        //para que não fique repetindo toda hora as dicas porque os acertos e erros podem ser aumentados ou diminuído conforme as jogadas.

        bool bCartaVirada = ScoreManager.instance.todasPrimeirasCartasSelecionadasID.Contains(choise1.ToString());
        //int iAdicaoParSugerido = 0; // Controla a inclusão do par sugerido que deverá ser incluído no vetor de sugestão, apenas uma vez.
        int iPosicaoVetorPrimeiraCarta = 0; // Posição da primeira carta
        int iPosicaoVetorSegundaCarta = 0; // Posição da segunda carta

        sSugeridoPar = "";

        if (ScoreManager.instance.qtdeErro > ScoreManager.instance.qtdeAcerto + pairs)
        {
            for (int i = 0; i < hiddenButtonList.Count; i++)
            {
                
                Card tempCard = hiddenButtonList[i].GetComponent<Card>();

                if ((tempCard.id == choise1) && (bCartaVirada)) // compara os Id das cartas encontradas para removê-las e a variável "bCartaVirada" se a carta já foi virada
                {
                    //Particle FX
                    //Tentar colocar um outro tipo de componente para dar a dica da carta correta.
                    Instantiate(fxExplosionSquare, hiddenButtonList[i].transform.position + new Vector3(0, 0, -20), Quaternion.identity);

                    if (iPosicaoVetorPrimeiraCarta == 0)
                    {
                        iPosicaoVetorPrimeiraCarta = i;
                    }

                    if ((iPosicaoVetorPrimeiraCarta != i) && (iPosicaoVetorSegundaCarta == 0))
                    {
                        iPosicaoVetorSegundaCarta = i;
                    }

                    if ((iPosicaoVetorPrimeiraCarta != 0) && (iPosicaoVetorSegundaCarta != 0))
                    {
                        ScoreManager.instance.AddPosicaoParSugeridoID("[" + iPosicaoVetorPrimeiraCarta.ToString() + "," + iPosicaoVetorSegundaCarta.ToString() + "]");
                        iPosicaoVetorPrimeiraCarta = 0;
                        iPosicaoVetorSegundaCarta = 0;
                    }

                    //O par sugerido e que for acertado por meio da sugestão será salvo com um asterico
                    sSugeridoPar = "*";
                }
            }
        }
    }

    // Função para encapsular o acesso via Card.cs e não precisar importar a classe ScoreManager.cs.
    public void RegistraTempoPrimeiraCarta()
    {
        ScoreManager.instance.RegistraTempoPrimeiraCartaVirada();
    }

    public void RegistraTempoSegundaCarta()
    {
        ScoreManager.instance.RegistraTempoSegundaCartaVirada();
    }

    public void ApresentaDescricaoCartaSelecionada(string primeiraCartaId)
    {
        ScoreManager.instance.ObterDescricaoParSelecionadoID(primeiraCartaId);
    }


    public IEnumerator CompareCards()
    {
        if(choise2 == 0 || choosen)
        {
            yield break;
        }
        choosen = true;
        yield return new WaitForSeconds(1.5f);//tempo que a carta é exibida        

        //Debug.Log("No evento CompareCards -> choise1: " + choise1.ToString() + " choise2: " + choise2.ToString());

        ScoreManager.instance.AddParSelecionadoID(choise1.ToString(), choise2.ToString(), "[" + choise1.ToString() + "," + choise2.ToString() + "]");

        //No Match! =(
        if ((choise1 != 0 && choise2 != 0) && (choise1 != choise2))
        {
            //vira as cartas abertas
            FlipAllBack();

            //reinicia o combo e a ponutação.
            ScoreManager.instance.ResetCombo();

            //Soma a quantidade de cartas viradas erradas.
            ScoreManager.instance.SomarQtdeErro();

            //Adiciona os pares de cartas errados selecionados:
            ScoreManager.instance.AddParSelecionadoErradoID(choise1.ToString(), choise2.ToString(), "[" +choise1.ToString()+","+ choise2.ToString()+"]");

        }
        else if ((choise1 != 0 && choise2 != 0) && (choise1 == choise2))
        {
            lastMatchId = choise1;
            //Adiciona a pontuação
            ScoreManager.instance.AddScore(matchScore);

            //Soma a quantidade de cartas viradas corretamente.
            ScoreManager.instance.SomarQtdeAcerto();

            //Adiciona os pares de cartas errados selecionados.
            //É enviado apenas uma das variáveis porque as duas tem o mesmo Id.
            ScoreManager.instance.AddParSelecionadoCertoID(choise1.ToString(), choise2.ToString(), sSugeridoPar + "[" + choise1.ToString() + "," + choise2.ToString() + "]");

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

    // Apresenta todas as cartas no início do jogo
    public IEnumerator FlipAllCardsFront()
    {
        for (int i = 0; i < hiddenButtonList.Count; i++)
        {
            Card tempCard = hiddenButtonList[i].GetComponent<Card>();
            hiddenButtonList[i].GetComponent<UnityEngine.UI.Image>().sprite = tempCard.cardFront;
        }

        yield return new WaitForSeconds(3.5f);//tempo que a carta é exibida

        for (int i = 0; i < hiddenButtonList.Count; i++)
        {
            Card tempCard = hiddenButtonList[i].GetComponent<Card>();
            hiddenButtonList[i].GetComponent<UnityEngine.UI.Image>().sprite = tempCard.cardBack;
            sequenciaCartasID.Add(tempCard.id.ToString());
            //Debug.Log(tempCard.id);
        }

        ScoreManager.instance.AddSequenciaCartaID(sequenciaCartasID);

        //Inicia o temporizador após apresentar as cartas
        ScoreManager.instance.StartTime();
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

                //Debug.Log("Posição no HiddenbuttonList: "+hiddenButtonList[i].transform.position);

                //Oculta a carta que teve o par combinado...
                hiddenButtonList[i].GetComponent<UnityEngine.UI.Image>().enabled = false;

                //Remove a carta
                hiddenButtonList.RemoveAt(i);
            }
        }
    }

    void CheckWin()
    {
        //ScoreManager.instance.mensagemText.text = "hiddenButtonList.Count: "+ hiddenButtonList.Count.ToString("D").Trim();

        if (hiddenButtonList.Count < 1)
        {
            //Para a contagem de tempo
            ScoreManager.instance.StopTime();
            //Show UI

            //Fireworks

            //Show Stars
            ScoreManager.instance.mensagemText.enabled = true;
            ScoreManager.instance.mensagemText.text = "Fim de jogo!";
            ScoreManager.instance.jogoFinalizadoText.enabled = true;

            ScoreManager.instance.RegistrarEventos();
            //Debug.Log("Você venceu!");

            //Apresenta a tela de Parabéns
            SceneManager.LoadScene("Parabens");
        }

        if (hiddenButtonList.Count == 4)
        {
            ScoreManager.instance.mensagemText.enabled = true;
            ScoreManager.instance.mensagemText.text = "Vamos lá, faltam 2 combinações!";
        }

        if (ScoreManager.instance.turnsText.text == "6" & hiddenButtonList.Count > 2)
        {
            ScoreManager.instance.mensagemText.text = "Hmmm...pense bem...lembre de combinar os pares!";           
        }

        //Dá dica da posição em que o par está escondido

    }
}
