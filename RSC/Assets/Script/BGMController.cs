using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    public static BGMController Instance { get; private set; }

    [SerializeField] private List<AudioClip> bgmClips; // BGM ����Ʈ
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� BGM ����
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true; // BGM�� �ݺ� ���

        // ���� ���� �� �⺻ BGM(0��° BGM) ���
        PlayBGM(0);
    }

    // Ư�� �ε����� BGM ���
    public void PlayBGM(int index)
    {
        // �ε����� �߸��Ǿ��ų� �⺻ BGM�� �������� ���� ��� ù ��° BGM ���
        if (index < 0 || index >= bgmClips.Count)
        {
            Debug.LogWarning("�߸��� BGM �ε����Դϴ�. �⺻ BGM�� ����մϴ�.");
            index = 0;
        }

        if (audioSource.clip == bgmClips[index]) return; // �̹� ��� ���̸� ����

        audioSource.clip = bgmClips[index];
        audioSource.Play();
    }

    // BGM ����
    public void StopBGM()
    {
        audioSource.Stop();
    }
}
