using UnityEngine;

[CreateAssetMenu(fileName = "StyleData", menuName = "PlayerStyle", order = 1)]
public class PlayerStyleData : ScriptableObject
{
    public int playerID;
    public Sprite alive;
    public Sprite spirit;
    public Sprite instrument;
}
