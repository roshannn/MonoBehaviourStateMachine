using ModestTree;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class StateMachineFactory<T, U> where T : MonoBehaviour where U : Enum {
    private readonly Dictionary<U, IState<T>> states = new Dictionary<U, IState<T>>();

    public IState<T> GetState(U state) {
        if (!states.TryGetValue(state, out IState<T> newState)) {
            Type type = Type.GetType($"{typeof(T).Namespace}.{state}");
            if (type == null) {
                Debug.LogError($"Failed to get state {state}. Class not found.");
                return null;
            }
            newState = (IState<T>)Activator.CreateInstance(type);
            states.Add(state, newState);
        }
        return states[state];
    }
}
