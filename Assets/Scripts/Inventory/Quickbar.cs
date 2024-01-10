using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quickbar : MonoBehaviour
{
    public GameObject quickbar;

    private void Update()
    {
        StartCoroutine(ChangeLayers());
    }

    void ChangeLayersToDefault(GameObject parent)
    {
        foreach(Transform child in parent.transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer("Default");

            if(child.childCount > 0)
            {
                ChangeLayersToDefault(child.gameObject);
            }
        }
    }

    IEnumerator ChangeLayers()
    {
        yield return new WaitForSeconds(0.2f);
        ChangeLayersToDefault(quickbar);
    }
}
