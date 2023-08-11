
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using System.Collections;

public enum SlimeAnimationState { Idle, Walk, Jump, Attack, Damage }
public class PetAI : MonoBehaviour, IIntractable
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

    public enum WalkType { Patroll, ToDestination}
    private WalkType walkType;

    public enum WorkType { None, Follow, Collect}
    private WorkType workType;
    private bool isCollecting = false;
    public bool isDisplayUI = false;

    private EventInstance slimeWalkSound;
    FMOD.ATTRIBUTES_3D attributes;
    public int timePerCollect;
    void Start()
    {
        faceMaterial = SmileBody.GetComponent<Renderer>().materials[1];
        Debug.Log(faceMaterial.name);
        walkType = WalkType.Patroll;
        workType = WorkType.None;
        FMOD.ATTRIBUTES_3D attributes = new FMOD.ATTRIBUTES_3D();
        attributes.position = RuntimeUtils.ToFMODVector(transform.position); // Set the position in 3D space
        attributes.velocity = RuntimeUtils.ToFMODVector(Vector3.zero); // Set the velocity (optional)
        attributes.forward = RuntimeUtils.ToFMODVector(transform.forward); // Set the forward vector (optional)
        attributes.up = RuntimeUtils.ToFMODVector(transform.up);
        slimeWalkSound = AudioManager.instance.CreateInstance(FMODEvents.instance.slimeWalkSound);
        slimeWalkSound.setVolume(0.5f);
        slimeWalkSound.set3DAttributes(attributes);
        
    }
    void Collecting()
    {
        string objectName = waypoints[0].gameObject.GetComponent<CropHarvest>().itemData.ItemPreFab.name;
        Vector3 _dropOffset = new Vector3(Random.Range(-0.1f, 0.1f), 0, Random.Range(-0.1f, 0.1f));
        GameObject newObject = PhotonNetwork.Instantiate(objectName,new Vector3(waypoints[0].position.x-1,waypoints[0].position.y+1,waypoints[0].position.z+1) + _dropOffset,Quaternion.identity);
        newObject.GetComponent<Rigidbody>().AddForce(waypoints[0].forward * 5,ForceMode.Impulse);
    }
    // public void CancelGoNextDestination() => CancelInvoke(nameof(Collect));
    void StopCollecting(){
        isCollecting = false;
        workType = WorkType.None;
        CancelInvoke ("Collecting");
    }
    void MoveToDestination(Vector3 destination){
        agent.isStopped = false;
        agent.updateRotation = true;
        agent.SetDestination(destination);
    }
    void SetFace(Texture tex)
    {
        faceMaterial.SetTexture("_MainTex", tex);
    }
    public float GetRemainingDistance()
    {
        float distance = 0;
        Vector3[] corners = agent.path.corners;

        if (corners.Length > 2)
        {
                for (int i = 1; i < corners.Length; i++)
                {
                    Vector2 previous = new Vector2(corners[i - 1].x, corners[i - 1].z);
                    Vector2 current = new Vector2(corners[i].x, corners[i].z);

                    distance += Vector2.Distance(previous, current);
                }
        }
        else 
        {
                distance = agent.remainingDistance;
        }
        return distance;
    }
    void Update()
    {
        UpdateSound();
        switch (currentState)
        {
            case SlimeAnimationState.Idle:
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) return;
                StopAgent();
                break;
            case SlimeAnimationState.Walk:
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk") || animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) return;
                MoveToDestination(waypoints[0].position);
                if (walkType == WalkType.ToDestination)
                {
                    SetFace(faces.WalkFace);
                    // // agent reaches the destination
                    float remainingDistance = GetRemainingDistance();
                    if (remainingDistance <= agent.stoppingDistance && remainingDistance != 0)
                    {
                        if(workType == WorkType.Follow)
                            walkType = WalkType.Patroll;
                        else if(workType == WorkType.Collect){
                            SetFace(faces.attackFace);
                            animator.SetTrigger("Attack");
                            if(!isCollecting){
                                isCollecting = true;
                                StopAgent();
                                InvokeRepeating("Collecting", timePerCollect, timePerCollect);
                            }
                        }
                    }
                    else{
                        if(isCollecting){
                            CancelInvoke("Collecting");
                            isCollecting = false;
                            MoveToDestination(waypoints[0].position);
                        }
                        
                    }
                }
                //Patroll
                else if (walkType == WalkType.Patroll)
                {
                    if (waypoints.Length == 0 || waypoints[0] == null) return;

                    // agent reaches the destination
                    if (agent.remainingDistance > agent.stoppingDistance)
                    {
                        walkType = WalkType.ToDestination;                        
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
                walkType = WalkType.ToDestination;
                currentState = SlimeAnimationState.Walk;
            }
            else currentState = SlimeAnimationState.Idle;

            //Debug.Log("DamageAnimationEnded");
        }

        if (message.Equals("AnimationAttackEnded"))
        {
            // currentState = SlimeAnimationState.Idle;
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
            //set idle 
        //currentState = SlimeAnimationState.Idle;
            //set follow
        // waypoints = new Transform[1];
        // waypoints[0] = interactor.gameObject.transform;
        // currentState = SlimeAnimationState.Walk;
        // walkType = WalkType.ToDestination;
        // workType = WorkType.Follow;
            //set collect ore
        // waypoints = new Transform[1];
        // waypoints[0] = GameObject.FindGameObjectWithTag("Ore").transform;
        // currentState = SlimeAnimationState.Walk;
        // walkType = WalkType.ToDestination;
        // workType = WorkType.Collect;
        interactor.gameObject.GetComponent<PlayerInventoryHolder>().pet = this;
        interactor.hotBar.ToolTip.enabled = false;
        interactor.gameObject.GetComponent<PlayerInventoryHolder>().TurnOnPetControllerUI();
        // switch (currentState)
        // {
        //     case SlimeAnimationState.Idle:
        //         // waypoints = new Transform[1];
        //         // waypoints[0] = interactor.gameObject.transform;
        //         // currentState = SlimeAnimationState.Walk;
        //         // walkType = WalkType.ToDestination;

        //         waypoints = new Transform[1];
        //         waypoints[0] = GameObject.FindGameObjectWithTag("Ore").transform;
        //         currentState = SlimeAnimationState.Walk;
        //         walkType = WalkType.ToDestination;
        //         workType = WorkType.Collect;
        //         break;
        //     case SlimeAnimationState.Walk:
        //         currentState = SlimeAnimationState.Idle;
        //         break;
        // }
    }
    public void Idle(){
        StopCollecting();
        currentState = SlimeAnimationState.Idle;
    }
    public void Follow(Transform transform){
        StopCollecting();
        waypoints = new Transform[1];
        waypoints[0] = transform;
        currentState = SlimeAnimationState.Walk;
        walkType = WalkType.ToDestination;
    }
    public void Collect(){
        StopCollecting();
        waypoints = new Transform[1];
        waypoints[0] = GameObject.FindGameObjectWithTag("Ore").transform;
        currentState = SlimeAnimationState.Walk;
        walkType = WalkType.ToDestination;
        workType = WorkType.Collect;
    }

    private void UpdateSound()
    {
        attributes.position = RuntimeUtils.ToFMODVector(transform.position); // Set the position in 3D space
        attributes.velocity = RuntimeUtils.ToFMODVector(Vector3.zero); // Set the velocity (optional)
        attributes.forward = RuntimeUtils.ToFMODVector(transform.forward); // Set the forward vector (optional)
        attributes.up = RuntimeUtils.ToFMODVector(transform.up);
        slimeWalkSound.set3DAttributes(attributes);       
        if (walkType == WalkType.ToDestination)
        {     
            PLAYBACK_STATE playbackState;
            slimeWalkSound.getPlaybackState(out playbackState);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                slimeWalkSound.start();     
            }
        }
        else
        {
            slimeWalkSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

    }
    // [System.Serializable]
    // public struct PetsSaveData
    // {
    //     public GameObject gameObject;
    //     public Vector3 position;
    //     public Quaternion rotation;

    //     public PetsSaveData(GameObject _gameObject, Vector3 _position, Quaternion _rotation)
    //     {
    //         gameObject = _gameObject;
    //         position = _position;
    //         rotation = _rotation;
    //     }
    // }
}

