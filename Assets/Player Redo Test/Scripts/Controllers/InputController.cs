using UnityEngine;

public abstract class InputController : ScriptableObject
{
    public abstract float RetrieveMoveInput();

    public abstract bool RetrieveJumpInput();
    public abstract bool RetrieveHealInput();
    public abstract bool RetrieveRespawnInput();
}
