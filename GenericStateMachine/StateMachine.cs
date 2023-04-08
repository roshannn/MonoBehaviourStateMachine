using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class StateMachine<T, U> where T : MonoBehaviour where U : Enum {

    private IState<T> currentState;
    private T stateObject = default;
    private readonly StateMachineFactory<T, U> stateMachineFactory;

    public StateMachine(T stateObject) {
        this.stateObject = stateObject;
        stateMachineFactory = new StateMachineFactory<T, U>();
    }

    public void ChangeState(U stateName) {
        IState<T> newState = stateMachineFactory.GetState(stateName);
        if (newState == null) {
            Debug.LogError($"Failed to change state to {stateName}.");
            return;
        }
        currentState?.OnStateExit();
        currentState = newState;
        currentState?.OnStateEnter(stateObject);
    }

    public void Update() {
        currentState?.OnStateUpdate();
    }
    public void LateUpdate() {
        currentState?.OnStateLateUpdate();
    }
}