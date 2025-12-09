using UnityEngine;

public class AttackBehaviour : MonoBehaviour
{
    public GameEvent OnSirenWaveAttack; 

    public void OnMagicButtonPressed()
    {
        OnSirenWaveAttack.Raise(this,null);
        Debug.Log("SIREN SWEEP");
    }
    
}
