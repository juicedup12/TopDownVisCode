using UnityEngine;
using topdown;

//for preset stages and procedual stages
public interface iStageBuild 
    {
    player _player { set; }
    //setup levels will do the animation and when it's done it will give player control
    void SetupLevels();
    Vector2 getspawn();

    }

