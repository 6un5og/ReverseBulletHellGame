using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // void Start()     게임매니저를 통해 호출하는 모습
    // {
    //     GameManager.instance.pool.Get(0);
    // }

    public Transform[] spawnPoint;      // 자식 오브젝트의 트랜스폼을 담을 배열 변수 선언
    public SpawnData[] spawnData;

    int level;
    float timer;                        // 소환 타이머 변수
    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();      // 배열 초기화 GetComponentsInChildren
    }

    void Update()
    {
        timer += Time.deltaTime;        // 시간을 계속 더함
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 10f), spawnData.Length - 1);

        // 레벨이 0이면 소환 타이밍이 0.5초
        if (timer > spawnData[level].spawnTime)
        {
            timer = 0;                  // 원래대로 초기화
            Spawn();                    // 타이머가 일정 시간에 도달하면 소환하도록 작성
        }

    }

    void Spawn()
    {
        GameObject enemy = GameManager.instance.pool.Get(0);        // 풀 함수에는 랜덤 인자 값을 넣도록 변경   .. 레벨값에 따라 풀링
        // GetComponentsInChildren 함수는 본인도 포함이기 떄문에 자식 오브젝트에서만 선택되도록 랜덤 시작을 1부터 만듦
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;                  // 만들어둔 소환 위치 중 하나로 배치되도록 작성
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }

}
// 코드 위에 [] 가 있으면 속성자라는 뜻
[System.Serializable]
// 소환 데이터를 담당하는 클래스 선언
public class SpawnData
{

    // 추가할 속성들 : 스프라이트 타입, 소환 시간, 체력, 속도
    public float spawnTime;
    // 밑 세개는 enemy클래스에서 호출해서 씀
    public int spriteType;
    public int health;
    public float speed;
}
