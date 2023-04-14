using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//no failsafe for state dependencies 
//no state validations - hint use promises 
//think about it and implement it in the future roshan - Abhijith 
public sealed class StateMachine<T, U> where T : MonoBehaviour where U : Enum 
{

    private IState<T> currentState;
    private T stateObject = default;

    private U stateName = default;
    private readonly StateMachineFactory<T, U> stateMachineFactory;

    public StateMachine(T stateObject) 
    {
        this.stateObject = stateObject;

        if (!typeof(U).IsEnum)
        {
            throw new ArgumentException("Type U must be an enum."); // c#9 cant check for u being an enum during complie time 
            //how ever will throw an error when implemented wrongly , i think c# 11 fixes this untill then use this noob safety check 
        }

        stateMachineFactory = new StateMachineFactory<T, U>();
    }

    public void ChangeState(U _stateName) 
    {
        stateName = _stateName;

        IState<T> newState = stateMachineFactory.GetState(stateName);
        if (newState == null) 
        {
            Debug.LogError($"Failed to change state to {stateName}.");
            return;
        }
        currentState?.OnStateExit();
        currentState = newState;
        currentState?.OnStateEnter(stateObject);
    }
    /// <summary>
    /// to be used by classes which changes state async or by cor , or states depended on https events
    /// </summary>
    /// <param name="stateName"></param>
    public void AsyncChangeStateHandler(U _stateName) 
    {
        stateName = _stateName;
        
        IState<T> newState = stateMachineFactory.GetState(stateName);
        if (newState is  null) //used is to prevent overloading 
        {
            Debug.LogError($"Failed to change state to {stateName}.");
            return;
        }

        // Use a Promise to handle the state transition asynchronously you can use it otherwise too better callbacks here 
        Promise<bool> stateTransitionPromise = new Promise<bool>();

        // Called when  OnStateExit() on the current state and wait for the Promise to maybe  resolve
        currentState?.OnStateExit(stateTransitionPromise.Resolve);

        // Attach a callback to the Promise's Then method to handle the state transition
        stateTransitionPromise.Then((result) => 
        {
            if (result) 
            {
                currentState = newState;
                currentState?.OnStateEnter(stateObject);
            } else 
            {
                Debug.LogError($"Failed to exit current state {stateName}.");
            }
        });

        // Attach a callback to the Promise's Catch method to handle any errors
        stateTransitionPromise.Catch((ex) => {
            Debug.LogError($"Failed to exit current state {stateName}: {ex.Message}");
        });
    }

    public void Update()
    {
        currentState?.OnStateUpdate();
    }

    public void LateUpdate() 
    {
        currentState?.OnStateLateUpdate();
    }
}
