using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{

    public float speed;
    public int attackDamage;
    public GameObject impactEffect;

    public float lifeTime;
    public float MaxLifeTime = 5f;

    private Animator FireballAnimator;

    // Start is called before the first frame update
    void Start()
    {
        FireballAnimator = GetComponent<Animator>();
        FireballAnimator.SetBool("Hit",false);
        lifeTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.right * transform.localScale.x * speed * Time.deltaTime);
        
        
        lifeTime += Time.deltaTime;
        if (lifeTime > MaxLifeTime)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            return;

        FireballAnimator.SetBool("Hit", true);
        speed = 0;

        //Trigger the custom action on the other object IF IT EXISTS
        if (collision.GetComponent<Enemy>())
            collision.GetComponent<Enemy>().TakeDamage(attackDamage); 

        UnityEngine.Debug.Log("Hit "+ collision.name);


    }

    public void  DestroyFirball() => Destroy(gameObject);
    
        
 }
