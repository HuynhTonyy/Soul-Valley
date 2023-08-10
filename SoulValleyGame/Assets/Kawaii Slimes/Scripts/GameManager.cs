using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject mainSlime;
    public Button idleBut, walkBut,jumpBut,attackBut,damageBut0,damageBut1,damageBut2;
    public Camera cam;
    private void Start()
    {
        
        idleBut.onClick.AddListener( delegate { Idle(); } );
        walkBut.onClick.AddListener(delegate {  ChangeStateTo(SlimeAnimationState.Walk); });
        jumpBut.onClick.AddListener(delegate { LookAtCamera(); ChangeStateTo(SlimeAnimationState.Jump); });
        attackBut.onClick.AddListener(delegate { LookAtCamera(); ChangeStateTo(SlimeAnimationState.Attack); });
        damageBut0.onClick.AddListener(delegate { LookAtCamera(); ChangeStateTo(SlimeAnimationState.Damage); mainSlime.GetComponent<PetAI>().damType = 0; });
        damageBut1.onClick.AddListener(delegate { LookAtCamera(); ChangeStateTo(SlimeAnimationState.Damage); mainSlime.GetComponent<PetAI>().damType = 1; });
        damageBut2.onClick.AddListener(delegate { LookAtCamera(); ChangeStateTo(SlimeAnimationState.Damage); mainSlime.GetComponent<PetAI>().damType = 2; });
    }
    void Idle()
    {
        LookAtCamera();
        // mainSlime.GetComponent<PetAI>().CancelGoNextDestination();
        ChangeStateTo(SlimeAnimationState.Idle);
    }
    public void ChangeStateTo(SlimeAnimationState state)
    {
       if (mainSlime == null) return;    
       if (state == mainSlime.GetComponent<PetAI>().currentState) return;

       mainSlime.GetComponent<PetAI>().currentState = state ;
    }
    void LookAtCamera()
    {
       mainSlime.transform.rotation = Quaternion.Euler(new Vector3(mainSlime.transform.rotation.x, cam.transform.rotation.y, mainSlime.transform.rotation.z));   
    }
}
