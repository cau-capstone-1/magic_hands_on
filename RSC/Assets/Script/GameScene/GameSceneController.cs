using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameSceneController : MonoBehaviour
{
    [SerializeField] private GameTimer gameTimer; // GameTimer ��ũ��Ʈ�� ����
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
        characterAnimator = character.GetComponent<Animator>(); // Animator ������Ʈ ��������
        tutorialCanvasGroup.alpha = 0;
        dimCanvasGroup.alpha = 1;

        // ActionText�� ������ 0���� �ʱ�ȭ�ϰ� ��ġ�� ����
        InitializeActionText(actionTextGood);
        InitializeActionText(actionTextGreat);
        InitializeActionText(actionTextPerfect);

        // Ʃ�丮�� ���� �ٷ� ���� ����
        StartGame();
    }

    private void StartGame()
    {
        isGameStarted = true;
        StartCoroutine(FadeOutCanvasGroup(dimCanvasGroup, 1.0f)); // Dim ȭ�� ���̵� �ƿ�
        StartCoroutine(SpawnBirds()); // �� ���� ����
        gameTimer.enabled = true; // Ÿ�̸� ����
        gameTimer.ResetTimer(); // Ÿ�̸� �ʱ�ȭ
        Debug.Log("���� ���۵Ǿ����ϴ�.");
    }

    // ActionText�� �ʱ�ȭ�ϴ� �Լ�
    private void InitializeActionText(GameObject actionText)
    {
        CanvasGroup canvasGroup = actionText.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0; // alpha ���� 0���� ����
        }

        RectTransform rectTransform = actionText.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, 0, rectTransform.localPosition.z); // Y ��ġ�� 0���� ����
        }
        else
        {
            Debug.LogWarning($"{actionText.name}�� RectTransform�� �����ϴ�. RectTransform�� �߰��� �ּ���.");
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
            Debug.LogWarning($"{actionText.name}�� CanvasGroup�� �����ϴ�. CanvasGroup�� �߰��� �ּ���.");
        }
    }

    private void InitializeActionTexts()
    {
        // ActionText �ʱ� alpha �� ����
        actionTextGood.GetComponent<CanvasGroup>().alpha = 0;
        actionTextGreat.GetComponent<CanvasGroup>().alpha = 0;
        actionTextPerfect.GetComponent<CanvasGroup>().alpha = 0;
    }

    private IEnumerator TutorialSequence()
    {
        yield return StartCoroutine(FadeInCanvasGroup(tutorialCanvasGroup, 1.0f));
        yield return DisplayDialogue("Ʃ�丮�� ����!", 2.0f);
        yield return DisplayDialogue("�ȳ�! ���� ����̾�. �� ���ӿ� �°� ȯ����!", 4.0f);
        yield return DisplayDialogue("���� ���� Ʃ�丮���� �����غ���!", 4.0f);
        yield return DisplayDialogue("��! ���⿡ ���� ������ ���� ���� �־�.", 4.0f);

        SpawnTutorialBird();
        yield return DisplayDialogue("�� ���� �´� ��ư�� ������!", 4.0f);
        yield return DisplayDialogue("���÷� ����� ���� �����߾�. Z ��ư�� ������ ���� ��ƺ�!", 4.0f);

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Z));
        yield return DisplayDialogue("���߾�! ���� ���������� ������ �����غ���.", 4.0f);

        StartCoroutine(FadeOutCanvasGroup(dimCanvasGroup, 1.0f)); // Dim ȭ�� ���̵� �ƿ�
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
        birdController.Initialize("Yellow", character.transform, 1, Time.time); // Yellow �� ���� �� �ʱ�ȭ, ���� �ð� ���
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

        // �¿� ���� �� ���� ����
        bool spawnLeft = spawnPosition.x < 0;
        string birdColor;

        if (spawnLeft)
        {
            // ���ʿ����� ���, ���� �� �� �ϳ� ��ȯ
            randomIndex = Random.Range(0, 2); // 0 �Ǵ� 1
            birdColor = birdColors[randomIndex];
        }
        else
        {
            // �����ʿ����� �ʷ�, �Ķ� �� �� �ϳ� ��ȯ
            randomIndex = Random.Range(2, 4); // 2 �Ǵ� 3
            birdColor = birdColors[randomIndex];
        }

        birdInstance = Instantiate(birdPrefabs[randomIndex]);
        birdInstance.transform.position = spawnPosition;
        birdInstance.transform.localScale = new Vector3(spawnLeft ? -0.3f : 0.3f, 0.3f, 0.3f);

        BirdController birdController = birdInstance.GetComponent<BirdController>();
        birdController.Initialize(birdColor, character.transform, 3, Time.time); // ���� �ð� ����
        Debug.Log($"Spawned Bird Color: {birdColor}, Initial HP: 3");
    }

    // ���� ���� ������ ���� ActionText ǥ��
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
        // Ű �Է��� �ﰢ������ �����Ͽ� HandleBirdDamage�� ����
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

    private float lastAttackTime = 0f; // ������ ���� �ð��� ����� ����
    private float attackCooldown = 0.3f; // ���� ��ٿ� �ð� (0.3��)

    private void HandleBirdDamage(string color)
    {
        BirdController[] birds = FindObjectsOfType<BirdController>();
        bool damageApplied = false;
        int hitCount = 0; // ���� ���� ����

        foreach (var bird in birds)
        {
            if (bird.BirdColor == color)
            {
                bird.TakeDamage();
                Debug.Log($"Bird Color: {color}, Current HP: {bird.GetCurrentHP()}");
                soundController.PlaySound(1);

                // ���ݹ��� ������ ������ �ִϸ��̼� ����
                StartCoroutine(BlinkBird(bird));

                hitCount++; // ���� ���� ���� ����
                damageApplied = true;
            }
        }

        // ���� ���� ����
        if (damageApplied)
        {
            if (color == "Yellow" || color == "Black")
            {
                character.transform.localScale = new Vector3(-108, 108, 108); // ������ �ٶ�
            }
            else if (color == "Green" || color == "Blue")
            {
                character.transform.localScale = new Vector3(108, 108, 108); // �������� �ٶ�
            }

            PlayAttackAnimation();
            DisplayActionTextBasedOnHits(hitCount); // ���� ���� ������ ���� �ؽ�Ʈ ǥ��
        }
        else
        {
            // ���� ���� ���� ��� ������ �Ա�
            TakeDamage(1);
            Debug.Log("�ش� ������ ���� ���� �÷��̾ ���ظ� �Ծ����ϴ�.");
        }
    }


    private IEnumerator BlinkBird(BirdController bird)
    {
        SpriteRenderer renderer = bird.GetComponent<SpriteRenderer>();

        if (renderer == null) yield break; // SpriteRenderer�� ���� ��� �ڷ�ƾ ����

        for (int i = 0; i < 2; i++)
        {
            if (renderer != null) // renderer�� �����ϴ��� Ȯ��
            {
                renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0); // �����ϰ�
            }
            yield return new WaitForSeconds(0.1f);

            if (renderer != null) // renderer�� �����ϴ��� Ȯ��
            {
                renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1); // �ٽ� �������
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void SkipTutorial()
    {
        if (tutorialCoroutine != null)
        {
            StopCoroutine(tutorialCoroutine); // Ʃ�丮�� ������ �ߴ�
        }

        tutorialText.text = ""; // Ʃ�丮�� �ؽ�Ʈ �ʱ�ȭ
        StartCoroutine(FadeOutCanvasGroup(dimCanvasGroup, 1.0f)); // Dim ȭ�� ���̵� �ƿ�
        StartCoroutine(SpawnBirds()); // �ٷ� ���� ����

        Debug.Log("Ʃ�丮���� ��ŵ�Ǿ����ϴ�.");
    }

    private void PlayAttackAnimation()
    {
        // AttackTrigger �ߵ�
        characterAnimator.SetTrigger("AttackTrigger");
    }

    public void TakeDamage(int amount)
    {
        playerHP -= amount;
        hpText.text = $"{playerHP}";  // .text ������Ƽ�� ���� �ؽ�Ʈ ����
        Debug.Log($"Player HP: {playerHP}");
        StartCoroutine(BlinkCharacter()); // �÷��̾� ������ ȿ��

        if (playerHP <= 0)
        {
            Debug.Log("Game Over");
            // ���� ���� ó�� ���� �߰�
        }
    }

    private IEnumerator BlinkCharacter()
    {
        SpriteRenderer renderer = character.GetComponent<SpriteRenderer>();
        for (int i = 0; i < 2; i++)
        {
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0); // �����ϰ�
            yield return new WaitForSeconds(0.1f);
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1); // �ٽ� �������
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
        // ������ ǥ�õ� ��� ActionText�� ����
        HideAllActionTexts();

        actionText.SetActive(true);
        CanvasGroup canvasGroup = actionText.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0;
        }

        float duration = 0.3f; // ���� ���� �ִϸ��̼� �ð�
        float elapsed = 0f;

        // Alpha �ִϸ��̼� (���� ������ ����)
        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1; // ���������� alpha�� 1�� ����
        yield return new WaitForSeconds(0.3f); // ª�� �ð� ���� ǥ�� ����

        // ������� �ִϸ��̼�
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

    // ��� ActionText�� ����� �Լ�
    private void HideAllActionTexts()
    {
        actionTextGood.SetActive(false);
        actionTextGreat.SetActive(false);
        actionTextPerfect.SetActive(false);

        // ��� �ؽ�Ʈ�� CanvasGroup ���� �ʱ�ȭ
        actionTextGood.GetComponent<CanvasGroup>().alpha = 0;
        actionTextGreat.GetComponent<CanvasGroup>().alpha = 0;
        actionTextPerfect.GetComponent<CanvasGroup>().alpha = 0;
    }
}
