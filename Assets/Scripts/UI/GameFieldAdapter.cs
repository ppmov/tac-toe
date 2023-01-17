using UnityEngine;
using System.Linq;
using TicTacToe;

public class GameFieldAdapter : MonoBehaviour
{
    [SerializeField]
    private FieldStyle style;

    private CellButton[] buttons;
    private AI xAI;
    private AI oAI;

    public Controller Controller { get; private set; }

    private bool Interactable
    {
        set
        {
            foreach (var button in buttons)
                if (button.Sign != null)
                    button.interactable = false;
                else
                    button.interactable = value;
        }
    }

    private void Awake()
    {
        buttons = GetComponentsInChildren<CellButton>();
        foreach (var button in buttons)
            button.Setup(style.X, style.O);

        Controller = new(buttons.ToList<ICell>().ToArray());
    }

    private void Start()
    {
        Controller.TurnEvent += UpdatePlayerInteraction;
        Controller.FinishEvent += OnGameFinished;

        Controller.Play();
    }

    public void AddAI_X() => xAI = new(Controller, Sign.X);
    public void AddAI_O() => oAI = new(Controller, Sign.O);

    private void UpdatePlayerInteraction()
    {
        if (Controller.Lead == Sign.X && xAI != null)
            Interactable = false;
        else
        if (Controller.Lead == Sign.O && oAI != null)
            Interactable = false;
        else
            Interactable = true;
    }

    private void OnGameFinished(Sign? winner)
    {
        if (winner != null)
        {
            var cell = Controller.GetAllLines().SingleOrDefault(x => Controller.GetLineState(x) == LineState.Full);
            Crossout(GetCellTransform(cell[0]), GetCellTransform(cell[^1]));
        }

        xAI = null;
        oAI = null;

        static RectTransform GetCellTransform(ICell cell) => (cell as CellButton).transform as RectTransform;
    }

    private void Crossout(RectTransform from, RectTransform to)
    {
        var obj = Instantiate(style.line, transform);
        var rect = obj.GetComponent<RectTransform>();

        // put line between cells
        rect.anchoredPosition = (from.anchoredPosition + to.anchoredPosition) / 2f;

        // rotate line
        var angle = Vector2.Angle(-Vector2.right, to.anchoredPosition - from.anchoredPosition);
        rect.Rotate(Vector3.forward, angle);

        // line length = distance between cells + average cell size
        var averageButtonSize = (to.sizeDelta.x + to.sizeDelta.y) / 2f;
        rect.sizeDelta = new Vector2(Vector2.Distance(from.anchoredPosition, to.anchoredPosition) + averageButtonSize, rect.sizeDelta.y);
    }
}