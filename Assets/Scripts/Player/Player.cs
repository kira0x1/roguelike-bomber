using UnityEngine;

[RequireComponent (typeof (CharacterController))]
public class Player : MonoBehaviour {
    private Vector3 movDir = Vector3.zero;
    private CharacterController controller;

    [SerializeField] private float curSpeed = 6f;
    [SerializeField] private float gravity = 15f;

    private void Awake () {
        controller = GetComponent<CharacterController> ();
    }

    private void Update () {
        Move ();
    }

    private void Move () {
        float h = Input.GetAxis ("Horizontal");
        float v = Input.GetAxis ("Vertical");
        movDir = new Vector3 (h, 0, v);
        movDir = transform.TransformDirection (movDir);
        movDir = Vector3.ClampMagnitude (movDir, 1);
        movDir *= curSpeed;

        movDir.y -= gravity;
        controller.Move (movDir * Time.deltaTime);
    }
}