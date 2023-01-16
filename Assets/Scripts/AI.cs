using System.Linq;
using UnityEngine;
using TicTacToe;

public class AI
{
    private const float pressDelay = 0.5f;

    private readonly Sign sign;
    private readonly Controller TTT;

    private bool isFirstTurn = true;

    public AI(Controller controller, Sign sign)
    {
        this.sign = sign;

        TTT = controller;
        TTT.TurnEvent += MakeTurn;
    }

    private void MakeTurn()
    {
        if (TTT.Lead == sign)
            ((CellButton)SelectCell()).PressWithDelay(pressDelay);
    }

    private ICell SelectCell()
    {
        if (isFirstTurn)
        {
            isFirstTurn = false;

            // на первом ходу пытаемся сходить в центр
            var cell = SelectMiddleCell();

            if (cell != null)
                return cell;

            // если не получилось - ходим в рандомный угол
            var diagonal = Random.Range(0, 2) == 0 ? TTT.grid.MainDiagonal : TTT.grid.AntiDiagonal;
            return diagonal[Random.Range(0, 2) == 0 ? 0 : TTT.grid.Size - 1];
        }

        var linesOfInterest = TTT.GetAllLines().Where(l => Controller.GetLineState(l) != LineState.Ambigious);

        var xLines = linesOfInterest.Where(l => l.FirstOrDefault(x => x.Sign == Sign.X) != null);
        var oLines = linesOfInterest.Where(l => l.FirstOrDefault(o => o.Sign == Sign.O) != null);

        var myLines = sign == Sign.X ? xLines : oLines;
        var hisLines = sign == Sign.X ? oLines : xLines;

        // выбираются линии, на которых почти достигнута победа
        var myFirstAlmostFullLine = myLines.FirstOrDefault(x => Controller.GetLineState(x) == LineState.AlmostFull);

        if (myFirstAlmostFullLine != null)
            return myFirstAlmostFullLine.Where(x => x.Sign == null).SingleOrDefault();

        // выбираются линии, на которых почти победил противоположный игрок
        var hisFirstAlmostFullLine = hisLines.FirstOrDefault(x => Controller.GetLineState(x) == LineState.AlmostFull);

        if (hisFirstAlmostFullLine != null)
            return hisFirstAlmostFullLine .Where(x => x.Sign == null).SingleOrDefault();

        // выбирается линия, на которой больше всего занятых позиций
        var myFirstBarelyFullLine = myLines.Where(x => Controller.GetLineState(x) == LineState.BarelyFull).OrderBy(l => l.Count(x => x.Sign == null)).FirstOrDefault();

        if (myFirstBarelyFullLine != null)
            return myFirstBarelyFullLine.Where(x => x.Sign == null).FirstOrDefault();

        return SelectRandomCell();
    }

    private ICell SelectRandomCell()
    {
        var emptyCells = TTT.grid.Where(x => x.Sign == null).ToArray();

        if (emptyCells.Length > 0)
        {
            var randomEmptyCell = emptyCells[Random.Range(0, emptyCells.Length)];

            if (randomEmptyCell != null)
                return randomEmptyCell;
        }

        return null;
    }

    private ICell SelectMiddleCell()
    {
        int middle = (TTT.grid.Size - 1) / 2;

        if (TTT.grid.rows[middle][middle].Sign == null)
            return TTT.grid.rows[middle][middle];

        return null;
    }
}