using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartScript : MonoBehaviour
{
    private GameObject[] mitos;
    private GameObject[] branches;

    public void Restart()
    {
        mitos = GameObject.FindGameObjectsWithTag("Mito");
        branches = GameObject.FindGameObjectsWithTag("Branch");

        for (int i = 0; i < mitos.Length; i++)
        {
            Destroy(mitos[i]);
        }

        for (int i = 0; i < branches.Length; i++)
        {
            Destroy(branches[i]);
        }
    }

}
