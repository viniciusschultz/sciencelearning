using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpacerCard : MonoBehaviour
{
    public static SpacerCard instance;
    private GridLayoutGroup glg;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        glg = gameObject.GetComponent<GridLayoutGroup>();
        //Debug.Log(glg);
        //RedimensionarGridCartas(3);
    }

    public void RedimensionarGridCartas(int iQuantidadePares)
    {        
        //Debug.Log(iQuantidadePares);
        //Informar a dimensão do GridLayout
        //Para 3 pares: [Top = 10; Cell Size {X = 160, Y = 200}; Spacing {X = 20, Y = 20}
        
        if (iQuantidadePares == 3)
        {
            glg.padding.top = 10;
            glg.cellSize = new Vector2(160, 200);
            glg.spacing = new Vector2(20, 20);            
        }        
    }
}
