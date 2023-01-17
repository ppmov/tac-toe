using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToe
{
    public enum Sign { X, O }
    public enum LineState { Free, Ambigious, BarelyFull, AlmostFull, Full }

    public interface ICell
    {
        public Sign? Sign { get; }
        public event Action PressEvent;

        public void Draw(Sign sign);
        public void Clear();
    }

    public class Controller
    {
        public Sign Lead { get; private set; }
        public bool IsPlaying { get; private set; }

        public event Action TurnEvent;
        public event Action<Sign?> FinishEvent;

        public readonly DynamicMatrix<ICell> grid;

        public Controller(ICell[] cells)
        {
            grid = new DynamicMatrix<ICell>(cells);

            foreach (var cell in grid)
                cell.PressEvent += () => OnPressed(cell);
        }

        public void Play()
        {
            foreach (var cell in grid)
                cell.Clear();

            Lead = Sign.X;
            IsPlaying = true;
            TurnEvent?.Invoke();
        }

        public ICell[][] GetAllLines()
        {
            var result = new List<ICell[]>(grid.Size * 2 + 2);

            foreach (var row in grid.rows)
                result.Add(row);

            foreach (var col in grid.cols)
                result.Add(col);

            result.Add(grid.MainDiagonal);
            result.Add(grid.AntiDiagonal);
            return result.ToArray();
        }

        public static LineState GetLineState(ICell[] line)
        {
            var _Count = line.Where(x => x.Sign == null).Count();
            var xCount = line.Where(x => x.Sign == Sign.X).Count();
            var oCount = line.Where(x => x.Sign == Sign.O).Count();

            if (xCount == line.Length || oCount == line.Length)
                return LineState.Full;

            if (xCount > 0 && oCount > 0)
                return LineState.Ambigious;

            if (_Count == 1)
                return LineState.AlmostFull;

            if (_Count > 1)
                return LineState.BarelyFull;

            return LineState.Free;
        }

        private void Finish(Sign? winner)
        {
            IsPlaying = false;
            FinishEvent?.Invoke(winner);
        }

        private void OnPressed(ICell cell)
        {
            if (!IsPlaying)
                return;

            cell.Draw(Lead);

            if (IsGameFinished(out var winner))
                Finish(winner);
            else
            {
                Lead = Lead == Sign.X ? Sign.O : Sign.X;
                TurnEvent?.Invoke();
            }
        }

        private bool IsGameFinished(out Sign? sign)
        {
            sign = Lead; // lead may win

            bool canBeContinued = false;

            foreach (var line in GetAllLines())
                switch (GetLineState(line))
                {
                    case LineState.Full:
                        return true;

                    case LineState.Ambigious:
                        continue;

                    default:
                        canBeContinued = true;
                        continue;
                }

            sign = null;

            if (!canBeContinued)
                return true; // draw
            else
                return false;
        }
    }
}