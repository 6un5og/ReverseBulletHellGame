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

    float timer;                        // 소환 타이머 변수
    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();      // 배열 초기화 GetComponentsInChildren
    }

    void Update()
    {
        timer += Time.deltaTime;        // 시간을 계속 더함

        if (timer > 0.2f)
        {
            timer = 0;                  // 원래대로 초기화
            Spawn();                    // 타이머가 일정 시간에 도달하면 소환하도록 작성
        }

    }

    void Spawn()
    {
        GameObject enemy = GameManager.instance.pool.Get(Random.Range(0, 2));        // 풀 함수에는 랜덤 인자 값을 넣도록 변경
        // GetComponentsInChildren 함수는 본인도 포함이기 떄문에 자식 오브젝트에서만 선택되도록 랜덤 시작을 1부터 만듦
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;                  // 만들어둔 소환 위치 중 하나로 배치되도록 작성
    }

}
