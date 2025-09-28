using UnityEngine;

public class MagicTrickView : MonoBehaviour
{
        public GameEvent OnMagicTrickPerformed;

        public void OnMagicButtonPressed()
        {
            OnMagicTrickPerformed.Raise(this, null);
            Debug.Log("Poof!");
        }
}
