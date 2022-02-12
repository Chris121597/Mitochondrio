using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MitochondriaScript : MonoBehaviour
{
    public List< List<(int, Vector3)> > target = null;
    public float speed;
    public int current;
    public int j;
    public bool isFollower = false;
    public bool isLeader = false;
    public MitochondriaScript leader = null;
    public int followers;
    public bool isMainLeader = false;
    public bool hasMainLeader = false;
    public int uid = 0;
    public float fusionProbability = 1f;
    public float fissionProbability = 0.001f;
    public float closeEnoughDistance = 0.15f;

    private float fps;

    // Start is called before the first frame update
    void Start()
    {
        uid = gameObject.GetInstanceID();
    }

    // Update is called once per frame
    void Update()
    {
        fps = 1f / Time.deltaTime;

        // fission
        if (Random.Range(0f, 1f) <= (fissionProbability / fps) )
        {
            isFollower = false;
            hasMainLeader = false;

            if (leader)
            {
                leader.followers -= 1;
                leader = null;
            }

            if (followers > 0)
            {
                isMainLeader = true;
            }

        }

        if (isFollower)
        {
            var leaderPosition = leader.transform.position;
            float distance = Vector3.Distance(transform.position, leaderPosition);
            if (distance > closeEnoughDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, leaderPosition, speed * Time.deltaTime);
            }
            return;
        }

        if (target == null) 
            return;

        float dist = Vector3.Distance(transform.position, target[current][j].Item2);

        if (dist > closeEnoughDistance)
        {   
            if ( Random.Range(0f, 1f) > 0.80 )
                transform.position = Vector3.MoveTowards(transform.position, target[current][j].Item2, speed * Time.deltaTime);
        }
        else 
        {
            current = target[current][j].Item1;
            j = Random.Range(0, target[current].Count);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.name.Contains("Mitochondrion") || Random.Range(0f, 1f) > fusionProbability)
        {
            return;
        }
        var otherMito = other.GetComponent<MitochondriaScript>(); // get the class of the other mito 


        if (isMainLeader && (otherMito.isMainLeader || otherMito.hasMainLeader))
        {

            bool snakeBitingTail = isSnakeBitingItsTail(uid, otherMito);
            if (snakeBitingTail)
            {
                return;
            }
            leader = otherMito;
            isMainLeader = false;
            isFollower = true;
            hasMainLeader = true;
            otherMito.followers += 1;
        }

        else if (!isFollower && !isMainLeader)
        {
            isFollower = true;
            leader = otherMito;
            hasMainLeader = true;
            otherMito.followers += 1;

            if (!otherMito.hasMainLeader)
            {
                otherMito.isMainLeader = true;
            }

        }


    }

    private bool isSnakeBitingItsTail(int headUid, MitochondriaScript tail)
    {
        if (headUid == tail.uid)
        {
            return true;
        }

        if (tail.leader == null)
        {
            return false;
        }

        return isSnakeBitingItsTail(headUid, tail.leader);
    }

}
