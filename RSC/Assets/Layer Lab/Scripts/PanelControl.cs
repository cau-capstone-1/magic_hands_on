using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace LayerLab.GUIScripts
{
    public class PanelControl : MonoBehaviour
    {
        private int _page;
        private bool _isReady;
        private TextMeshProUGUI _textTitle;
        [SerializeField] private List<GameObject> defaultPanels = new();
        [SerializeField] private List<GameObject> otherPanels = new();

        [SerializeField] private Transform panelTransformDefault;
        [SerializeField] private Transform panelTransformOther;
        [SerializeField] private Button buttonPrev;
        [SerializeField] private Button buttonNext;

        private bool IsOtherMode { get; set; }

        private void OnValidate()
        {
            var panels = GameObject.Find("Panels");
            if (panels) panelTransformDefault = panels.transform;

            buttonPrev = transform.GetChild(0).GetComponent<Button>();
            buttonNext = transform.GetChild(2).GetComponent<Button>();
        }

        private void Reset()
        {
            OnValidate();
        }

        private void Start()
        {
            _textTitle = transform.GetComponentInChildren<TextMeshProUGUI>();
            buttonPrev.onClick.AddListener(Click_Prev);
            buttonNext.onClick.AddListener(Click_Next);

            foreach (Transform t in panelTransformDefault)
            {
                defaultPanels.Add(t.gameObject);
                t.gameObject.SetActive(false);
            }
            defaultPanels[_page].SetActive(true);

            if (panelTransformOther == null) return;

            foreach (Transform t in panelTransformOther)
            {
                otherPanels.Add(t.gameObject);
                t.gameObject.SetActive(false);
            }

            if (otherPanels.Count > 0) otherPanels[_page].SetActive(true);

            _isReady = true;
            CheckControl();
        }

        private void Update()
        {
            if (defaultPanels.Count <= 0 || !_isReady) return;

#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            {
                Click_Prev();
            }
            else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
            {
                Click_Next();
            }
#else
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Click_Prev();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Click_Next();
            }
#endif
        }

        private void Click_Prev()
        {
            GoToPage(_page - 1);
        }

        private void Click_Next()
        {
            GoToPage(_page + 1);
        }

        // 원하는 페이지로 이동하는 메서드 추가
        public void GoToPage(int pageIndex)
        {
            if (pageIndex < 0 || pageIndex >= defaultPanels.Count) return;

            defaultPanels[_page].SetActive(false);
            if (otherPanels.Count > 0) otherPanels[_page].SetActive(false);

            _page = pageIndex;

            defaultPanels[_page].SetActive(true);
            if (otherPanels.Count > 0) otherPanels[_page].SetActive(true);

            CheckControl();
        }

        private void SetArrowActive()
        {
            buttonPrev.gameObject.SetActive(_page > 0);
            buttonNext.gameObject.SetActive(_page < defaultPanels.Count - 1);
        }

        private void CheckControl()
        {
            if (!IsOtherMode)
            {
                _textTitle.text = defaultPanels[_page].name.Replace("_", " ");
            }
            else
            {
                if (otherPanels.Count > 0)
                {
                    _textTitle.text = otherPanels[_page].name.Replace("_", " ");
                }
            }

            SetArrowActive();
        }

        public void Click_Mode()
        {
            IsOtherMode = !IsOtherMode;
            SetMode();
            CheckControl();
        }

        private void SetMode()
        {
            panelTransformDefault.gameObject.SetActive(!IsOtherMode);
            if (otherPanels.Count > 0) panelTransformOther.gameObject.SetActive(IsOtherMode);
        }
    }
}
