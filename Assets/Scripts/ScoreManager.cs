using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    //Score
    public int currentScore;
    //Combo / Turn
    private int currentComboAmount;
    private int currentTurn;
    //Playtime
    public int playtime;
    private int seconds;
    private int minutes;
    [Header("Text Connections")]
    public Text timeText;
    public Text scoreText;
    public Text comboText;
    public Text turnsText;

    //Carregar Dados do BD
    private String tempoEvento;
    private String pontosEvento;
    private String combosEvento;
    private String tentativasEvento;
    private int sucessoEvento;
    private int jogoFinalizadoEvento;
    //Variáveis de clique do mouse
    private int qtdeBotaoEsquerdo;
    private int qtdeBotaoCentro;
    private int qtdeBotaoDireito;
    //Variáveis de Quantidade de Acerto e Erro
    public int qtdeAcerto;
    //A variável qtdeErro será um contador de vezes de cartas não combinadas. A ideia é que o personagem indique o próximo par
    //Lembrar que conforme a quantidade de pares a "ajuda" seria exibida com menos ou mais pares não combinados
    public int qtdeErro;
    private int qtdeTeclasUsadas;
    //Variáreis de registro das Cartas apresentadas
    private String descricaoCarta;
    private String descricaoParSelecionado;
    //Descrição da Cor Carta Selecionada
    private String descricaoCorCarta;
    private String descricaoCoresParSelecionado;
    //Variáveis de Identificação das Cartas
    private String SequenciaCartaID;
    private string todosSequenciaCartaID;
    private string todosParesSelecionadoErradoID;
    private string todosParesSelecionadoCertoID;
    private String todosParesSelecionadopartidaID;
    public String todasPrimeirasCartasSelecionadasID;
    private String todasPosicaoParSugerido;
    private String todostempoPrimeiraCartaVirada;
    private String todostempoSegundaCartaVirada;

    //Mensagem do jogo
    public Text mensagemText;
    public Text jogoFinalizadoText;
    private String retornobd;

    //Torna o objeto acessível a qualquer momento do jogo.
    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        jogoFinalizadoText.enabled = false;
        UpdateScoreText();
        //StartCoroutine("PlayTime"); // Faz a chamada pelo nome do procedimento entre quotes, mas desse jeito também funciona: StartCoroutine(Playtime());
        //Inicializa as variáveis de texto
        todosSequenciaCartaID = "";
        todosParesSelecionadoErradoID = "";
        todosParesSelecionadoCertoID = "";
        todosParesSelecionadopartidaID = "";
        todasPrimeirasCartasSelecionadasID = "";
        todasPosicaoParSugerido = "";
        todostempoPrimeiraCartaVirada = "";
        todostempoSegundaCartaVirada = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            SomarBotaoMouse(0);
            //Debug.Log("Pressed primary button.");

        if (Input.GetMouseButtonDown(1))
            SomarBotaoMouse(1);
        //Debug.Log("Pressed secondary button.");

        if (Input.GetMouseButtonDown(2))
            SomarBotaoMouse(2);
        //Debug.Log("Pressed middle click.");

        //Verifica as teclas do teclado utilizadas sem considerar os botões do mouse
        if ( (Input.anyKeyDown) && !( Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2) ) )
        {
            SomarQtdeTeclasUsadas();
            //Debug.Log("Tecla pressionada!! =O ");
        }
    }

    public void RestartLevel()
    {
        if (jogoFinalizadoText.enabled == false) { // Se a mensagem de finalização estiver habilitada e reiniciar o jogo, não registra o evento.
            RegistrarEventos();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);// Reinicia a cena
    }
    // Botão para voltar para o menu de jogos
    public void RetornarMenuGame()
    {
        if (jogoFinalizadoText.enabled == false)
        { // Se a mensagem de finalização estiver habilitada e reiniciar o jogo, não registra o evento.
            RegistrarEventos();
        }

        SceneManager.LoadScene("MenuGame");
    }

    //Soma a quantidade de cliques do botão esquerdo do mouse
    public void SomarBotaoMouse(int botaomouse)
    {
        if (botaomouse == 0) { qtdeBotaoEsquerdo++; }
        if (botaomouse == 1) { qtdeBotaoDireito++; }
        if (botaomouse == 2) { qtdeBotaoCentro++; }

    }

    public void SomarQtdeTeclasUsadas()
    {
        qtdeTeclasUsadas++;
    }

    public void SomarQtdeAcerto()
    {
        qtdeAcerto++;
    }

    public void SomarQtdeErro()
    {
        qtdeErro++;
    }

    public void AddSequenciaCartaID(List<string> idSequencia)
    {
        todosSequenciaCartaID = string.Join(",", idSequencia.ToArray());
        //Debug.Log("Sequência da cartas apresentadas: " + todosSequenciaCartaID);
    }

    //função que armazena a combinação dos pares selecionados errados (sem combinação)
    public void ObterDescricaoParSelecionadoID(string primeiraCartaID)
    {
        var cartas = new List<object>();
        cartas.Add(primeiraCartaID);
        //cartas.Add(segundaCartaID);

        descricaoCarta = "";
        descricaoCorCarta = "";
        String beneficioDescricaoCarta = "";

        foreach (var item in cartas)
        {
            switch (item)
            {
                case "1":
                    descricaoCarta = "Abóbora";
                    descricaoCorCarta = "Laranja"; //Cor laranja e não uma laranja
                    beneficioDescricaoCarta = "A Abóbora é benéfica para a visão e ajuda a preservar a pele.";
                    break;
                case "2":
                    descricaoCarta = "Cenoura";
                    descricaoCorCarta = "Laranja"; //Cor laranja e não uma laranja
                    beneficioDescricaoCarta = "A Cenoura fortalece o sistema imunológico e pode ajudar a reduzir o colesterol.";
                    break;
                case "3":
                    descricaoCarta = "Maçã Verde";
                    descricaoCorCarta = "Verde"; //Cor verde
                    beneficioDescricaoCarta = "A Maçã Verde tem ação anti-ácida, melhora a digestão e alivia azias e refluxos.";
                    break;
                case "4":
                    descricaoCarta = "Banana";
                    descricaoCorCarta = "Amerela"; //Cor amarela
                    beneficioDescricaoCarta = "A Banana ajuda na recuperação muscular e ajuda a evitar as câimbras.";
                    break;
                case "5":
                    descricaoCarta = "Morango";
                    descricaoCorCarta = "Vermelha"; //Cor vermelha
                    beneficioDescricaoCarta = "O morango combate o envelhecimento da pele, ajuda a prevenir doenças cardiovasculares e melhorar a capacidade mental.";
                    break;
                case "6":
                    descricaoCarta = "Pimentão";
                    descricaoCorCarta = "Amarela"; //Cor amarela
                    beneficioDescricaoCarta = "O Pimentão contribui para a manutenção de ossos e dentes saudáveis pois possui cálcio na composição.";
                    break;
                case "7":
                    descricaoCarta = "Tomate";
                    descricaoCorCarta = "Vermelha"; //Cor vermelha
                    beneficioDescricaoCarta = "O Tomate ajuda a prevenir o câncer de próstata porque é composto de licopeno.";
                    break;
                case "8":
                    descricaoCarta = "Cebola";
                    descricaoCorCarta = "Branca"; //Cor branca
                    beneficioDescricaoCarta = "A Cebola tem ação expectorante e funciona como um ótimo remédio natural para o tratamento da tosse.";
                    break;
                case "9":
                    descricaoCarta = "Berinjela";
                    descricaoCorCarta = "Roxa"; //Cor Roxa
                    beneficioDescricaoCarta = "A Berinjela traz diversos benefícios para o coração devido a presença de fibras solúveis e antioxidantes, e melhora do fluxo sanguíneo.";
                    break;
                case "10":
                    descricaoCarta = "Batata";
                    descricaoCorCarta = "Marrom"; //Cor marrom
                    beneficioDescricaoCarta = "A Batata ajuda o corpo a manter a estrutura e a força óssea.";
                    break;
            }

            if (String.IsNullOrEmpty(descricaoParSelecionado) || String.IsNullOrEmpty(descricaoCoresParSelecionado))
            {
                //Descrição do vegetal/fruta selecionada
                descricaoParSelecionado = descricaoCarta;
                //Cor do vegetal/fruta selecionada
                descricaoCoresParSelecionado = descricaoCorCarta;
            }
            else
            {
                descricaoParSelecionado = descricaoParSelecionado + " , " + descricaoCarta;
                descricaoCoresParSelecionado = descricaoCoresParSelecionado + " , " + descricaoCorCarta;
            }
        }

        //Exibe a mensagem referente a um determinado fruta/legume pelo personagem
        mensagemText.text = beneficioDescricaoCarta;

        //Debug.Log("Pares selecionados: " + descricaoParSelecionado);
        //Debug.Log("Cor dos Pares selecionados: " + descricaoCoresParSelecionado);
    }

    //função que armazena a combinação dos pares sugeridos para a combinação
    public void AddPosicaoParSugeridoID(string PosicaoParSugeridoID)
    {
        //ObterDescricaoParSelecionadoID(primeiraCartaID, segundaCartaID);

        if (String.IsNullOrEmpty(todasPosicaoParSugerido))
        {
            todasPosicaoParSugerido = PosicaoParSugeridoID;
        }
        else
        {
            todasPosicaoParSugerido = todasPosicaoParSugerido + " , " + PosicaoParSugeridoID;
        }

        //Debug.Log("todasPosicaoParSugerido: "+todasPosicaoParSugerido);
    }

    //função que armazena a combinação dos pares selecionados durante a partida, sendo certo ou errado.
    //E também registra o ID da primeira carta selecionada para fazer a busca e apresentar as dicas das cartas que já foram viradas.
    public void AddParSelecionadoID(string primeiraCartaID, string segundaCartaID, string parSelecionadoID)
    {
        //ObterDescricaoParSelecionadoID(primeiraCartaID, segundaCartaID);

        if (String.IsNullOrEmpty(todosParesSelecionadopartidaID))
        {
            todosParesSelecionadopartidaID = parSelecionadoID;
        }
        else
        {
            todosParesSelecionadopartidaID = todosParesSelecionadopartidaID + " , " + parSelecionadoID;
        }

        //Registra o ID da primeira carta selecionada para fazer a busca e apresentar as dicas das cartas que já foram viradas.
        if (String.IsNullOrEmpty(todasPrimeirasCartasSelecionadasID))
        {
            todasPrimeirasCartasSelecionadasID = primeiraCartaID;
        }
        else
        {
            todasPrimeirasCartasSelecionadasID = todasPrimeirasCartasSelecionadasID + " , " + primeiraCartaID;
        }
    }

    //função que armazena a combinação dos pares selecionados errados (sem combinação)
    public void AddParSelecionadoErradoID(string primeiraCartaID, string segundaCartaID, string parErradoSelecionadoID)
    {
        //ObterDescricaoParSelecionadoID(primeiraCartaID, segundaCartaID);

        if (String.IsNullOrEmpty(todosParesSelecionadoErradoID)) {
            todosParesSelecionadoErradoID = parErradoSelecionadoID;
        }
        else
        {
            todosParesSelecionadoErradoID = todosParesSelecionadoErradoID + " , " + parErradoSelecionadoID;
        }
    }

    //função que armazena a combinação dos pares selecionados errados (com combinação)
    //O par que é selecionado usando a dica é atribuído um * no CardManager.cs pela variável sSugeridoPar
    public void AddParSelecionadoCertoID(string primeiraCartaID, string segundaCartaID, string parCertoSelecionadoID)
    {
        //ObterDescricaoParSelecionadoID(primeiraCartaID, segundaCartaID);

        if (String.IsNullOrEmpty(todosParesSelecionadoCertoID))
        {
            todosParesSelecionadoCertoID = parCertoSelecionadoID;
        }
        else
        {
            todosParesSelecionadoCertoID = todosParesSelecionadoCertoID + " , " + parCertoSelecionadoID;
        }
    }

    public void RegistraTempoPrimeiraCartaVirada()
    {
        if (String.IsNullOrEmpty(todostempoPrimeiraCartaVirada))
        {
            todostempoPrimeiraCartaVirada = minutes.ToString("D2") + ":" + seconds.ToString("D2");
        }
        else
        {
            todostempoPrimeiraCartaVirada = todostempoPrimeiraCartaVirada + " , " + minutes.ToString("D2") + ":" + seconds.ToString("D2");
        }
    }
    public void RegistraTempoSegundaCartaVirada()
    {
        if (String.IsNullOrEmpty(todostempoSegundaCartaVirada))
        {
            todostempoSegundaCartaVirada = minutes.ToString("D2") + ":" + seconds.ToString("D2");
        }
        else
        {
            todostempoSegundaCartaVirada = todostempoSegundaCartaVirada + " , " + minutes.ToString("D2") + ":" + seconds.ToString("D2");
        }
    }

    public void AddScore(int scoreAmount)
    {
        currentComboAmount++;
        currentTurn++;
        currentScore += scoreAmount * currentComboAmount;
        UpdateScoreText();
    }

    public void ResetCombo()
    {
        currentComboAmount = 0;
        currentTurn++;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreText.text = "Pontos: "+"\r\n"+ currentScore.ToString("D").Trim();
        comboText.text = "Combo: " + "\r\n" + currentComboAmount.ToString("D").Trim();
        turnsText.text = currentTurn.ToString("D").Trim(); //String.Format("{0,10:N0}", currentTurn); //currentTurn.ToString("N");
    }

    IEnumerator PlayTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            playtime++;
            seconds = (playtime % 60); // módulo de 60 segundos
            minutes = (playtime / 60) % 60; // módulo de 60 segundos
            updateTime();
        }
    }

    void updateTime()
    {
        timeText.text = "Tempo:" + "\r\n" + minutes.ToString("D2") + ":" + seconds.ToString("D2");
    }

    public void StartTime()
    {
        //StopAllCoroutines();
        StartCoroutine("PlayTime");
    }

    public void StopTime()
    {
        //StopAllCoroutines();
        StopCoroutine("PlayTime");
    }

    //Métodos para salvar os eventos no BD
    void UpdateEventos()
    {
        tempoEvento = minutes.ToString("D2") + ":" + seconds.ToString("D2");
        pontosEvento = currentScore.ToString("D").Trim();
        combosEvento = currentComboAmount.ToString("D").Trim();
        tentativasEvento = currentTurn.ToString("D").Trim(); //String.Format("{0,10:N0}", currentTurn); //currentTurn.ToString("N");
        //Se chegou até a parte de atualizar os eventos para o BD, o jogo foi finalizado e sucesso...
        sucessoEvento = jogoFinalizadoText.enabled ? 1 : 0;
        jogoFinalizadoEvento = sucessoEvento;
    }

    public void RegistrarEventos()
    {
        //Antes de enviar para o BD atualiza os valores
        UpdateEventos();
        StartCoroutine(Register());
    }

    IEnumerator Register()
    {
        //
        WWWForm form = new WWWForm();
        form.AddField("id_usuario", DBManager.idUsuario);
        form.AddField("id_jogo", 1); // 1 - Jogo da Memória
        form.AddField("tempo", tempoEvento);
        form.AddField("pontos", pontosEvento);
        form.AddField("combo", combosEvento);
        form.AddField("tentativas", tentativasEvento);
        form.AddField("qtde_clique_esquerdo", qtdeBotaoEsquerdo);
        form.AddField("qtde_clique_direito", qtdeBotaoDireito);
        form.AddField("qtde_acerto", qtdeAcerto);
        form.AddField("qtde_erro", qtdeErro);
        form.AddField("qtde_teclas_usadas", qtdeTeclasUsadas);        
        form.AddField("sucesso", sucessoEvento); //1-Sim / 0-Não
        form.AddField("jogo_finalizado", jogoFinalizadoEvento);
        form.AddField("sequencia_carta_id", todosSequenciaCartaID);
        form.AddField("pares_combinado_errado_id", todosParesSelecionadoErradoID);
        form.AddField("pares_combinado_certo_id", todosParesSelecionadoCertoID);
        form.AddField("pares_selecionados_partida_id", todosParesSelecionadopartidaID);
        form.AddField("posicao_par_sugerido_partida", todasPosicaoParSugerido);
        form.AddField("primeira_carta_par_selecionada_id", todasPrimeirasCartasSelecionadasID); // Primeira carta do par selecionada
        form.AddField("tempo_primeira_carta_virada", todostempoPrimeiraCartaVirada);
        form.AddField("tempo_segunda_carta_virada", todostempoSegundaCartaVirada);

        //WWW www = new WWW("http://localhost/scilearn/registrareventos.php", form);
        WWW www = new WWW("http://dainf.pg.utfpr.edu.br/scilearn/registrareventos.php", form);

        yield return www;

        Debug.Log(www.text);

        retornobd = www.text; // Por algum motivo o retorno de string não fazia a comparação correta, fiz a conversão.

        if (retornobd.Trim() == "OK")
        {
            Debug.Log("Eventos registrados com sucesso!");
            //UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
        else
        {
            Debug.Log("Registro de pontos falhou...Erro #" + www.text);
        }
    }
}
