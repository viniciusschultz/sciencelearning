using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuConfiguracao : MonoBehaviour
{
    public static MenuConfiguracao instance;

    [SerializeField]
    public Dropdown DropdownQuantidadeCartas;
    public Text textMensagem;

    public void Start()
    {
        textMensagem.enabled = false;
        //Carrega as configurações.
        carregarConfiguracao();        
    }

    private void Awake()
    {
        instance = this;
    }

    public void carregarConfiguracao()
    {
        if (PlayerPrefs.HasKey("lembrarConfiguracao") && PlayerPrefs.GetInt("lembrarConfiguracao") == 1)
        {
            DropdownQuantidadeCartas.value = PlayerPrefs.GetInt("ValueDropdownQtdeCartas");
            //ConfirmarConfiguracao();
        }
    }

    public void ConfirmarConfiguracao()
    {
        int qtdeCartasConfiguracao = DropdownQuantidadeCartas.value;

        PlayerPrefs.SetInt("lembrarConfiguracao", 1);
        PlayerPrefs.SetInt("ValueDropdownQtdeCartas", qtdeCartasConfiguracao);

        textMensagem.enabled = true;
    }

    public void RetornarMenuGame()
    {        
        SceneManager.LoadScene("MenuGame");
    }
}
