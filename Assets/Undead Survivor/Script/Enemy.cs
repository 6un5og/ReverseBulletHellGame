using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;     // 속도
    public Rigidbody2D target;  // 목표(물리적 -> 리지드바디)

    bool isLive = true;                // 생존 여부

    Rigidbody2D rigid;
    SpriteRenderer spriter;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (!isLive)            // 몬스터가 살아있는 동안에만 움직이도록 조건 추가
            return;

        Vector2 dirVec = target.position - rigid.position;      // 위치 차이 = 타겟 위치 - 나의 위치, 값이 1이 아니기 때문에 -> 방향 = 위치 차이의 정규화(normalized)
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);           // 플레이어의 키입력 값을 더한 이동 = 몬스터의 방향 값을 더한 이동
        rigid.velocity = Vector2.zero;                          // 물리 속도가 이동에 영향을 주지 않도록 속도 제거
    }

    void LateUpdate()
    {
        if (!isLive)
            return;
        spriter.flipX = target.position.x < rigid.position.x;   // 목표의 X축 값과 자신의 Y축 값을 비교하여 작으면 true가 되도록 설정
    }

    void OnEnable()         // Enemy를 prefabs로 넘기면서 target을 잃었기 때문에 Enemy가 생성되고나서 바로 본인 스스로 타겟을 초기화시킴
    {
        // OnEnable에서 타겟 변수에 게임매니저를 활용하여 플레이어 할당
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
    }
}
