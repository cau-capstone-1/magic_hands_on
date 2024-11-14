using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameSceneController : MonoBehaviour
{
    [SerializeField] private GameTimer gameTimer; // GameTimer 스크립트를 참조
    [SerializeField] private GameObject character;
    [SerializeField] private Text tutorialText;
    [SerializeField] private CanvasGroup tutorialCanvasGroup;
    [SerializeField] private CanvasGroup dimCanvasGroup;
    [SerializeField] private List<GameObject> birdPrefabs;
    [SerializeField] private float birdSpawnInterval = 3.0f;
    [SerializeField] private int playerHP = 10;
    [SerializeField] private SoundController soundController;
    [SerializeField] private TextMeshProUGUI hpText;

    [SerializeField] private GameObject actionTextGood;
    [SerializeField] private GameObject actionTextGreat;
    [SerializeField] private GameObject actionTextPerfect;

    private Animator characterAnimator;
    private Dictionary<KeyCode, string> keyColorMap = new Dictionary<KeyCode, string>
    {
        { KeyCode.Z, "Yellow" },
        { KeyCode.X, "Black" },
        { KeyCode.C, "Green" },
        { KeyCode.V, "Blue" },
        { KeyCode.B, "Red" },
        { KeyCode.N, "Brown" }
    };

    private Coroutine tutorialCoroutine;
    private List<string> birdColors = new List<string> { "Yellow", "Black", "Green", "Blue" };

    private bool isGameStarted = false;

    private void Start()
    {
        characterAnimator = character.GetComponent<Animator>(); // Animator 컴포넌트 가져오기
        tutorialCanvasGroup.alpha = 0;
        dimCanvasGroup.alpha = 1;

        // ActionText의 투명도를 0으로 초기화하고 위치를 설정
        InitializeActionText(actionTextGood);
        InitializeActionText(actionTextGreat);
        InitializeActionText(actionTextPerfect);

        // 튜토리얼 없이 바로 게임 시작
        StartGame();
    }

    private void StartGame()
    {
        isGameStarted = true;
        StartCoroutine(FadeOutCanvasGroup(dimCanvasGroup, 1.0f)); // Dim 화면 페이드 아웃
        StartCoroutine(SpawnBirds()); // 새 스폰 시작
        gameTimer.enabled = true; // 타이머 시작
        gameTimer.ResetTimer(); // 타이머 초기화
        Debug.Log("게임 시작되었습니다.");
    }

    // ActionText를 초기화하는 함수
    private void InitializeActionText(GameObject actionText)
    {
        CanvasGroup canvasGroup = actionText.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0; // alpha 값을 0으로 설정
        }

        RectTransform rectTransform = actionText.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, 0, rectTransform.localPosition.z); // Y 위치를 0으로 설정
        }
        else
        {
            Debug.LogWarning($"{actionText.name}에 RectTransform이 없습니다. RectTransform을 추가해 주세요.");
        }
    }

    private void SetActionTextAlpha(GameObject actionText, float alpha)
    {
        CanvasGroup canvasGroup = actionText.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = alpha;
        }
        else
        {
            Debug.LogWarning($"{actionText.name}에 CanvasGroup이 없습니다. CanvasGroup을 추가해 주세요.");
        }
    }

    private void InitializeActionTexts()
    {
        // ActionText 초기 alpha 값 설정
        actionTextGood.GetComponent<CanvasGroup>().alpha = 0;
        actionTextGreat.GetComponent<CanvasGroup>().alpha = 0;
        actionTextPerfect.GetComponent<CanvasGroup>().alpha = 0;
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

        StartCoroutine(FadeOutCanvasGroup(dimCanvasGroup, 1.0f)); // Dim 화면 페이드 아웃
        StartCoroutine(SpawnBirds());
    }

    private IEnumerator DisplayDialogue(string message, float delay = 3.0f)
    {
        tutorialText.text = message;
        yield return new WaitForSeconds(delay);
    }

    private IEnumerator FadeInCanvasGroup(CanvasGroup canvasGroup, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }

    private IEnumerator FadeOutCanvasGroup(CanvasGroup canvasGroup, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0f;
        canvasGroup.gameObject.SetActive(false);
    }

    private void SpawnTutorialBird()
    {
        GameObject birdInstance = Instantiate(birdPrefabs[0]);
        birdInstance.transform.position = GetRandomOffScreenPosition();

        BirdController birdController = birdInstance.GetComponent<BirdController>();
        birdController.Initialize("Yellow", character.transform, 1, Time.time); // Yellow 새 생성 및 초기화, 생성 시간 기록
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
        int randomIndex;
        GameObject birdInstance;
        Vector3 spawnPosition = GetRandomOffScreenPosition();

        // 좌우 반전 및 색상 설정
        bool spawnLeft = spawnPosition.x < 0;
        string birdColor;

        if (spawnLeft)
        {
            // 왼쪽에서는 노랑, 검정 새 중 하나 소환
            randomIndex = Random.Range(0, 2); // 0 또는 1
            birdColor = birdColors[randomIndex];
        }
        else
        {
            // 오른쪽에서는 초록, 파랑 새 중 하나 소환
            randomIndex = Random.Range(2, 4); // 2 또는 3
            birdColor = birdColors[randomIndex];
        }

        birdInstance = Instantiate(birdPrefabs[randomIndex]);
        birdInstance.transform.position = spawnPosition;
        birdInstance.transform.localScale = new Vector3(spawnLeft ? -0.3f : 0.3f, 0.3f, 0.3f);

        BirdController birdController = birdInstance.GetComponent<BirdController>();
        birdController.Initialize(birdColor, character.transform, 3, Time.time); // 생성 시간 전달
        Debug.Log($"Spawned Bird Color: {birdColor}, Initial HP: 3");
    }

    // 맞춘 새의 개수에 따라 ActionText 표시
    private void DisplayActionTextBasedOnHits(int hitCount)
    {
        if (hitCount == 1)
        {
            DisplayActionText("Good");
        }
        else if (hitCount == 2)
        {
            DisplayActionText("Great");
        }
        else if (hitCount >= 3)
        {
            DisplayActionText("Perfect");
        }
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
        // 키 입력을 즉각적으로 감지하여 HandleBirdDamage에 전달
        if (Input.GetKeyDown(KeyCode.Z)) HandleBirdDamage("Yellow");
        if (Input.GetKeyDown(KeyCode.X)) HandleBirdDamage("Black");
        if (Input.GetKeyDown(KeyCode.C)) HandleBirdDamage("Green");
        if (Input.GetKeyDown(KeyCode.V)) HandleBirdDamage("Blue");
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

    private float lastAttackTime = 0f; // 마지막 공격 시간이 저장될 변수
    private float attackCooldown = 0.3f; // 공격 쿨다운 시간 (0.3초)

    private void HandleBirdDamage(string color)
    {
        BirdController[] birds = FindObjectsOfType<BirdController>();
        bool damageApplied = false;
        int hitCount = 0; // 맞춘 새의 개수

        foreach (var bird in birds)
        {
            if (bird.BirdColor == color)
            {
                bird.TakeDamage();
                Debug.Log($"Bird Color: {color}, Current HP: {bird.GetCurrentHP()}");
                soundController.PlaySound(1);

                // 공격받은 새에게 깜빡임 애니메이션 적용
                StartCoroutine(BlinkBird(bird));

                hitCount++; // 맞춘 새의 개수 증가
                damageApplied = true;
            }
        }

        // 공격 방향 설정
        if (damageApplied)
        {
            if (color == "Yellow" || color == "Black")
            {
                character.transform.localScale = new Vector3(-108, 108, 108); // 왼쪽을 바라봄
            }
            else if (color == "Green" || color == "Blue")
            {
                character.transform.localScale = new Vector3(108, 108, 108); // 오른쪽을 바라봄
            }

            PlayAttackAnimation();
            DisplayActionTextBasedOnHits(hitCount); // 맞춘 새의 개수에 따라 텍스트 표시
        }
        else
        {
            // 맞춘 새가 없을 경우 데미지 입기
            TakeDamage(1);
            Debug.Log("해당 색상의 새가 없어 플레이어가 피해를 입었습니다.");
        }
    }


    private IEnumerator BlinkBird(BirdController bird)
    {
        SpriteRenderer renderer = bird.GetComponent<SpriteRenderer>();

        if (renderer == null) yield break; // SpriteRenderer가 없는 경우 코루틴 종료

        for (int i = 0; i < 2; i++)
        {
            if (renderer != null) // renderer가 존재하는지 확인
            {
                renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0); // 투명하게
            }
            yield return new WaitForSeconds(0.1f);

            if (renderer != null) // renderer가 존재하는지 확인
            {
                renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1); // 다시 원래대로
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void SkipTutorial()
    {
        if (tutorialCoroutine != null)
        {
            StopCoroutine(tutorialCoroutine); // 튜토리얼 시퀀스 중단
        }

        tutorialText.text = ""; // 튜토리얼 텍스트 초기화
        StartCoroutine(FadeOutCanvasGroup(dimCanvasGroup, 1.0f)); // Dim 화면 페이드 아웃
        StartCoroutine(SpawnBirds()); // 바로 게임 시작

        Debug.Log("튜토리얼이 스킵되었습니다.");
    }

    private void PlayAttackAnimation()
    {
        // AttackTrigger 발동
        characterAnimator.SetTrigger("AttackTrigger");
    }

    public void TakeDamage(int amount)
    {
        playerHP -= amount;
        hpText.text = $"{playerHP}";  // .text 프로퍼티를 통해 텍스트 설정
        Debug.Log($"Player HP: {playerHP}");
        StartCoroutine(BlinkCharacter()); // 플레이어 깜빡임 효과

        if (playerHP <= 0)
        {
            Debug.Log("Game Over");
            // 게임 오버 처리 로직 추가
        }
    }

    private IEnumerator BlinkCharacter()
    {
        SpriteRenderer renderer = character.GetComponent<SpriteRenderer>();
        for (int i = 0; i < 2; i++)
        {
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0); // 투명하게
            yield return new WaitForSeconds(0.1f);
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1); // 다시 원래대로
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void DisplayActionText(string actionType)
    {
        GameObject actionText = null;

        switch (actionType)
        {
            case "Perfect":
                actionText = actionTextPerfect;
                break;
            case "Great":
                actionText = actionTextGreat;
                break;
            case "Good":
                actionText = actionTextGood;
                break;
        }

        if (actionText != null)
        {
            StartCoroutine(AnimateActionText(actionText));
        }
    }

    public void DisplayActionTextBasedOnTime(BirdController bird)
    {
        float elapsedTime = Time.time - bird.SpawnTime;
        if (elapsedTime < 2f)
        {
            DisplayActionText("Perfect");
        }
        else if (elapsedTime < 4f)
        {
            DisplayActionText("Great");
        }
        else
        {
            DisplayActionText("Good");
        }
    }


    private IEnumerator AnimateActionText(GameObject actionText)
    {
        // 기존에 표시된 모든 ActionText를 숨김
        HideAllActionTexts();

        actionText.SetActive(true);
        CanvasGroup canvasGroup = actionText.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0;
        }

        float duration = 0.3f; // 투명도 변경 애니메이션 시간
        float elapsed = 0f;

        // Alpha 애니메이션 (투명도 조정만 수행)
        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1; // 최종적으로 alpha를 1로 설정
        yield return new WaitForSeconds(0.3f); // 짧은 시간 동안 표시 유지

        // 사라지는 애니메이션
        elapsed = 0f;
        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(1, 0, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0;
        actionText.SetActive(false);
    }

    // 모든 ActionText를 숨기는 함수
    private void HideAllActionTexts()
    {
        actionTextGood.SetActive(false);
        actionTextGreat.SetActive(false);
        actionTextPerfect.SetActive(false);

        // 모든 텍스트의 CanvasGroup 투명도 초기화
        actionTextGood.GetComponent<CanvasGroup>().alpha = 0;
        actionTextGreat.GetComponent<CanvasGroup>().alpha = 0;
        actionTextPerfect.GetComponent<CanvasGroup>().alpha = 0;
    }
}
