using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("General References")]
    private Animator oAnimator;
    // Start is called before the first frame update
    void Start()
    {
        oAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Rodar animação de hit ao encontrar com o trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Soco"))
        {
            //Rodar animação de hit
            oAnimator.SetTrigger("enemyHit");  
        }
    }
}
