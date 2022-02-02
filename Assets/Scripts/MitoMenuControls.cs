using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MitoMenuControls : MonoBehaviour
{
    private GameObject[] mitos;
    private Vector3 mitoLocalScale;

    public void updateFusionProbability(string newProbability)
    {
        updateProbability(newProbability, "fusion");
    }

    public void updateFissionProbability(string newProbability)
    {
        updateProbability(newProbability, "fission");
    }

    public void updateProbability(string newProbability, string type)
    {
        mitos = GameObject.FindGameObjectsWithTag("Mito");

        for (int i = 0; i < mitos.Length; i++)
        {
            var mito = mitos[i];

            var mitoScript = mito.GetComponent<MitochondriaScript>();

            try
            {
                float floatNewProbability = (float)System.Convert.ToDouble(newProbability);
                floatNewProbability /= 100f;

                switch(type)
                {
                    case "fusion": 
                        mitoScript.fusionProbability = floatNewProbability;
                        break;
                    case "fission":
                        mitoScript.fissionProbability = floatNewProbability;
                        break;
                }
                
            }
            catch (System.FormatException) { }
        }

    }

    public void updateMitoScale(float newScale)
    {
        mitos = GameObject.FindGameObjectsWithTag("Mito");

        if (mitoLocalScale.Equals(Vector3.zero) && mitos.Length > 0)
        {
            mitoLocalScale = mitos[0].transform.localScale;
        }

        for (int i = 0; i < mitos.Length; i++)
        {
            var mito = mitos[i];
            mito.transform.localScale = mitoLocalScale * newScale;
        }
    }
}
