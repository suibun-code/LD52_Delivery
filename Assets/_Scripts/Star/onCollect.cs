using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onCollect : MonoBehaviour
{
    public GameObject prefab;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player") 
        {
            ScoreManager.instance.AddPoint();
            Instantiate(prefab, gameObject.transform.position,Quaternion.identity);
            Destroy(gameObject);
            Debug.Log("obtained 1 star");
        }
    }
}
