using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Parabens : MonoBehaviour
{
    public static Parabens instance;

    public Text TextMensagemParabens;
    public GameObject fxExplosion;

    void Start()
    {
        //Deixa as estrelas na telas para comemoração...
        Instantiate(fxExplosion, TextMensagemParabens.transform.position + new Vector3(0, 0, -1), Quaternion.identity);
        /*
        for (int i = -5; i < 5; i++)
        {
            Instantiate(fxExplosion, ScoreManager.instance.mensagemFinalJogoText.transform.position + new Vector3(0 + i, 0, -1), Quaternion.identity);
            //System.Threading.Thread.Sleep(2300);
        }
        */
    }

    // Botão para voltar para o menu de jogos
    public void RetornarMenuGame()
    {
        SceneManager.LoadScene("MenuGame");
    }
}
