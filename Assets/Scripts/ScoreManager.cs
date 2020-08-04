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

    //Torna o objeto acessível a qualquer momento do jogo.
    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        UpdateScoreText();
        StartCoroutine("PlayTime"); // Faz a chamada pelo nome do procedimento entre quotes, mas desse jeito também funciona: StartCoroutine(Playtime());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);// Reinicia a cena
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
        scoreText.text = "Pontos: " + currentScore.ToString("D").Trim();
        comboText.text = "Combo: "+ currentComboAmount.ToString("D").Trim();
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
        timeText.text = "Tempo:" + minutes.ToString("D2") + ":" + seconds.ToString("D2");
    }

    public void StopTime()
    {
        //StopAllCoroutines();
        StopCoroutine("PlayTime");
    }
}
