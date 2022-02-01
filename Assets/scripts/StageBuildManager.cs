using UnityEngine;
using topdown;


// handles making a level and placing player in spawn point
class StageBuildManager : MonoBehaviour

{
    [SerializeField]
    iLevelBuild levelBuilder;
    [SerializeField]
    player _player;

    private void Start()
    {
        levelBuilder = GetComponent<iLevelBuild>();
        //give player the vector 2 of where to spawn from setuplevels()
        levelBuilder.SetupLevels(_player);


        _player.WalkInDir(levelBuilder.getspawn(), Vector2.up);
    }

}

