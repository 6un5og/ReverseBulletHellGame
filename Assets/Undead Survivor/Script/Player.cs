using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public Scanner scanner;
    public Hand[] hands;
    public RuntimeAnimatorController[] animCon;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true);        // 인자값 true를 넣으면 비활성화 된 오브젝트도 가능
    }

    void OnEnable()
    {
        speed *= Character.Speed;       // 각 캐릭터의 계수에 따라
        anim.runtimeAnimatorController = animCon[GameManager.instance.playerId];    // 플레이어 선택 시 플레이어 ID에 맞는 애니메이터 넣기
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;
    }

    void FixedUpdate()
    {
        // rigid.AddForce(inputVec);       // 힘을 준다

        // rigid.velocity = inputVec;      // 속도 제어

        if (!GameManager.instance.isLive)
            return;

        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;   // fixedDeltaTime : 물리 프레임 하나가 소비한 시간 
        rigid.MovePosition(rigid.position + nextVec);      // 위치 이동 (현재 위치도 더해줘야 함)
    }

    // LateUpdate : 프레임이 종료 되기 전 실행되는 생명주기 함수 (업데이트가 끝나고 다음 프레임 넘어가기 직전 실행)
    void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        // SetFloat("파라미터 이름", 반영할 파라미터 값) magnitude : 벡터의 순수한 크기
        anim.SetFloat("Speed", inputVec.magnitude);     // 애니메이터에서 설정한 파라미터 타입과 동일한 함수 작성

        if (inputVec.x != 0)                            // 좌, 우 키를 눌렀을 때만
        {
            spriter.flipX = inputVec.x < 0;             // 스프라이트 플립 설정 컴포넌트로 받아오고 현재 x값과 0 비교헤서 넣기
        }
    }

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }

        GameManager.instance.health -= Time.deltaTime * 10;

        if (GameManager.instance.health < 0)
        {
            // 플레이어가 죽으면 (본인포함) shadow, area 밑 전부 비활성화 시키기
            for (int index = 2; index < transform.childCount; index++)      // transform.childCount로 전체 개수 세고 index 2부터 시작
            {
                transform.GetChild(index).gameObject.SetActive(false);      // GetChild() : 주어진 인덱스의 자식 오브젝트를 반환하는 함수
            }

            anim.SetTrigger("Dead");
            GameManager.instance.GameOver();                                // 게임 오버 함수를 플레이어 스크립트의 사망 부분에서 호출하도록 작성
        }
    }
}
