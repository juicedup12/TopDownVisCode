using UnityEngine;

namespace topdown
{
    [CreateAssetMenu(fileName = "ItemObj", menuName = "ScriptableObjects/Item", order = 1)]
    public class ItemObject : ScriptableObject
    {

        public Sprite ItemSprite;

        public enum ItemType { room, global, inventory }
        public ItemType TypeOfItem;
        public GameObject ItemPrefab;
        public SpawnEvents spawnType;
        public RuntimeAnimatorController ItemAnim;
        [SerializeField]
        public ItemClass ItemToAdd;

    }

    
}
