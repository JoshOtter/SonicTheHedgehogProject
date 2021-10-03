using UnityEngine;

//This sets up the abstract class on which all other states are based.
public abstract class SonicBaseState
{
    public abstract void EnterState(SonicController_FSM player);

    public abstract void Update(SonicController_FSM player);

    public abstract void OnCollisionEnter2D(SonicController_FSM player);
}
