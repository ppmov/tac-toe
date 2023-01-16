using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private TMPro.TMP_Text infoView;
    [SerializeField]
    private GameObject playButton;
    [SerializeField]
    private GameObject restartButton;

    [Space]
    [SerializeField]
    private Slider sizeSlider;
    [SerializeField]
    private Slider xSlider;
    [SerializeField]
    private Slider oSlider;

    [Space]
    [SerializeField]
    private GameFieldAdapter[] prefabs3x3;
    [SerializeField]
    private GameFieldAdapter[] prefabs5x5;

    private GameFieldAdapter gameField;

    private bool IsInteractable
    {
        set
        {
            sizeSlider.interactable = xSlider.interactable = oSlider.interactable = value;
            sizeSlider.fillRect.gameObject.SetActive(value);
            xSlider.fillRect.gameObject.SetActive(value);
            oSlider.fillRect.gameObject.SetActive(value);
        }
    }

    private bool HasPlayButton
    {
        set
        {
            playButton.SetActive(value);
            restartButton.SetActive(!value);
        }
    }

    private void Start() => IsInteractable = HasPlayButton = true;

    private void Update()
    {
        if (gameField != null)
            if (Input.GetKeyDown(KeyCode.Escape))
                Close();
    }

    public void Play()
    {
        IsInteractable = HasPlayButton = false;

        var prefab = sizeSlider.value == 0
            ? prefabs3x3[Random.Range(0, prefabs3x3.Length)]
            : prefabs5x5[Random.Range(0, prefabs5x5.Length)];

        gameField = Instantiate(prefab, transform);
        gameField.Controller.FinishEvent += (x) => IsInteractable = true;

        if (xSlider.value == 1)
            gameField.AddAI_X();

        if (oSlider.value == 1)
            gameField.AddAI_O();

        if (infoView != null)
        {
            gameField.Controller.TurnEvent += SetTurnInfo;
            gameField.Controller.FinishEvent += SetResultInfo;
        }
    }

    public void Close()
    {
        IsInteractable = HasPlayButton = true;

        if (gameField != null)
            Destroy(gameField.gameObject);

        if (infoView != null)
            infoView.text = string.Empty;
    }

    private void SetTurnInfo() => infoView.text = $"{gameField.Controller.Lead} turn";
    private void SetResultInfo(TicTacToe.Sign? winner) => infoView.text = winner == null ? "draw" : $"{winner} wins";
}
