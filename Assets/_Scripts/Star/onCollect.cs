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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.transform.parent.GetComponentInChildren<Player>();
            player.AddScore(1);
            AudioManager.Instance.PlayCoinCollectSound();

            Instantiate(prefab, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
