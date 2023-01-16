using UnityEngine;

[CreateAssetMenu(fileName = "New Field Style", menuName = "Tic Tac Toe")]
public class FieldStyle : ScriptableObject
{
    public Sprite X;
    public Sprite O;
    public GameObject line;
}
