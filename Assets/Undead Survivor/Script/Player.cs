using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public Scanner scanner;
    public Hand[] hands;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true);        // 인자값 true를 넣으면 비활성화 된 오브젝트도 가능
    }

    void FixedUpdate()
    {
        // rigid.AddForce(inputVec);       // 힘을 준다

        // rigid.velocity = inputVec;      // 속도 제어


        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;   // fixedDeltaTime : 물리 프레임 하나가 소비한 시간 
        rigid.MovePosition(rigid.position + nextVec);      // 위치 이동 (현재 위치도 더해줘야 함)
    }

    // LateUpdate : 프레임이 종료 되기 전 실행되는 생명주기 함수 (업데이트가 끝나고 다음 프레임 넘어가기 직전 실행)
    void LateUpdate()
    {                                                   // SetFloat("파라미터 이름", 반영할 파라미터 값) magnitude : 벡터의 순수한 크기
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
}
