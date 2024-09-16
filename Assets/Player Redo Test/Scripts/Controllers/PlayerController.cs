using UnityEngine;

[CreateAssetMenu(fileName = "PlayerController", menuName = "InputController/PlayerController")]

public class PlayerController : InputController
{
    public override bool RetrieveJumpInput()
    {
        return Input.GetButtonDown("Jump");
    }

    public override float RetrieveMoveInput()
    {
        return Input.GetAxisRaw("Horizontal");
    }

    public override bool RetrieveHealInput()
    {
        return Input.GetKeyDown(KeyCode.H);
    }

    public override bool RetrieveRespawnInput()
    {
        return Input.GetKeyDown(KeyCode.G);
    }
}
