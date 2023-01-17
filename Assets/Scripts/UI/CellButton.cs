using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TicTacToe;

public class CellButton : Button, ICell
{
    private Sprite xs;
    private Sprite os;

    private Image Image => _image == null ? _image = GetComponentInChildren<Image>() : _image;
    private Image _image;

    public event Action PressEvent;

    public Sign? Sign { get; private set; } = null;

    public void Setup(Sprite xs, Sprite os)
    {
        this.xs = xs;
        this.os = os;

        Clear();
    }

    public void Draw(Sign sign)
    {
        Sign = sign;

        switch (Sign)
        {
            case TicTacToe.Sign.X:
                Image.sprite = xs;
                break;
            case TicTacToe.Sign.O:
                Image.sprite = os;
                break;
        }

        Image.color = Color.white;
    }

    public void Clear()
    {
        Sign = null;
        Image.color = Color.clear;

        onClick.RemoveAllListeners();
        onClick.AddListener(() => PressEvent.Invoke());
    }

    public void PressWithDelay(float delay) => StartCoroutine(WaitAndPress(delay));

    private IEnumerator WaitAndPress(float delay)
    {
        yield return new WaitForSeconds(delay);
        onClick.Invoke();
    }
}
