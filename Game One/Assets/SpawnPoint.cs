using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GameObject Fire;
    public GameObject Glow;
    public GameObject Spark;
    public Transform SpawnPosition;
    private bool Triggered;



    // Start is called before the first frame update
    void Start()
    {
        Fire.SetActive(false);
        Glow.SetActive(false);  
        Spark.SetActive(false);
        Triggered = false;


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void OnTriggerEnter2D(Collider2D player)
    {
        //Debug.Log("Spawn point collision");

        if (player.gameObject.CompareTag("Player") && !Triggered)
        {
            Fire.SetActive(true);
            Glow.SetActive(true);
            Spark.SetActive(true);
            Debug.Log("Player activated save spot!");

            Triggered = true;

            player.gameObject.GetComponent<PlayerAction>().SetSpawnPoint(SpawnPosition);
        }
    }
}
