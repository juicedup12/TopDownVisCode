using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace topdown
{
    //class to connect other classes with multiple tile group transitioners
    //don't hold specific subclasses, look for base class and hold an array of the basetype
    public class TileTransitionManager : MonoBehaviour
    {

        [SerializeField] Tilemap tmap;
        TileGroupTransition[] TileGroups;
        [SerializeField] TimeLineBindingController TimeLineBinder;

        private void OnEnable()
        {
            BaseRoom.OnRoomActive += ListenToRoom;
        }

        private void OnDisable()
        {
            BaseRoom.OnRoomActive -= ListenToRoom;
        }

        //triggers when a base room becomes active
        void ListenToRoom(BaseRoom room)
        {
            print("tile transition manager listen callback received from " + room.gameObject);
            if (room is ITileTransitionDataProvider)
            {
                InitialiseTiles(room.transform.position);
                DesignateTileGroupByType(room as ITileTransitionDataProvider);
            }
        }

        public void InitialiseTiles(Vector3 GridOffset)
        {
            print("initialising tiles at position " + GridOffset);
            TileGroups = GetComponents<TileGroupTransition>();
            foreach (TileGroupTransition transition in TileGroups)
            {
                transition.SetTilemap = tmap;
                transition.SetOffset = new Vector3Int(Mathf.RoundToInt(GridOffset.x), Mathf.RoundToInt(GridOffset.y),0);
            }
        }


        public TileGroupTransition DesignateRandomTileGroup()
        {
            if (TileGroups == null || TileGroups.Length < 1) 
                return null;
            int RandomTileTransition = Random.Range(0, TileGroups.Length);
            TileGroupTransition CurrentTransition = TileGroups[RandomTileTransition];
            return CurrentTransition;

        }

        //is provided with a scriptable object by roomgen
        //designate tile group
        //and connects it with timeline binding
        public void DesignateTileGroupByType(ITileTransitionDataProvider TransitionClient) 
        {
            TileGroupTransition DesignatedTileGroup = null;
                switch (TransitionClient.GetTileDataHandler.GetTileTransitionType)
                {
                    case TileGroupTransitionType.Rotate:
                        DesignatedTileGroup = FindTileTransition<TileGroupRotate>();
                        break;
                    case TileGroupTransitionType.Scale:
                        DesignatedTileGroup = FindTileTransition<TileGroupScale>();
                        break;
                    case TileGroupTransitionType.Transform:
                        print("there is no transform tile transition yet");
                        break;
                    case TileGroupTransitionType.Random:
                        DesignatedTileGroup = DesignateRandomTileGroup();
                        break;
                    default:
                        break;
                }
            
            if(DesignatedTileGroup == null)
            {
                return;
            }
            DesignatedTileGroup.NewTiles(TransitionClient.GetTileDataHandler.GetTilesToTransform);
            print("room enter tile transition is " + DesignatedTileGroup);
            if (!TimeLineBinder) return;
            TimeLineBinder.ChangeTransitionTrackBinding(DesignatedTileGroup);

        }


        TileGroupTransition FindTileTransition<T>()
        {
            foreach (TileGroupTransition item in TileGroups)
            {
                if(item is T)
                {
                    print(gameObject + " found transition type successfully " + item);
                    return item;
                }
            }
            return null;
        }
    }
}