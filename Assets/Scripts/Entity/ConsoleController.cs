using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class DialogueEntry
{
    public string message;
    public bool isChoice;
    public bool isEnd;
    public List<Choice> choices;

    [System.Serializable]
    public class Choice
    {
        public string text;
        public int nextIndex;
        
        [NonSerialized] public Action onChosen;
    }
}

public class ConsoleController : MonoBehaviour
{
    [SerializeField] private List<DialogueEntry> _dialogues;
    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private GameObject _messagePanel;
    [SerializeField] private GameObject _choicePanel;
    [SerializeField] private GameObject _choiceButtonPrefab;

    private int _currentIndex = 0;
    public bool isActive = false;
    private bool removeObj = false;


    private void Start()
    {
        if (GameManager.IsObjectDestroyed(this.name))
        {
            Destroy(this.gameObject);
            return;
        }
    }

    public void TriggerMessage()
    {
        if (!isActive)
        {
            isActive = true;
            _currentIndex = 0;
            _messagePanel.SetActive(true);
            AssignActionsToChoice();
            ShowDialogue();
        }
        else
        {
            if (_dialogues[_currentIndex].isChoice) return;
            _currentIndex++;
            if (_currentIndex < _dialogues.Count)
                ShowDialogue();
            else
                EndDialogue();
        }
    }

    private void ShowDialogue()
    {
        var entry = _dialogues[_currentIndex];
        _messageText.text = entry.message;

        if (entry.isEnd)
        {
            EndDialogue();
            return;
        }
        
        if (entry.isChoice)
        {
            _choicePanel.SetActive(true);
            foreach (Transform child in _choicePanel.transform)
                Destroy(child.gameObject);

            foreach (var choice in entry.choices)
            {
                GameObject button = Instantiate(_choiceButtonPrefab, _choicePanel.transform);
                button.GetComponentInChildren<TextMeshProUGUI>().text = choice.text;

                button.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
                {
                    choice.onChosen?.Invoke();
                    _currentIndex = choice.nextIndex;
                    _choicePanel.SetActive(false);
                    ShowDialogue();
                });
            }
        }
        else
        {
            _choicePanel.SetActive(false);
        }
    }

    private void EndDialogue()
    {
        isActive = false;
        _messagePanel.SetActive(false);
        _choicePanel.SetActive(false);
        if (removeObj) Destroy(this.gameObject);
    }

    private void AssignActionsToChoice()
    {
        foreach (var dialogue in _dialogues)
        {
            if (!dialogue.isChoice) continue;

            foreach (var choice in dialogue.choices)
            {
                switch (choice.text)
                {
                    case "던전 입장":
                        choice.onChosen = EntranceDungeon;
                        break;
                    case "동료로 영입한다 (10Wave)":
                        choice.onChosen = () => Recruit(10, "Elf_F");
                        break;
                    case "동료로 영입한다 (15Wave)":
                        choice.onChosen = () => Recruit(15, "Dwarf_F");
                        break;
                    case "동료로 영입한다 (20Wave)":
                        choice.onChosen = () => Recruit(20, "Wizzard_M");
                        break;
                    case "상자를 열어본다":
                        choice.onChosen = () => ChestOpen(20);
                        break;
                    default:
                        choice.onChosen = null;
                        break;
                }
            }
        }
    }

    public void EntranceDungeon()
    {
        GameManager.instance.UIManager.EntranceDungeon();
    }

    public void Recruit(int needWave, string prefabName)
    {
        if (GameManager.instance.BestScore < needWave)
        {
            Debug.Log("동료 영입 실패");
            return;
        }
        
        Debug.Log("동료 영입 성공");
        _dialogues[_currentIndex + 1].message = "영입에 성공했습니다.";
        GameManager.LoadFriendPrefab(prefabName);
        GameManager.MarkObjectAsDestroyed(this.name);
        Debug.Log("친구수 = " + GameManager.Friends.Count);
        removeObj = true;
    }

    public void ChestOpen(int needWave)
    {
        if (needWave > GameManager.instance.BestScore)
        {
            Debug.Log("못열음");
            return;
        }
        
        Animator animator;
        animator = GetComponentInChildren<Animator>();
        
        int isOpen = Animator.StringToHash("IsOpen");
        animator.SetBool(isOpen, true);

        GameManager.MarkObjectAsDestroyed(this.name);
        GameManager._hasGoldenSword = true;
        GameManager.instance.Player.ChangeWeapon();
        _dialogues[_currentIndex + 1].message = "최강의 검을 얻었습니다.";
        removeObj = true;
    }
}

