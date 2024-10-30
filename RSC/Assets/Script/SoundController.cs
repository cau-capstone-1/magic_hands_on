using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] private List<AudioClip> audioClips = new(); // 사운드 리스트
    private AudioSource audioSource;

    private void Awake()
    {
        // AudioSource 컴포넌트 추가 및 설정
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // 시작 시 자동 재생 방지
    }

    // 특정 사운드를 재생하는 메서드
    public void PlaySound(int index)
    {
        if (index >= 0 && index < audioClips.Count)
        {
            audioSource.PlayOneShot(audioClips[index]); // 인덱스에 해당하는 사운드 재생
        }
        else
        {
            Debug.LogWarning("잘못된 사운드 인덱스입니다.");
        }
    }
}
