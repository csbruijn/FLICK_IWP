using UnityEngine;
using UnityEngine.UI;

public class NotesBonusView : MonoBehaviour
{
    [SerializeField] private Image barToFill; 
    public void OnUpdateNotesBar(Component sender, System.Object data)
    {
        barToFill.fillAmount = (float)data;
    }
}
