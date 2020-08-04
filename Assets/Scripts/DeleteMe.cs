using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteMe : MonoBehaviour
{
    public float deleteTime = 0.8f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, deleteTime);        
    }
}
