
using UnityEngine;
using UnityEngine.AI;
public enum SlimeAnimationState { Idle, Walk, Jump, Attack, Damage }
public class EnemyAi : MonoBehaviour, IIntractable
{

    public Face faces;
    public GameObject SmileBody;
    public SlimeAnimationState currentState;

    public Animator animator;
    public NavMeshAgent agent;
    public Transform[] waypoints;
    public int damType;


    private Material faceMaterial;
    private Vector3 originPos;

    public enum WalkType { Patroll, ToPlayer }
    private WalkType walkType;
    private GameObject player;
    void Start()
    {
        faceMaterial = SmileBody.GetComponent<Renderer>().materials[1];
        walkType = WalkType.Patroll;
    }
    public void WalkToNextDestination()
    {
        currentState = SlimeAnimationState.Walk;
        agent.SetDestination(waypoints[0].position);
        // agent.SetDestination()
        SetFace(faces.WalkFace);
    }
    public void CancelGoNextDestination() => CancelInvoke(nameof(WalkToNextDestination));

    void SetFace(Texture tex)
    {
        faceMaterial.SetTexture("_MainTex", tex);
    }
    void Update()
    {
        switch (currentState)
        {
            case SlimeAnimationState.Idle:
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) return;
                StopAgent();
                break;
            case SlimeAnimationState.Walk:
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk")) return;
                agent.isStopped = false;
                agent.updateRotation = true;
                if (walkType == WalkType.ToPlayer)
                {
                    agent.SetDestination(waypoints[0].position);
                    // SetFace(faces.WalkFace);
                    // // agent reaches the destination
                    if (agent.remainingDistance < agent.stoppingDistance - 10)
                    {
                        walkType = WalkType.Patroll;
                        //facing to camera
                        transform.rotation = Quaternion.identity;
                        currentState = SlimeAnimationState.Idle;
                    }
                }
                //Patroll
                else
                {
                    if (waypoints.Length == 0 || waypoints[0] == null) return;

                    agent.SetDestination(waypoints[0].position);

                    // agent reaches the destination
                    if (agent.remainingDistance < agent.stoppingDistance)
                    {
                        currentState = SlimeAnimationState.Idle;

                        //wait 2s before go to next destionation
                        Invoke(nameof(WalkToNextDestination),0);
                    }

                }
                // set Speed parameter synchronized with agent root motion moverment
                animator.SetFloat("Speed", agent.velocity.magnitude);


                break;

            // case SlimeAnimationState.Jump:

            //     if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump")) return;

            //     StopAgent();
            //     SetFace(faces.jumpFace);
            //     animator.SetTrigger("Jump");

            //     //Debug.Log("Jumping");
            //     break;

            // case SlimeAnimationState.Attack:

            //     if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) return;
            //     StopAgent();
            //     SetFace(faces.attackFace);
            //     animator.SetTrigger("Attack");

            //     // Debug.Log("Attacking");

            //     break;
            // case SlimeAnimationState.Damage:

            //     // Do nothing when animtion is playing
            //     if (animator.GetCurrentAnimatorStateInfo(0).IsName("Damage0")
            //          || animator.GetCurrentAnimatorStateInfo(0).IsName("Damage1")
            //          || animator.GetCurrentAnimatorStateInfo(0).IsName("Damage2")) return;

            //     StopAgent();
            //     animator.SetTrigger("Damage");
            //     animator.SetInteger("DamageType", damType);
            //     SetFace(faces.damageFace);

            //     //Debug.Log("Take Damage");
            //     break;

        }

    }


    private void StopAgent()
    {
        agent.isStopped = true;
        animator.SetFloat("Speed", 0);
        agent.updateRotation = false;
    }
    // Animation Event
    public void AlertObservers(string message)
    {

        if (message.Equals("AnimationDamageEnded"))
        {
            // When Animation ended check distance between current position and first position 
            //if it > 1 AI will back to first position 

            float distanceOrg = Vector3.Distance(transform.position, originPos);
            if (distanceOrg > 1f)
            {
                walkType = WalkType.ToPlayer;
                currentState = SlimeAnimationState.Walk;
            }
            else currentState = SlimeAnimationState.Idle;

            //Debug.Log("DamageAnimationEnded");
        }

        if (message.Equals("AnimationAttackEnded"))
        {
            currentState = SlimeAnimationState.Idle;
        }

        if (message.Equals("AnimationJumpEnded"))
        {
            currentState = SlimeAnimationState.Idle;
        }
    }

    void OnAnimatorMove()
    {
        // apply root motion to AI
        Vector3 position = animator.rootPosition;
        position.y = agent.nextPosition.y;
        transform.position = position;
        agent.nextPosition = transform.position;
    }

    public void Interact(Interactor interactor)
    {
        switch (currentState)
        {
            case SlimeAnimationState.Idle:
                waypoints = new Transform[1];
                waypoints[0] = interactor.gameObject.transform;
                currentState = SlimeAnimationState.Walk;
                walkType = WalkType.ToPlayer;
                break;
            case SlimeAnimationState.Walk:
                currentState = SlimeAnimationState.Idle;
                break;
        }
    }
}
