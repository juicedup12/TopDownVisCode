using UnityEngine;
using topdown;

//for handling transitions of prebuilt stages
//holds array of IRoomTransitioner 
class PresetLevelCreator : MonoBehaviour, iStageBuild
{
    [SerializeField]
    GameObject[] TransitionObjects;
    IRoomTransitioner[] Transitioners;
    [SerializeField]
    private Transform ActiveDoor;
    int TransitionerIndex = 0;
    player _Player;

    public player _player { set => _Player = value; }

    private void Start()
    {
        Transitioners = new IRoomTransitioner[TransitionObjects.Length];
        for (int i = 0; i < TransitionObjects.Length; i++)
        {
            Transitioners[i] = TransitionObjects[i].GetComponent<IRoomTransitioner>();
        }
        print(_Player == null ? "no player " : " Player assigned ");
        _Player.SetPosAndWalkDir(ActiveDoor.position, Vector2.up);
    }

    //moves to next roomtransitioner whenever this is called until it reaches the final one
    //when final one is reached player will regain control
    public void SetupLevels()
    {
        print("transition index is " + TransitionerIndex + " out of " + Transitioners.Length);
        if(TransitionerIndex >= Transitioners.Length)
        {
            print("all transitions are done");
            _Player.SequenceDone = true;
            return;
        }
        
        Transitioners[TransitionerIndex].DoRoomTransition(this);
        TransitionerIndex++;

    }


    public Vector2 getspawn()
    {
        return ActiveDoor.position;
    }
}

