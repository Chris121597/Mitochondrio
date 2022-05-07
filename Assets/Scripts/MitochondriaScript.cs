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
    private float originalCloseEnoughDistance = 0.15f;
    public float closeEnoughDistance;
    /** 
     * this timout prevents from the repel logic from being fired twice when
     * two mitos bumpt into each other
     */
    public bool repelTimeOut = false;

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

        // if it's follower, adjust current to leader's position
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
        {
            return;
        }

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
        // get the class of the other mito 
        var otherMito = other.GetComponent<MitochondriaScript>();

        // if we didnt bumpt into a mito or if fusion did not happen
        if (!other.name.Contains("Mitochondrion") || Random.Range(0f, 1f) > fusionProbability)
        {
            // if fussion did not happen, move the mitos in opposite directions of each other since they shouldn't be 
            // allowed to use the same space
            if (other.name.Contains("Mitochondrion") && !repelTimeOut)
            {
                if (current < otherMito.current)
                {
                    current = Mathf.Max(current - 1, 0);
                    otherMito.current = Mathf.Min(otherMito.current + 1, otherMito.target.Count - 1);
                }
                else
                {
                    current = Mathf.Min(current + 1, target.Count - 1);
                    otherMito.current = Mathf.Max(otherMito.current - 1, 0);
                }
                j = 0;
                otherMito.j = 0;
                otherMito.repelTimeOut = true;
            }
            else
            {
                repelTimeOut = false;
            }
            return;
        }

        if (isMainLeader && (otherMito.isMainLeader || otherMito.hasMainLeader))
        {
            MitochondriaScript snakeBitingTail = isSnakeBitingItsTail(uid, otherMito);

            isFollower = true;
            hasMainLeader = true;
            isMainLeader = false;

            if (snakeBitingTail != null)
            {
                // make the follower mito the main leader
                snakeBitingTail.isMainLeader = true;
                snakeBitingTail.hasMainLeader = true;
                snakeBitingTail.leader = null;
                snakeBitingTail.isFollower = false;
                snakeBitingTail.followers += 1;
                leader = snakeBitingTail;
                followers -= 1;

                // make target of the mito that was following the current mito, the current mito's target
                snakeBitingTail.current = current;
                snakeBitingTail.j = j;
                snakeBitingTail.target = target;
                return;
            }

            // make the mito we bumped into the leader
            leader = otherMito;
            otherMito.followers += 1;
        }
        else if (!isFollower && !isMainLeader)
        {
            // make the mito we bumped into the leader
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

    /**
     * Checks to see if a leader is colliding with one of its followers. If so, it returns the closest follower and null
     * otherwise.
    */
    private MitochondriaScript isSnakeBitingItsTail(int headUid, MitochondriaScript tail)
    {
        if (tail.leader && headUid == tail.leader.uid)
        {
            return tail;
        }

        if (tail.leader == null)
        {
            return null;
        }

        return isSnakeBitingItsTail(headUid, tail.leader);
    }

    public float getOriginalCloseEnoughDistance()
    {
        return originalCloseEnoughDistance;
    }
}
