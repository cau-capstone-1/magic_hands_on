using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneController : MonoBehaviour
{
    [SerializeField] private GameObject character;
    [SerializeField] private Text tutorialText;
    [SerializeField] private CanvasGroup tutorialCanvasGroup;
    [SerializeField] private CanvasGroup dimCanvasGroup;
    [SerializeField] private List<GameObject> birdPrefabs;
    [SerializeField] private float birdSpawnInterval = 3.0f;

    private Dictionary<KeyCode, string> keyColorMap = new Dictionary<KeyCode, string>
    {
        { KeyCode.Z, "Yellow" },
        { KeyCode.X, "Black" },
        { KeyCode.C, "Green" },
        { KeyCode.V, "Blue" },
        { KeyCode.B, "Red" },
        { KeyCode.N, "Brown" }
    };

    private void Start()
    {
        tutorialCanvasGroup.alpha = 0;
        dimCanvasGroup.alpha = 1;
        StartCoroutine(TutorialSequence());
    }

    private IEnumerator DisplayDialogue(string message, float delay = 3.0f)
    {
        tutorialText.text = message;
        yield return new WaitForSeconds(delay);
    }

    private IEnumerator TutorialSequence()
    {
        yield return StartCoroutine(FadeInCanvasGroup(tutorialCanvasGroup, 1.0f));
        yield return DisplayDialogue("튜토리얼 시작!", 2.0f);
        yield return DisplayDialogue("안녕! 나는 펭귄이야. 이 게임에 온걸 환영해!", 4.0f);
        yield return DisplayDialogue("나랑 같이 튜토리얼을 진행해보자!", 4.0f);
        yield return DisplayDialogue("어! 저기에 나를 잡으러 오는 새가 있어.", 4.0f);

        SpawnTutorialBird();
        yield return DisplayDialogue("새 색깔에 맞는 버튼을 눌러줘!", 4.0f);
        yield return DisplayDialogue("예시로 노란색 새가 등장했어. Z 버튼을 눌러서 새를 잡아봐!", 4.0f);

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Z));
        yield return DisplayDialogue("잘했어! 이제 본격적으로 게임을 시작해보자.", 4.0f);

        StartCoroutine(FadeOutCanvasGroup(dimCanvasGroup, 1.0f));
        StartCoroutine(SpawnBirds());
    }

    private IEnumerator FadeInCanvasGroup(CanvasGroup canvasGroup, float duration)
    {
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1;
    }

    private IEnumerator FadeOutCanvasGroup(CanvasGroup canvasGroup, float duration)
    {
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0;
        canvasGroup.gameObject.SetActive(false);
    }

    private void SpawnTutorialBird()
    {
        GameObject birdInstance = Instantiate(birdPrefabs[0]);
        birdInstance.transform.position = GetRandomOffScreenPosition();

        BirdController birdController = birdInstance.GetComponent<BirdController>();
        birdController.Initialize("Yellow", character.transform, 1); // Yellow 새 생성 및 초기화
    }

    private IEnumerator SpawnBirds()
    {
        while (true)
        {
            SpawnRandomBird();
            yield return new WaitForSeconds(birdSpawnInterval);
        }
    }

    private void SpawnRandomBird()
    {
        int randomIndex = Random.Range(0, birdPrefabs.Count);
        GameObject birdInstance = Instantiate(birdPrefabs[randomIndex]);
        Vector3 spawnPosition = GetRandomOffScreenPosition();

        // 좌우 반전 적용
        bool spawnLeft = spawnPosition.x < 0;
        birdInstance.transform.position = spawnPosition;
        birdInstance.transform.localScale = new Vector3(spawnLeft ? -0.3 : 0.3, 0.3, 0.3);

        BirdController birdController = birdInstance.GetComponent<BirdController>();
        birdController.Initialize(birdController.BirdColor, character.transform, 3); // 예시로 HP를 3으로 설정
    }

    private Vector3 GetRandomOffScreenPosition()
    {
        float screenHalfWidth = Camera.main.aspect * Camera.main.orthographicSize;
        float screenHalfHeight = Camera.main.orthographicSize;

        float x = Random.Range(-screenHalfWidth * 1.5f, screenHalfWidth * 1.5f);
        float y = Random.Range(-screenHalfHeight * 1.5f, screenHalfHeight * 1.5f);

        if (x > -screenHalfWidth && x < screenHalfWidth) x = x < 0 ? -screenHalfWidth * 1.5f : screenHalfWidth * 1.5f;
        if (y > -screenHalfHeight && y < screenHalfHeight) y = y < 0 ? -screenHalfHeight * 1.5f : screenHalfHeight * 1.5f;

        return new Vector3(x, y, 0);
    }

    private void Update()
    {
        CheckInputForBirds();
    }

    private void CheckInputForBirds()
    {
        foreach (var entry in keyColorMap)
        {
            if (Input.GetKeyDown(entry.Key))
            {
                HandleBirdDamage(entry.Value);
            }
        }
    }

    private void HandleBirdDamage(string color)
    {
        BirdController[] birds = FindObjectsOfType<BirdController>();
        foreach (var bird in birds)
        {
            if (bird.BirdColor == color)
            {
                bird.TakeDamage();
                break;
            }
        }
    }
}
