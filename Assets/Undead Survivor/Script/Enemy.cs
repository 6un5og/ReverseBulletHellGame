using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;     // 속도
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;  // 목표(물리적 -> 리지드바디)

    bool isLive;                // 생존 여부

    Rigidbody2D rigid;
    Collider2D coll;
    Animator anim;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;

    void Awake()
    {
        anim = GetComponent<Animator>();            // Init 함수가 Start보다 빨리 호출돼서 anim 변수가 초기화되지 않음
        coll = GetComponent<Collider2D>();
        spriter = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        wait = new WaitForFixedUpdate();
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        // 넉백 설정 : 애니메이터가 진행중일 때 현재 정보를 "GetCurrentAnimatorStateInfo(레이어 번호)" 로 가져오고  IsName() 으로 이름을 매치함
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))            // 몬스터가 살아있는 동안에만 움직이도록 조건 추가
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
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();   // OnEnable에서 타겟 변수에 게임매니저를 활용하여 플레이어 할당
        isLive = true;          // 활성화 될 때 살아남
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
        health = maxHealth;     // 죽어있거나 새로 생기면 최대 체력을 채워서 생성
    }

    // 초기 속성을 적용하는 함수 추가
    public void Init(SpawnData data)     // 매개 변수로 소환데이터 하나 지정
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];      // 매개변수의 속성을 몬스터 속성 변경에 활용하기
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive)     // 사망 로직이 연달아 실행되는 것을 방지하기 위해 조건 추가(!isLive)
            return;

        health -= collision.GetComponent<Bullet>().damage;          // Bullet 컴포넌트로 접근하여 데미지를 가져와 피격 계산하기
        StartCoroutine(KnockBack());            // 문자열로 감싸도 가능     StartCoroutine("KnockBack");

        if (health > 0)
        {
            anim.SetTrigger("Hit");
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
        }
        else
        {
            isLive = false;
            coll.enabled = false;           // 컴포넌트 비활성화
            rigid.simulated = false;        // 리지드바디의 물리적 비활성화는 .simulated = false;
            spriter.sortingOrder = 1;       // 스프라이트 랜더러의 sorting order 감소
            anim.SetBool("Dead", true);
            GameManager.instance.kill++;    // 몬스터 사망 시 킬 수 증가
            GameManager.instance.GetExp();  // 몬스터 사망 시 겅험치 함수 호출

            if (GameManager.instance.isLive)
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
        }
    }

    IEnumerator KnockBack()     // IEnumerator : 코루틴만의 반환형 인터페이스 (앞 글자가 I가 들어가면 인터페이스 관련)
    {
        yield return wait;      // 하나의 물리 프레임 딜레이
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;                // 플레이어의 반대방향으로 가기 위한 계산
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);     // 순간적인 힘을 주기 위해 Impulse
    }

    void Dead()
    {
        gameObject.SetActive(false);        // 오브젝트 풀링이기 때문에 Destroy가 아닌 false
    }
}
