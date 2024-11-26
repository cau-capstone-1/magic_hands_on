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
    [SerializeField] private int birdHp = 3;
    [SerializeField] private int playerHP = 10;
    [SerializeField] private SoundController soundController;
    [SerializeField] private TextMeshProUGUI hpText;

    [SerializeField] private GameObject actionTextGood;
    [SerializeField] private GameObject actionTextGreat;
    [SerializeField] private GameObject actionTextPerfect;

    [SerializeField] private CanvasGroup GameOverCanvasGroup;
    [SerializeField] private CanvasGroup GameClearCanvasGroup;
    [SerializeField] private Transform parentObject; // 새가 생성될 부모 오브젝트

    [SerializeField] private Slider leftSlider; // 왼쪽 슬라이더
    [SerializeField] private Slider rightSlider; // 오른쪽 슬라이더
    [SerializeField] private Button leftSliderButton; // 왼쪽 슬라이더의 버튼
    [SerializeField] private Button rightSliderButton; // 오른쪽 슬라이더의 버튼

    [SerializeField] private PanelControl Panel;

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
        GameOverCanvasGroup.alpha = 0; // 게임 시작 시 GameOverCanvasGroup 숨기기
        GameOverCanvasGroup.gameObject.SetActive(false);
        GameClearCanvasGroup.alpha = 0;
        GameClearCanvasGroup.gameObject.SetActive(false);

        // 슬라이더 초기화
        leftSlider.value = 0;
        rightSlider.value = 0;

        // 버튼 비활성화 (게이지가 가득 찰 때만 활성화됨)
        leftSliderButton.interactable = false;
        rightSliderButton.interactable = false;

        // 버튼 이벤트 설정
        leftSliderButton.onClick.AddListener(DealDamageToAllBirds);
        rightSliderButton.onClick.AddListener(HealPlayerHP);

        // ActionText의 투명도를 0으로 초기화하고 위치를 설정
        InitializeActionText(actionTextGood);
        InitializeActionText(actionTextGreat);
        InitializeActionText(actionTextPerfect);

        // 튜토리얼 없이 바로 게임 시작
        StartGame();

        // idle 애니메이션 반복 재생
        PlayIdleAnimation();
    }

    // Idle 애니메이션을 반복 재생하는 메서드
    private void PlayIdleAnimation()
    {
        characterAnimator.Play("penguin_idle"); // Animator에서 penguin_idle 애니메이션 실행
        characterAnimator.SetBool("isIdle", true); // 계속 반복되도록 isIdle 플래그 설정
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

    // FadeIn 메서드로 GameOverCanvasGroup 보이게 하기
    private IEnumerator FadeInCanvasGroup(CanvasGroup canvasGroup, float duration)
    {
        canvasGroup.gameObject.SetActive(true); // 캔버스 그룹 활성화
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
        while (isGameStarted) // 게임이 시작된 상태일 때만 새를 소환
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

        // Instantiate 메서드에서 부모 오브젝트를 설정
        birdInstance = Instantiate(birdPrefabs[randomIndex], spawnPosition, Quaternion.identity, parentObject);

        birdInstance.transform.localScale = new Vector3(spawnLeft ? -0.3f : 0.3f, 0.3f, 0.3f);

        BirdController birdController = birdInstance.GetComponent<BirdController>();
        birdController.Initialize(birdColor, character.transform, birdHp, Time.time); // 생성 시간 전달
     
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

        float x = Random.Range(-screenHalfWidth * 1.3f, screenHalfWidth * 1.3f);
        float y = Random.Range(-screenHalfHeight * 1.3f, screenHalfHeight * 1.3f);

        if (x > -screenHalfWidth && x < screenHalfWidth) x = x < 0 ? -screenHalfWidth * 1.3f : screenHalfWidth * 1.3f;
        if (y > -screenHalfHeight && y < screenHalfHeight) y = y < 0 ? -screenHalfHeight * 1.3f : screenHalfHeight * 1.3f;

        return new Vector3(x, y, 0);
    }

    private void Update()
    {
        if (!isGameStarted) return; // 게임이 시작된 상태에서만 업데이트
        if (gameTimer.elapsedTime >= 120) GameDone();

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

    // HandleBirdDamage 메서드에서도 게임 오버 상태 확인 추가
    private void HandleBirdDamage(string color)
    {
        if (!isGameStarted) return; // 게임 오버 상태에서는 새에게 피해를 줄 수 없음

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

                // 슬라이더 게이지 증가
                if (color == "Yellow" || color == "Black")
                {
                    leftSlider.value += 6; // 왼쪽 슬라이더 게이지 증가
                    if (leftSlider.value >= 100)
                    {
                        leftSliderButton.interactable = true; // 게이지가 가득 차면 버튼 활성화
                    }
                }
                else if (color == "Green" || color == "Blue")
                {
                    rightSlider.value += 6; // 오른쪽 슬라이더 게이지 증가
                    if (rightSlider.value >= 100)
                    {
                        rightSliderButton.interactable = true; // 게이지가 가득 차면 버튼 활성화
                    }
                }
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

    private void DealDamageToAllBirds()
    {
        BirdController[] birds = FindObjectsOfType<BirdController>();
        foreach (var bird in birds)
        {
            bird.TakeDamage(); // 모든 새에게 데미지 1 주기
            Debug.Log($"All Birds Damaged, Bird Color: {bird.BirdColor}, Current HP: {bird.GetCurrentHP()}");
        }

        leftSlider.value = 0; // 슬라이더 초기화
        leftSliderButton.interactable = false;
    }

    private void HealPlayerHP()
    {
        playerHP += 2;
        hpText.text = $"{playerHP}";
        Debug.Log($"Player HP increased to: {playerHP}");

        rightSlider.value = 0; // 슬라이더 초기화
        rightSliderButton.interactable = false;
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
            GameOver(); // 게임 오버 처리 메서드 호출
        }
    }

    public void GameOver()
    {
        isGameStarted = false; // 게임이 종료 상태임을 표시
        gameTimer.enabled = false; // 타이머 중지
        StartCoroutine(FadeInCanvasGroup(GameOverCanvasGroup, 1.0f)); // GameOverCanvasGroup 
        GameOverCanvasGroup.gameObject.SetActive(true); // GameOver 화면 표시

        // BirdParent 하위 모든 오브젝트 삭제
        foreach (Transform child in parentObject)
        {
            Destroy(child.gameObject);
        }

        Debug.Log("게임 오버");

        // 5초 뒤에 다른 함수 호출
        StartCoroutine(ExecuteAfterDelay(5.0f, BackToPanelStage));
    }

    public void GameDone()
    {
        isGameStarted = false; // 게임이 종료 상태임을 표시
        gameTimer.enabled = false; // 타이머 중지
        StartCoroutine(FadeInCanvasGroup(GameClearCanvasGroup, 1.0f)); // GameClearCanvasGroup 
        GameClearCanvasGroup.gameObject.SetActive(true); // GameOver 화면 표시

        // BirdParent 하위 모든 오브젝트 삭제
        foreach (Transform child in parentObject)
        {
            Destroy(child.gameObject);
        }

        Debug.Log("게임 완료");

        // 5초 뒤에 다른 함수 호출
        StartCoroutine(ExecuteAfterDelay(5.0f, BackToPanelStage));
    }

    public void BackToPanelStage()
    {
        Panel.ShowPanel(2);
    }

    // 지연 시간을 두고 특정 함수를 실행하는 코루틴
    private IEnumerator ExecuteAfterDelay(float delay, System.Action action)
    {
        yield return new WaitForSeconds(delay); // 지연 시간 대기
        action?.Invoke(); // 전달된 함수를 실행
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

    public void RestartGame()
    {
        // 게임 상태 초기화
        isGameStarted = false;

        // 플레이어 HP 초기화
        playerHP = 10;
        hpText.text = $"{playerHP}";

        // 슬라이더 초기화
        leftSlider.value = 0;
        rightSlider.value = 0;
        leftSliderButton.interactable = false;
        rightSliderButton.interactable = false;

        // BirdParent 하위 모든 오브젝트 삭제
        foreach (Transform child in parentObject)
        {
            Destroy(child.gameObject);
        }

        // GameOver UI 숨기기
        GameOverCanvasGroup.alpha = 0;
        GameOverCanvasGroup.gameObject.SetActive(false);

        // 타이머 초기화
        gameTimer.ResetTimer();
        gameTimer.enabled = true;

        // 캐릭터 애니메이션 초기화
        PlayIdleAnimation();

        // Dim 화면 페이드 인 후 다시 게임 시작
        StartCoroutine(RestartSequence());
    }

    // Dim 화면 페이드 인 후 게임 재시작
    private IEnumerator RestartSequence()
    {
        yield return StartCoroutine(FadeInCanvasGroup(dimCanvasGroup, 1.0f));
        StartGame(); // 게임 시작
    }
}
