using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;         // static : 정적으로 사용하겠다는 키워드. 바로 메모리에 얹어버림, 스태틱 선언 변수는 인스팩터에 나타나지 않음

    // Header : 인스펙터의 속성들을 예쁘게 구분시켜주는 타이틀
    [Header("# Game Control")] // 게임 시간과 최대 게임 시간을 담당할 변수 선언
    public bool isLive;
    public float gameTime;
    public float maxGameTime = 2 * 10f;

    [Header("# Player Info")] // 적 처치 후 경험치 관련 변수 선언
    public int playerId;                // 게임 캐릭터 ID 저장 변수
    public float health;                // 게임메니저의 생명력 관련 변수는 float
    public float maxHealth = 100;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 };

    [Header("# GameObject")]
    public PoolManager pool;     // 다양한 곳에서 쉽게 접근할 수 있도록 게임매니저에 풀매니저 추가
    public Player player;
    public LevelUp uiLevelUp;
    public Result uiResult;
    public GameObject enemyCleaner;

    void Awake()
    {
        instance = this;                        // this 자기 자신 초기화
    }

    public void GameStart(int id)
    {
        playerId = id;

        health = maxHealth;

        player.gameObject.SetActive(true);  // 게임 시작할 때 플레이어 활성화 후 기본 무기 지급

        uiLevelUp.Select(playerId % 2); // 기존 무기 지급을 위한 함수 호출에서 인자 값을 캐릭터ID로 변경
        // 현재 무기가 2개밖에 없기 떄문에 캐릭터가 더 추가된다면 기본무기 지급을 위해 2로 나눈 나머지값을 인자 값으로 넣기

        Resume();                   // 게임 재시작시 타임스케일 1로 만들어주기
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;

        yield return new WaitForSeconds(0.5f);                      // 묘비 조금 볼 시간 벌기

        uiResult.gameObject.SetActive(true);                       // uiResult 보여주기
        uiResult.Lose();
        Stop();
    }

    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        enemyCleaner.SetActive(true);           // 게임 승리 코루틴의 전반부에 적 클리너를 활성화

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);    // LoadScene : 이름 혹은 인덱스로 장면을 새롭게 부르는 함수
    }

    void Update()
    {
        if (!isLive)
            return;

        gameTime += Time.deltaTime;        // 현재 게임 시간을 계속 더함

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;        // 최대 게임 시간이 되면 정지
            GameVictory();                 // 게임 시간이 최대 시간을 넘기는 떄에 게임 승리 함수 호출
        }

    }

    public void GetExp()
    {

        if (!isLive)
            return;

        exp++;

        if (exp == nextExp[Mathf.Min(level, nextExp.Length - 1)])       // min함수를 사용하여 최고 경험치를 그대로 사용하도록 변경
        {
            level++;
            exp = 0;
            uiLevelUp.Show();           // 레벨업 UI 활성화
        }
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;

    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
    }
}
