using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public Camera playerCam;

    [SerializeField]
    private float distance = 3f;

    [SerializeField]
    private LayerMask mask;

    [SerializeField]
    private TextMeshProUGUI promptText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText(string.Empty);

        Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            if (hitInfo.collider.GetComponent<Interactable>())
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                UpdateText("[E] " + interactable.promptMessage);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.BaseInteract();
                }
            }
        }
    }

    private void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage;
    }
}
