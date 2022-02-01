using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace topdown
{
    public class UIMinimapGird : MonoBehaviour
    {
        float displacement = 30;
        public GameObject TilePrefab;
        Vector2 currentTilepos;
        public GameObject StartingTile;
        Vector2 anchoredPosition;
        Vector2 anchorMin;
        Vector2 anchorMax;
        Vector2 sizedelta;
        List<Vector3> Positions = new List<Vector3>();
        RectTransform ParentRect;

     

        // Start is called before the first frame update
        void Start()
        {
            RectTransform TileRect = StartingTile.GetComponent<RectTransform>();

            currentTilepos = TileRect.anchoredPosition;
            //print(currentTilepos);
            anchoredPosition = TileRect.anchoredPosition;
            ParentRect = GetComponent<RectTransform>();
            
            anchorMin = TileRect.anchorMin;
            anchorMax = TileRect.anchorMax;
            sizedelta = TileRect.sizeDelta;
            Positions.Add(anchoredPosition);
            //AddTile(Vector2.zero, Vector2.right);
        }


        public void AddTile(Vector2 position, Vector2 dir)
        {
            Vector2 ParentAnchoredpos = ParentRect.anchoredPosition;
            //print("parent anchor pos is " + ParentAnchoredpos);
            Vector3 NextPos = currentTilepos + (dir * displacement);
            currentTilepos = NextPos;
            //print("next pos is " + NextPos);


            if (Positions.Contains(NextPos))
            {
                print(NextPos + "already marked");

                ParentRect.DOAnchorPos(ParentAnchoredpos + (dir * displacement * -1), 1);
                return;
            }

            RectTransform tile = Instantiate(StartingTile).GetComponent<RectTransform>();
            tile.transform.SetParent(transform);
        
            tile.anchorMax = anchorMax;
            tile.anchorMin = anchorMin;
            tile.anchoredPosition = NextPos;
            Positions.Add(NextPos);
            anchoredPosition = tile.anchoredPosition;
            tile.localScale = Vector3.one;
            print(" new tile's anchored pos is " + anchoredPosition);
            Image tileimg = tile.GetComponent<Image>();

            var tempColor = tileimg.color;
            tempColor.a = 0;
            tileimg.color = tempColor;

            Sequence sequence = DOTween.Sequence();
            

            sequence.Append(tile.GetComponent<Image>().DOFade(1, 1f).SetEase(Ease.InBounce));

            
            sequence.Append(ParentRect.DOAnchorPos(ParentAnchoredpos + (dir * displacement * -1), 1));
        }


    }
}
