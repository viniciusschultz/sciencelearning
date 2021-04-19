using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public static Login instance;

    public InputField nameField;
    public InputField passwordField;

    public Button submitButton;
    private int retornobd;

    //Mensagem do jogo
    public Text TextMensagem;

    //Manter login e senha de usuário
    [SerializeField]
    private Toggle LembrarDados = null;

    void Start()
    {
        TextMensagem.enabled = false;

        if(PlayerPrefs.HasKey("lembrar") && PlayerPrefs.GetInt("lembrar") == 1)
        {
            nameField.text = PlayerPrefs.GetString("rememberUser");
            passwordField.text = PlayerPrefs.GetString("rememberSenha");
        }
    }

    public void CallLogin()
    {
        string usuario = nameField.text;
        string senha = passwordField.text;

        if (LembrarDados.isOn)
        {
            PlayerPrefs.SetInt("lembrar", 1);
            PlayerPrefs.SetString("rememberUser", usuario);
            PlayerPrefs.SetString("rememberSenha", senha);
        }

        StartCoroutine(LoginPlayer());
    }

    IEnumerator LoginPlayer()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", nameField.text);
        form.AddField("password", passwordField.text);

        //WWW www = new WWW("http://localhost/scilearn/login.php", form);
        WWW www = new WWW("http://dainf.pg.utfpr.edu.br/scilearn/login.php", form);

        yield return www;

        //Lembrar de fazer o tratamento caso servidor de BD não esteja inicializado.
        Debug.Log(www.text[0]);

        if(www.text[0] == '0')
        {
            //Debug.Log("Opa #" + www.text);
            DBManager.username = nameField.text;
            //DBManager.score = int.Parse(www.text.Split('\t')[1]);
            DBManager.idUsuario = int.Parse(www.text.Split('\t')[1]);
            //Debug.Log("Login.cs -> DBManager.idUsuario:"+ DBManager.idUsuario);
            CarregarMenuJogos();
            //UnityEngine.SceneManagement.SceneManager.LoadScene(3); //Outra forma de carregar o MenuGame com as opções de jogos.
        }
        else
        {
            //Debug.Log("O Login falhou! Erro #" + www.text);
            TextMensagem.enabled = true;
            TextMensagem.text = www.text;
            if (www.text[0] == '5')
            {
                //Debug.Log("Enviando dados para a tela de Registro.");
                UserData.Usuario  = nameField.text;
                UserData.Password = passwordField.text;
                //CarregarMenuRegistro();                
            }
        }
    }

    public void VerifyInputs()
    {
        //submitButton.interactable = (nameField.text.Length >= 4 && passwordField.text.Length >= 4);
    }

    public void CarregarMenuJogos()
    {
        SceneManager.LoadScene("MenuGame");
    }

    public void CarregarMenuRegistro()
    {
        SceneManager.LoadScene("MenuRegistro");
    }

    public void SairJogo()
    {
        Debug.Log("Saindo...");
        Application.Quit();
    }
}
