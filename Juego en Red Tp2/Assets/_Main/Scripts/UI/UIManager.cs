using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject chat;
    [SerializeField] private Image micUi;
    [SerializeField] private Image micMuteUi;

    private void OnEnable()
    {
        ControllerFA.onChatHide += ShowChat;
        ControllerFA.onVoiceMute += ShowMicMuteUI;
    }

    public void ShowChat(bool state)
    {
        chat.SetActive(state);
    }

    public void ShowMicMuteUI(bool state)
    {
        micMuteUi.enabled = !state;
    }

    public void ShowMicUI(bool state)
    {
        micUi.enabled = state;
    }
}
