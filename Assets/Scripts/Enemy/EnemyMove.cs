using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class EnemyMove : MonoBehaviour
{

    // Start is called before the first frame update
    private Tweener fishTweener;
    private SpriteRenderer rend;
    private PolygonCollider2D coll;
    private float screenLeft;
    private Animator anim;

    // Start is called before the first frame update
    void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
        coll = GetComponent<PolygonCollider2D>();
        anim = GetComponent<Animator>();

        //screenLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        screenLeft = Camera.main.ScreenToWorldPoint(Vector3.zero).x;
    }

    private void Start()
    {
        this.StartMoveFish();
    }


    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            anim.SetTrigger("isDead");
            Invoke("DestroyGameObject", 0.25f);
        }
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    // }

    void DestroyGameObject()
    {
        this.Hooked();
        Destroy(gameObject);
    }
    
    public void StartMoveFish()
    {

        // Thiet lap lai vi tri 
        Vector3 position = transform.position; 
  
        transform.position = position;

        float delay = 2f;

        float duration = 6f;
        fishTweener = transform.DOMoveX(transform.position.x+5f, duration, false)
            .SetLoops(-1, LoopType.Yoyo)
            .SetDelay(delay)
            .OnStepComplete(delegate
            {
                // khi hoan than 1 chu ky di chuyen thi doi huong 
                Vector3 localScale = transform.localScale;
                localScale.x = -localScale.x;
                transform.localScale = localScale;
                // Flip X trong SpriteRenderer

            });
    }

    public void Hooked()
    {
        fishTweener.Kill();
    }

 
}
