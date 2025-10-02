using UnityEngine;


namespace MagicCube
{

    public class MagicTrickView : MonoBehaviour
    {
        public CubeColour selectedColour;

        public GameEvent OnMagicTrickPerformed;

        public void OnMagicButtonPressed()
        {
            OnMagicTrickPerformed.Raise(this, selectedColour);
            Debug.Log($"Poof, its {selectedColour} now!");
        }

        public void SelectColour(int cubeColourIndex)
        {
            selectedColour = (CubeColour)cubeColourIndex;
        }
    }

    public enum CubeColour
    {
        red = 0, blue = 1, yellow = 2
    }
}