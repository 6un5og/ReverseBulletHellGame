using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;        // 정적 메모리에 담기 위한 instance 변수 선언

    [Header("# BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;
    AudioHighPassFilter bgmEffect;

    [Header("# SFX")]               // 효과음
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;            // 다량의 효과음을 낼 수 있도록 채널 개수 변수 선언
    AudioSource[] sfxPlayers;       // 많은 소스들이 동시다발적으로 발생
    int channelIndex;               // 맨 마지막에 재생을 한 인덱스의 번호

    public enum Sfx
    {
        // 열거형 데이터는 대응하는 숫자를 지정할 수 있음
        Dead, Hit, LevelUp = 3, Lose, Melee, Range = 7, Select, Win
    }

    void Awake()
    {
        instance = this;
        Init();
    }

    void Init()
    {
        // 배경음 플레이어 초기화 (한 개)
        GameObject bgmObject = new GameObject("bgmPlayer");
        bgmObject.transform.parent = transform;             // 배경음을 담당하는 자식오브젝트를 생성
        bgmPlayer = bgmObject.AddComponent<AudioSource>();  // AddComponent 함수로 오디오 소스를 생성하고 변수에 저장
        bgmPlayer.playOnAwake = false;                      // 캐릭터를 선택하고 bgm이 나와야 함
        bgmPlayer.loop = true;                              // bgm을 계속 루프시킴
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;                           // 실행할 파일
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();


        // 효과음 플레이어 초기화 (채널 개수만큼 여러개)
        GameObject sfxObject = new GameObject("sfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];             // 채널 개수만큼 오디오소스 배열 초기화

        for (int index = 0; index < sfxPlayers.Length; index++) // 배열에 들어갈 컴포넌트 설정
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();  // 반복문으로 모든 효과음 오디오소스 생성하면서 저장
            sfxPlayers[index].playOnAwake = false;
            // AudioHighPassFilter는 Listener Effect계열이기 떄문에 효과음이 영향을 받지 않게 하기 위해서 true로 바꿔줌
            sfxPlayers[index].bypassListenerEffects = true;
            sfxPlayers[index].volume = sfxVolume;
        }
    }

    public void PlayBgm(bool isPlay)
    {
        if (isPlay)
        {
            bgmPlayer.Play();
        }
        else
        {
            bgmPlayer.Stop();
        }
    }

    public void EffectBgm(bool isPlay)
    {
        bgmEffect.enabled = isPlay;
    }

    public void PlaySfx(Sfx sfx)        // 효과음 재생 함수
    {
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length; // 채널 개수만큼 순회하도록 채널인덱스 변수 활용 (무한 루프)

            if (sfxPlayers[loopIndex].isPlaying)                    // 해당 효과음을 재생하고 있다면 (그냥 재생하면 탁음이 나옴)
                continue;                                           // 넘어가기

            int ranIndex = 0;
            if (sfx == Sfx.Hit || sfx == Sfx.Melee)
            {
                ranIndex = Random.Range(0, 2);
            }

            channelIndex = loopIndex;                               // 실행하게 됐기 때문에 루프인덱스로 채널인덱스를 바꿔줌
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];        // sfx배열에 맞는 클립을 enum 값에서 가져와서 형변환 시키고 플레이어 인덱스 번호에 넣음
            sfxPlayers[loopIndex].Play();                           // 재생
            break;                                                  // isPlaying이 안돼있으면 여러개의 플레이어가 똑같은 클립을 재생시킬 수 있어서 break
        }
    }
}
