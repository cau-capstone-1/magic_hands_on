using System;
using UnityEngine;

public class Register : MonoBehaviour {
    private GameObject textGuides;
    private GameObject nameInput;
    private GameObject welcome;

    private Boolean onceOnAnimationEndCalled = false;

    void Start() {
        // 씬에 있는 모든 GameObject 찾기 (비활성화된 객체도 포함)
        GameObject[] allGameObjects = FindObjectsOfType<GameObject>(true); // true를 주면 비활성화된 객체도 찾을 수 있습니다.

        foreach (GameObject obj in allGameObjects) {
            if (obj.name == "NameInput") {
                Debug.Log("NameInput을 찾았습니다: " + obj.name);
                nameInput = obj;
                continue;
            } else if (obj.name == "TextGuides") {
                Debug.Log("TextGuides를 찾았습니다: " + obj.name);
                textGuides = obj;
                continue;
            } else if (obj.name == "Welcome") {
                Debug.Log("Welcome을 찾았습니다: " + obj.name);
                welcome = obj;
                continue;
            }
        }

        Debug.Log(nameInput != null);
        nameInput.SetActive(false);
        welcome.SetActive(false);

    }

    void Update() {
        Animation animation = textGuides.GetComponent<Animation>();
        
        if (!animation.isPlaying && !onceOnAnimationEndCalled) {
            onceOnAnimationEndCalled = true;
            onAnimationEnd();
        }
    }


    public void Submit() {
        GameObject input = GameObject.Find("Input");
        string text = input.GetComponent<TMPro.TMP_InputField>().text;

        Debug.Log("Submit" + text);

        if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text)) {
            Debug.Log("이름을 입력해주세요.");
            return;
        }

        GameData.instance.playerName = text;

        nameInput.SetActive(false);
        welcome.SetActive(true);
    }

    void onAnimationEnd() {
        textGuides.SetActive(false);
        nameInput.SetActive(true);
    }
}
