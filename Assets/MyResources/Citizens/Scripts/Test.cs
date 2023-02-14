using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    [SerializeField] LayerMask vitals;
    void Update()
    {
         if (Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit,Mathf.Infinity,vitals))
                {
                   hit.collider.GetComponent<VitalsAnimator>().Damage(50);
                }
            }
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadSceneAsync(0);
        }
    }
}
