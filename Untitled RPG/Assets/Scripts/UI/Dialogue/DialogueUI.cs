using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace RPG.DialogueSystem.UI
{
    public class DialogueUI : MonoBehaviour
    {
        private PlayerConversant playerConversant;
        [SerializeField] private TextMeshProUGUI aiText;
        [FormerlySerializedAs("nextButton")] [SerializeField] private Button continueButton;

        [Header("Choices")] 
        [SerializeField] private Transform choicesContainer;
        [SerializeField] private Transform choicesButtonContainer;
        [SerializeField] private Button choiceButtonPrefab;

        [Space, SerializeField] private Button quitButton;

        private void Start()
        {
            playerConversant = GameObject.FindWithTag("Player").GetComponent<PlayerConversant>();
            playerConversant.OnConversationUpdated += UpdateUI;
            
            continueButton.onClick.AddListener(OnContinueButtonClicked);
            quitButton?.onClick.AddListener(playerConversant.QuitConversation);
            
            UpdateUI();
        }

        private void OnDestroy()
        {
            playerConversant.OnConversationUpdated -= UpdateUI;
            continueButton.onClick.RemoveAllListeners();
            quitButton?.onClick.RemoveAllListeners();
        }

        private void OnContinueButtonClicked()
        {
            playerConversant.Next();
        }

        void UpdateUI()
        {
            gameObject.SetActive(playerConversant.IsActive);

            if (!playerConversant.IsActive)
                return;
            
            continueButton.gameObject.SetActive(playerConversant.HasNext() && !playerConversant.HasChoices);
            choicesContainer.gameObject.SetActive(playerConversant.HasChoices);
            
            quitButton.gameObject.SetActive(!playerConversant.HasNext());

            if (playerConversant.HasChoices)
            {
                BuildChoiceList();
            }
            else
            {
                aiText.text = string.Empty;
                string message = playerConversant.GetText();
                DOTween.To(() => aiText.text, x => aiText.text = x, message, 2f).SetEase(Ease.Linear);
            }
        }

        private void BuildChoiceList()
        {
            foreach (Transform choice in choicesButtonContainer)
            {
                Destroy(choice.gameObject);
            }

            foreach (DialogueNode choiceNode in playerConversant.GetChoices())
            {
                Button choiceButton = Instantiate(choiceButtonPrefab, choicesButtonContainer);
                choiceButton.GetComponentInChildren<TextMeshProUGUI>().text = choiceNode.Text;
                choiceButton.onClick.AddListener(() =>
                {
                    playerConversant.SelectChoice(choiceNode);
                });
            }
        }
    }
}
