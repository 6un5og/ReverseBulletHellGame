using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;         // static : 정적으로 사용하겠다는 키워드. 바로 메모리에 얹어버림, 스태틱 선언 변수는 인스팩터에 나타나지 않음

    // Header : 인스펙터의 속성들을 예쁘게 구분시켜주는 타이틀
    [Header("# Game Control")] // 게임 시간과 최대 게임 시간을 담당할 변수 선언
    public float gameTime;
    public float maxGameTime = 2 * 10f;

    [Header("# Player Info")] // 적 처치 후 경험치 관련 변수 선언
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 };

    [Header("# GameObject")]
    public PoolManager pool;     // 다양한 곳에서 쉽게 접근할 수 있도록 게임매니저에 풀매니저 추가
    public Player player;

    void Awake()
    {
        instance = this;                        // this 자기 자신 초기화
    }

    void Update()
    {
        gameTime += Time.deltaTime;        // 현재 게임 시간을 계속 더함

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;                  // 최대 게임 시간이 되면 정지
        }

    }

    public void GetExp()
    {
        exp++;

        if (exp == nextExp[level])
        {
            level++;
            exp = 0;
        }
    }
}
