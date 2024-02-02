using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchiveManager : MonoBehaviour
{
    public GameObject[] lockCharacter;          // 잠금 캐릭터와 해금 캐릭터 나눠서 저장
    public GameObject[] unlockCharacter;
    public GameObject uiNotice;                 // 알림 오브젝트를 저장할 변수 선언

    enum Achive { UnlockPotato, UnlockBean }    // 업적 데이터 설정을 위해 열거형 enum으로 관리
    Achive[] achives;                           // 업적 데이터들을 저장해둘 배열 선언
    WaitForSecondsRealtime wait;                // timeScale이 0일때에도 루틴이 돌았다가 사라져야 하기 때문에 뒤에 Realtime 붙어있는걸로 쓰기

    void Awake()
    {
        // Enum.GetValues : 주어진 열거형의 데이터를 모두 가져오는 함수
        // typeof(enum이름) : enum 이름의 데이터를 쓰겠다는 뜻
        achives = (Achive[])Enum.GetValues(typeof(Achive));     // 명시적으로 지정하여 타입 맞추기 (위에 선언해준 대로)
        wait = new WaitForSecondsRealtime(5);
        if (!PlayerPrefs.HasKey("MyData"))          // HasKey 함수로 데이터 유무 체크 후 초기화 실행
        {
            Init();
        }

    }

    void Init()
    {
        // PlayerPrefs : 간단한 저장 기능을 제공하는 유니티 제공 클래스
        // SetInt 함수를 사용하여 key와 연결된 int형 데이터를 저장
        PlayerPrefs.SetInt("MyData", 1);

        // 효율성을 위해 foreach문 돌리기
        foreach (Achive achive in achives)
        {
            PlayerPrefs.SetInt(achive.ToString(), 0);   // ToString() 으로 문자열로 바꿔서 넣어주기 (0은 잠금캐릭터 1은 해금캐릭터)
        }
    }

    void Start()
    {
        UnlockCharacter();
    }

    void UnlockCharacter()      // 캐릭터 해금 함수
    {
        for (int index = 0; index < lockCharacter.Length; index++)
        {
            string achiveName = achives[index].ToString();  // 잠금 버튼 배열 순회하면서 인덱스에 해당하는 업적 이름 가져오기
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;    // bool 변수 선언 및 초기화로 active 상태 바꾸기(bool이기 때문에 0 아니면 1)
            lockCharacter[index].SetActive(!isUnlock);
            unlockCharacter[index].SetActive(isUnlock);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        foreach (Achive achive in achives)
        {
            CheckAchive(achive);
        }
    }

    void CheckAchive(Achive achive)     // 업적 달성 체크 함수
    {
        bool isAchive = false;

        switch (achive)
        {
            case Achive.UnlockPotato:
                if (GameManager.instance.isLive)
                {
                    // 게임 성공 시 전멸폭탄이 터져서 생존해도 해금되기 때문에 예외처리
                    isAchive = GameManager.instance.kill >= 10;
                }
                break;
            case Achive.UnlockBean:
                isAchive = GameManager.instance.gameTime == GameManager.instance.maxGameTime;
                break;
        }

        // 해당 업적이 처음 달성했다는 조건을 if문에 작성
        if (isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0) // 아직 해금이 안됐다(== 0)
        {
            PlayerPrefs.SetInt(achive.ToString(), 1);

            for (int index = 0; index < uiNotice.transform.childCount; index++)     // uiNotice의 자식 오브젝트 개수만큼
            {
                bool isActive = index == (int)achive;      // 활성화 조건(index가 achive의 순번과 일치하면 활성화),(achive를 int로 강제형변환)
                uiNotice.transform.GetChild(index).gameObject.SetActive(isActive);  // transform 들어가서 gameObject로 가야 활성화 가능
            }

            StartCoroutine(NoticeRoutine());
        }
    }

    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);

        yield return wait; // 코루틴 리턴 쓸 때는 항상 변수로 초기화 해주기 (최적화에 도움)

        uiNotice.SetActive(false);
    }
}
