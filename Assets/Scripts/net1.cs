using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class net1 : MonoBehaviour
{
    public GameObject mito;
    private int numMainBranches = 10;
    private int avgMainBranchLength = 10;
    private int avgSideBranches = 2;
    private int avgSideBranchLength = 7;
    private int avgMitosPerMainBranch = 10;
    private bool branchesEnabled = true;
    private float nucleusRadius = 1.5f;
    private float nucleusCenter = 0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void StartSimulation()
    {

        for (int j = 0; j < numMainBranches; j++)
        {
            // convert to radians
            var angle = (180 * j) / Mathf.PI;
            var rootX = nucleusCenter + nucleusRadius * Mathf.Cos(angle);
            var rootZ = nucleusCenter + nucleusRadius * Mathf.Sin(angle);

            var network = new List<List<(int, Vector3)>>();
            var nodes = new List<Vector3>();

            // Create main branch
            createNodes(nodes, (rootX, 0f, rootZ), avgMainBranchLength);
            LineRenderer l1 = createLineRenderer(Color.blue);
            l1.positionCount = nodes.Count;
            createMainBranch(network, nodes, l1);

            // number of branches on main branches
            for (int i = 0; i < avgSideBranches; i++)
            {
                branch(network, nodes);
            }

            if (!branchesEnabled)
            {
                toggleCytoskeleton(branchesEnabled);
            }

            //showNodes(nodes);

            // instantiate the mitos
            for (int i = 0; i < avgMitosPerMainBranch; i++)
            {
                var current = Random.Range(0, network.Count - 1);
                var jj = Random.Range(0, network[current].Count);

                var newMito = Instantiate(mito, network[current][jj].Item2, Quaternion.identity);
                newMito.tag = "Mito";
                var mitoScript = newMito.GetComponent<MitochondriaScript>();

                mitoScript.target = network;
                mitoScript.j = jj;
                mitoScript.current = current;
            }
        }

    }

    // variables set by user
    public void setNumMainBranches(string value)
    {
        try
        {
            numMainBranches = System.Int32.Parse(value);
        }
        catch(System.FormatException){}
    }

    public void setAvgMainBranch(string value)
    {
        try
        {
            avgMainBranchLength = System.Int32.Parse(value);
        }
        catch (System.FormatException){}
    }

    public void SetAvgSideBranches(string value)
    {
        try
        {
            avgSideBranches = System.Int32.Parse(value);
        }
        catch (System.FormatException) { }
    }

    public void setAvgSideBranchLength(string value)
    {
        try
        {
            avgSideBranchLength = System.Int32.Parse(value);
        }
        catch (System.FormatException) { }
    }

    public void setAvgMitosPerMainBranch(string value)
    {
        try
        {
            avgMitosPerMainBranch = System.Int32.Parse(value);
        }
        catch (System.FormatException) { }
    }

    // branch from a randomly chosen branch
    private void branch(List<List<(int, Vector3)>> network, List<Vector3> nodes)
    {
        int lastNodeNum = nodes.Count;
        int randNode = Random.Range(0, lastNodeNum - 1);
        Vector3 lastPos = nodes[randNode];
        float randPos = Random.Range(-0.2f, 0.2f);

        createNodes(nodes, (lastPos.x + randPos, lastPos.y + randPos, lastPos.z + randPos), avgSideBranchLength);
        int newNodesNum = nodes.Count - lastNodeNum;

        LineRenderer l2 = createLineRenderer(Color.gray);
        l2.positionCount = newNodesNum + 1;
        createSecondaryBranch(network, nodes, l2, lastNodeNum, randNode);
    }

    // Create a new line renderer Object
    private LineRenderer createLineRenderer(UnityEngine.Color color)
    {
        GameObject obj = new GameObject("cytoskeleton branch");
        obj.tag = "Branch";
        LineRenderer lineRenderer = obj.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.02f;

        lineRenderer.sharedMaterial.SetColor("_Color", color);

        return lineRenderer;
    }

    private void createNodes(List<Vector3> nodes, (float, float, float) initPos, int maxNodes)
    {
        float x = initPos.Item1;
        float y = initPos.Item2;
        float z = initPos.Item3;

        var xed = 0.2f;
        var yed = 0.1f;
        var zed = 0.2f;
        for (int i = 0; i < maxNodes; i++)
        {
            if (i == 0)
                nodes.Add(new Vector3(x, y, z));
            else
            {
                var a = nodes[nodes.Count - 1];
                x = a.x + xed;
                y = a.y + yed;
                z = a.z + zed;

                float xedCopy = xed;
                float yedCopy = yed;
                float zedCopy = zed;

                do
                {
                    var xturn = Random.Range(0f, 0.1f);
                    if (xturn < 0.06f)
                    {
                        xed += -0.1f;
                    }
                    else
                    {
                        xed += 0.1f;
                    }

                    var zturn = Random.Range(0f, 0.1f);
                    if (zturn < 0.06f)
                    {
                        zed += -0.1f;
                    }
                    else
                    {
                        zed += 0.1f;
                    }

                    var yturn = Random.Range(0f, 0.1f);
                    if (yturn < 0.05f)
                    {
                        yed += -0.0001f;
                    }
                    else
                    {
                        yed += 0.0001f;
                    }

                    if (Vector3.Distance(new Vector3(x + xed, y + yed, z + zed), new Vector3(nucleusCenter, nucleusCenter, nucleusCenter)) < nucleusRadius)
                    {
                        xed = -xed;
                        yed = -yed;
                        zed = -zed;
                    }

                } while (
                    Vector3.Distance(
                          new Vector3(x + xed, y + yed, z + zed),
                          new Vector3(nucleusCenter, nucleusCenter, nucleusCenter)
                          ) < nucleusRadius
                    );
                nodes.Add(new Vector3(x, y, z));
            }
            //x += Random.Range(-0.1f, 0.1f);
            //y += Random.Range(0f, 0f);
            //z += Random.Range(-0.1f, 0.1f);
        }
    }

    private void createMainBranch(List<List<(int, Vector3)>> network, List<Vector3> nodes, LineRenderer lineRenderer)
    {
        int counter = 0;
        for (int i = 0; i < nodes.Count; i++)
        {
            lineRenderer.SetPosition(counter, nodes[i]);
            counter++;

            if (i == nodes.Count - 1) network.Add(new List<(int, Vector3)> { (i - 1, nodes[i - 1]) });
            else if (i > 0) network.Add(new List<(int, Vector3)> { (i - 1, nodes[i - 1]), (i + 1, nodes[i + 1]) });
            else network.Add(new List<(int, Vector3)> { (i + 1, nodes[i + 1]) });
        }
    }

    private void createSecondaryBranch(List<List<(int, Vector3)>> network, List<Vector3> nodes, LineRenderer lineRenderer,
        int startNodeind, int branchPoint)
    {

        lineRenderer.SetPosition(0, nodes[branchPoint]);
        int counter = 1;
        for (int i = startNodeind; i < nodes.Count; i++)
        {
            lineRenderer.SetPosition(counter, nodes[i]);
            counter++;

            if (i == nodes.Count - 1) network.Add(new List<(int, Vector3)> { (i - 1, nodes[i - 1]) });
            else if (i == startNodeind) // create a connection at the branching point
            {
                network[branchPoint].Add((i, nodes[i]));
                network.Add(new List<(int, Vector3)> { (branchPoint, nodes[branchPoint]), (i + 1, nodes[i + 1]) });
            }
            else if (i > 0) network.Add(new List<(int, Vector3)> { (i - 1, nodes[i - 1]), (i + 1, nodes[i + 1]) });
        }
    }

    private void showNodes(List<Vector3> nodes)
    {
        // shows the nodes of the graph
        for (int i = 0; i < nodes.Count; i++)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = nodes[i];
            sphere.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        }
    }

    public void toggleCytoskeleton(bool enabled)
    {
        branchesEnabled = enabled;
        foreach (GameObject cytoskeletonBranch in GameObject.FindGameObjectsWithTag("Branch"))
        {
            var line = cytoskeletonBranch.GetComponent<LineRenderer>();
            line.enabled = enabled;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
