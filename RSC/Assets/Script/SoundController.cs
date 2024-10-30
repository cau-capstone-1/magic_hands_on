using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] private List<AudioClip> audioClips = new(); // ���� ����Ʈ
    private AudioSource audioSource;

    private void Awake()
    {
        // AudioSource ������Ʈ �߰� �� ����
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // ���� �� �ڵ� ��� ����
    }

    // Ư�� ���带 ����ϴ� �޼���
    public void PlaySound(int index)
    {
        if (index >= 0 && index < audioClips.Count)
        {
            audioSource.PlayOneShot(audioClips[index]); // �ε����� �ش��ϴ� ���� ���
        }
        else
        {
            Debug.LogWarning("�߸��� ���� �ε����Դϴ�.");
        }
    }
}
