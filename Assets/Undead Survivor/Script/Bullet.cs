using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int per;

    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Weapon.Fire() 에 있는 값들을 모두 받아와서 실행
    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;               // this : 해당 클래스의 변수로 접근
        this.per = per;

        if (per >= 0)                       // 관통이 -1(무한)보다 큰 것에 대해서는 속도 적용
        {
            rigid.velocity = dir * 15f;     // 속력 곱해줘서 속도 증가시키기
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || per == -100)
            return;

        per--;

        if (per < 0)  // 관통값이 하나씩 줄어들면서 -1이 되면 비활성화
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    void OnTriggerExit(Collider collision)  // 투사체가 멀리 날아가면 삭제 함수
    {
        if (!collision.CompareTag("Area") || per == -100)   // 플레이어가 가지고 있는 Area 밖으로 나가면 삭제
            return;

        gameObject.SetActive(false);
    }
}
