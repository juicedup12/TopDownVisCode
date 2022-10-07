using UnityEngine;
using topdown;


// handles making a level and placing player in spawn point
//for use with preset stages and procedual stages
//attatch to objects with roomgen component
class StageBuildManager : MonoBehaviour

{
    [SerializeField]
    iStageBuild StageBuilder;
    [SerializeField]
    player _player;

    private void Awake()
    {

        StageBuilder = GetComponent<iStageBuild>();
        StageBuilder._player = this._player;
    }

    private void Start()
    {
        //give player the vector 2 of where to spawn from setuplevels()
        //level buildier is in charge of playing transitions
        //and giving control to player when transitions are done

        StageBuilder.SetupLevels();

        //player will be in the waiting state, until level builder tells player that the sequence is done
        //_player.WalkInDir(levelBuilder.getspawn(), Vector2.up);
    }

}

