using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
public class Unit : MonoBehaviour {
    [SerializeField] protected NavMeshAgent agent;

    public void OnMove (Vector3 destination) {
        agent.destination = destination;
    }

    public void OnSelect () {
        //Highlight
    }

    public void OnDeselect () {

        //Remove Highlight
    }
}