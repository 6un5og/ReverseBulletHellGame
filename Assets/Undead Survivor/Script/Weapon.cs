using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // 무기 ID, 프리펩 ID, 데미지, 개수, 속도
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    float timer;
    Player player;

    void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            default:
                timer += Time.deltaTime;

                if (timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
        }

        if (Input.GetButtonDown("Jump"))
        {
            LevelUp(10, 1);
        }
    }

    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        if (id == 0)
        {
            Batch();
        }
    }

    public void Init()
    {
        // 아이디에 따라 다르기 때문에 switch문 사용
        switch (id)
        {
            case 0:
                speed = -150;       // - 값이면 시계방향으로 돌아감

                Batch();
                break;
            default:
                speed = 0.3f;       // 연사속도
                break;
        }
    }

    // 생성된 무기를 배치하는 함수 생성 및 호출
    void Batch()
    {
        for (int index = 0; index < count; index++)
        {
            // for 문으로 count 만큼 풀링에서 가져오기, 가져온 오브젝트의 Transform을 지역변수로 저장
            Transform bullet;


            // if 문 정리 : 기존 오브젝트를 먼저 활용하고 모자란 것을 풀링한다는 구문
            if (index < transform.childCount)       // 인덱스가 자신의 자식 오브젝트 개수보다 작다면
            {
                bullet = transform.GetChild(index); // 갖고 있는 자식에서 가져온다 (재활용)
            }
            else
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform;     // 풀링을 이용해서 가져온다
                bullet.parent = transform;      // PoolManager가 아닌 Bullet을 따라가야 하기 때문에 부모 변경
            }


            bullet.localPosition = Vector3.zero;            // 플레이어의 위치로 초기화
            bullet.localRotation = Quaternion.identity;     // 회전값도 초기화


            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);    // 이동하는 방향의 self 값은 bullet.up에서 이미 함. 이동방향은 Space.world 기준으로
            // bullet 컴포넌트 접근하여 속성 초기화 함수 호출
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero);     // -1 is Infinity Per. (좋은 주석)
        }
    }

    void Fire()
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;      // 적의 위치
        Vector3 dir = targetPos - transform.position;                   // 크기가 포함된 적을 향한 방향 -> 크기는 항상 일정해야 한다 -> normalized 사용
        dir = dir.normalized;                                           // normalized : 현재 벡터의 방향은 유지하고 크기를 1로 변환된 속성

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);   // FromToRotation : 지정된 축을 중심으로 목표를 향해 회전하는 함수
        bullet.GetComponent<Bullet>().Init(damage, count, dir);         // 원거리 공격에 맞게 초기화 함수 호출
    }
}
