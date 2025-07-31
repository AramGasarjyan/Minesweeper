using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameUIView : MonoBehaviour
{
    [SerializeField] private GameObject _endGamePanel;
    [SerializeField] private TextMeshProUGUI _endGameText;
    [SerializeField] private Button _restartButton;

    public event System.Action OnRestartButtonClicked;

    private void Start()
    {
        _restartButton.onClick.AddListener(HandleRestartButtonClick);
    }

    private void OnDestroy()
    {
        _restartButton.onClick.RemoveListener(HandleRestartButtonClick);
    }

    public void ShowEndGamePanel(string message)
    {
        _endGameText.text = message;
        _endGamePanel.gameObject.SetActive(true);

        EventSystem.current.SetSelectedGameObject(_restartButton.gameObject);
    }

    public void HideEndGamePanel()
    {
        _endGamePanel.gameObject.SetActive(false);
    }

    private void HandleRestartButtonClick()
    {
        OnRestartButtonClicked?.Invoke();
    }
}
