using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class Registration : MonoBehaviour
{

    public InputField nameField;
    public InputField passwordField;
    public InputField dataNascimentoField;
    public Dropdown dropdownGenero;

    public Button submitButton;
    private String retornobd;

    private int iIndex;
    private int iPrevLength;

    //Mensagem do jogo
    public Text TextMensagem;

    public void Start()
    {
        //nameField.text = UserData.Usuario;
        //passwordField.text = UserData.Password;

        //Debug.Log("UserData.Usuario " + UserData.Usuario);

        dataNascimentoField.onValueChanged.AddListener (OnValueChanged);
    }

    public void OnValueChanged(string str)
    {
        //print("String:" + str);
        if (str.Length > 0)
        {
            dataNascimentoField.onValueChanged.RemoveAllListeners();
            if (!char.IsDigit(str[str.Length - 1]) && str[str.Length - 1] != '/')
            { // Remove Letters
                dataNascimentoField.text = str.Remove(str.Length - 1);
                dataNascimentoField.caretPosition = dataNascimentoField.text.Length;
            }
            else if (str.Length == 2 || str.Length == 5)
            {
                if (str.Length < iPrevLength)
                { // Delete
                    dataNascimentoField.text = str.Remove(str.Length - 1);
                    dataNascimentoField.caretPosition = dataNascimentoField.text.Length;
                }
                else
                { // Add
                    dataNascimentoField.text = str + "/";
                    dataNascimentoField.caretPosition = dataNascimentoField.text.Length;
                }
            }
            dataNascimentoField.onValueChanged.AddListener(OnValueChanged);
        }
        iPrevLength = dataNascimentoField.text.Length;
    }

    public void CallRegister()
    {
        StartCoroutine(Register());
    }

    IEnumerator Register()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", nameField.text);
        form.AddField("password", passwordField.text);
        form.AddField("data_nascimento", dataNascimentoField.text);
        form.AddField("sexo", (dropdownGenero.value > 0 ? "FEMININO" : "MASCULINO"));
        //WWW www = new WWW("http://localhost/scilearn/register.php", form);
        WWW www = new WWW("http://dainf.pg.utfpr.edu.br/scilearn/register.php", form);

        yield return www;

        Debug.Log(www.text);

        retornobd = www.text; // Por algum motivo o retorno de string não fazia a comparação correta, fiz a conversão.

        if (retornobd.Trim() == "OK")
        {
            Debug.Log("Usuário criado com sucesso!");
            TextMensagem.text = "Usuário criado com sucesso! Clique em VOLTAR.";
            //yield return new WaitForSeconds(1.5f);//tempo que a carta é exibida
            //SceneManager.LoadScene("MenuLogin");
        }
        else
        {
            TextMensagem.text = www.text;
            Debug.Log("Criação de usuário falhou...Erro #" + www.text);
        }
    }

    public void VerifyInputs()
    {
        /*
        if(nameField.text.Length <= 4)
        {
            TextMensagem.text = "O usuário deve ter mais que 4 caracteres.";
        }
        */
        
        //submitButton.interactable = (nameField.text.Length >= 4 && passwordField.text.Length >= 4);
    }

    public void RetornarMenuLogin()
    {
        SceneManager.LoadScene("MenuLogin");
    }
}
