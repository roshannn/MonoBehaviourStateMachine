using UnityEngine;

public interface IState<T> where T : MonoBehaviour {
    public void OnStateEnter(T StateObject);
    public void OnStateExit();
    public void OnStateUpdate();
    public void OnStateLateUpdate();
}