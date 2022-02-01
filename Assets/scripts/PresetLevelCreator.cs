using UnityEngine;
using topdown;


class PresetLevelCreator : MonoBehaviour, iLevelBuild
{
    [SerializeField]
    GameObject[] levels;
    [SerializeField]
    private Transform ActiveDoor;


    public void SetupLevels(player player)
    {
        //get access to the active door of the first level in levels[] and return it
        for (int i = 0; i < levels.Length; i++)
        {
            if(i == 0)
            {
                levels[i].SetActive(true);
            }
            levels[i].GetComponent<RoomGen>().player = player;
        }
       

    }


    public Vector2 getspawn()
    {
        return ActiveDoor.position;
    }
}

