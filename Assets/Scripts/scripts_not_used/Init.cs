using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init : MonoBehaviour
{
    public GameObject mito;
    // Start is called before the first frame update
    void Start()
    {


        //var newMito = Instantiate(mito, new Vector3(0, 3, 0), Quaternion.identity);
        //var target = new List<Vector3>();

        //for (int i = 0; i < 3; i++)
        //    target.Add(new Vector3(i, i, i));

        //var mitoScript = newMito.GetComponent<MitochondriaScript>();
        //mitoScript.target = target;


        var network = new List< List<(int, Vector3)> >();
        var zero = new Vector3(0, 0.5f, 0);
        var one = new Vector3(0, 0.5f, 0.5f);
        var two = new Vector3(-0.5f, 0.5f, 0.5f);
        var three = new Vector3(0, 0.5f, 1.5f);
        var four = new Vector3(0, 0.5f, 2);
        var five = new Vector3(0.5f, 0.5f, 0.5f);
        var six = new Vector3(0, 0.5f, 2.5f);
        var seven = new Vector3(0, 0.5f, 3);

        network.Add( new List<(int, Vector3)>() { (1, one) } );
        network.Add( new List<(int, Vector3)>() { (2, two), (3, three) } );
        network.Add( new List<(int, Vector3)>() { (1, one), (3, three) } );
        network.Add( new List<(int, Vector3)>() { (4, four), (2, two), (1, one) } );
        network.Add(new List<(int, Vector3)>() { (3, three), (6, six), (5, five) });
        network.Add(new List<(int, Vector3)>() { (4, four), (6, six), (7, seven) });
        network.Add(new List<(int, Vector3)>() { (4, four), (5, five) });
        network.Add(new List<(int, Vector3)>() { (5, five) });


        for (int i = 0; i < network.Count; i++)
        {
            for (int j = 0; j < network[i].Count; j++)
            {
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.transform.position = network[i][j].Item2;
                sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            }
        }

        for (int i = 0; i < 7; i++)
        {
            var newMito = Instantiate(mito, zero, Quaternion.identity);
            var mitoScript = newMito.GetComponent<MitochondriaScript>();
            mitoScript.target = network;
        }
  


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
