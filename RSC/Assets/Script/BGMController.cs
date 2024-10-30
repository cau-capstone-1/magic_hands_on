using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    public static BGMController Instance { get; private set; }

    [SerializeField] private List<AudioClip> bgmClips; // BGM 리스트
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 BGM 유지
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true; // BGM은 반복 재생

        // 게임 시작 시 기본 BGM(0번째 BGM) 재생
        PlayBGM(0);
    }

    // 특정 인덱스의 BGM 재생
    public void PlayBGM(int index)
    {
        // 인덱스가 잘못되었거나 기본 BGM이 설정되지 않은 경우 첫 번째 BGM 재생
        if (index < 0 || index >= bgmClips.Count)
        {
            Debug.LogWarning("잘못된 BGM 인덱스입니다. 기본 BGM을 재생합니다.");
            index = 0;
        }

        if (audioSource.clip == bgmClips[index]) return; // 이미 재생 중이면 무시

        audioSource.clip = bgmClips[index];
        audioSource.Play();
    }

    // BGM 정지
    public void StopBGM()
    {
        audioSource.Stop();
    }
}
