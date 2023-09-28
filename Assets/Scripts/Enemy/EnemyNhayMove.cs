using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class EnemyNhayMove : MonoBehaviour
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

 
    }

    private void Start()
    {
        this.StartJumping();
    }


    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            anim.SetTrigger("isDead");
            Invoke("DestroyGameObject", 0.25f);
        }
    }
    

    void DestroyGameObject()
    {
        Destroy(gameObject);
        this.Hooked();
    }
    private void StartJumping()
    {
        // Sử dụng Sequence để lặp lại hoạt động nhảy
        Sequence jumpSequence = DOTween.Sequence();

        for (int i = 0; i < 1; i++)
        {
            // Tạo một Tween để nhảy lên
            jumpSequence.Append(transform.DOMoveY(transform.position.y + 3, 2)
                .SetEase(Ease.OutQuad)); // Có thể thay đổi loại đường cong tùy ý

            // Tạo một Tween để rơi xuống
            jumpSequence.Append(transform.DOMoveY(transform.position.y, 2)
                .SetEase(Ease.InQuad)
                .SetDelay(2f)
                .OnPlay(delegate {  })
                .OnStepComplete(delegate
                    {
                        // khi hoan than 1 chu ky di chuyen thi doi huong 
                        Vector3 localScale = transform.localScale;
                        localScale.x = -localScale.x;
                        transform.localScale = localScale;
                        // Flip X trong SpriteRenderer

                    })
            ); // Có thể thay đổi loại đường cong tùy ý
        }

        // Thiết lập lặp vô hạn
        jumpSequence.SetLoops(-1);
    }
    // public void StartMoveFish()
    // {
    //
    //     // Thiet lap lai vi tri 
    //     Vector3 position = transform.position; 
    //
    //     transform.position = position;
    //
    //     float delay = 1f;
    //
    //     float duration = 5f;
    //     
    //     // Lấy vị trí ban đầu của nhân vật
    //     Vector3 initialPosition = transform.position;
    //
    //     // Tính vị trí cuối cùng của nhân vật dựa trên độ cao tối đa của bay
    //     Vector3 finalPosition = initialPosition + new Vector3(5f, 2f, 0);
    //
    //     fishTweener = transform.DOMove(finalPosition, duration)
    //         .SetEase(Ease.InSine)
    //         .SetLoops(-1, LoopType.Yoyo)
    //         .SetDelay(delay)
    //         .OnStepComplete(delegate
    //         {
    //             Vector3 finalPosition2 = finalPosition + new Vector3(5f, 0f, 0);
    //             fishTweener = transform.DOMove(finalPosition2, duration)
    //                 .SetEase(Ease.InSine)
    //                 
    //                 .OnStepComplete(delegate
    //                 {
    //                     
    //                     // khi hoan than 1 chu ky di chuyen thi doi huong 
    //                     Vector3 localScale = transform.localScale;
    //                     localScale.x = -localScale.x;
    //                     transform.localScale = localScale;
    //                     // Flip X trong SpriteRenderer
    //                 });
    //             
    //
    //         });
    // }

    public void Hooked()
    {
        fishTweener.Kill();
    }

 
}
