using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuGame: MonoBehaviour
{
    public Text playerDisplay;

    private void Start()
    {
        if (DBManager.LoggedIn)
        {
            playerDisplay.text = "Bem vindo (a), " + DBManager.username + "!";
        }
    }

    public void CarregarMemoryGame()
	{
		SceneManager.LoadScene("MemoryGame");
	}

    public void RetornarMenuGame()
    {
        SceneManager.LoadScene("MenuGame");
    }

    public void CarregarMenuConfiguracao()
    {
        SceneManager.LoadScene("MenuConfiguracao");
    }

    public void SairJogo()
    {
        Debug.Log("Saindo...");
        Application.Quit();
    }
}
