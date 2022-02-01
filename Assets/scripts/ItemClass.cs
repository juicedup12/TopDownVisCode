using System;
using UnityEngine;


[Serializable]
    public class ItemClass
    {
        //holds data related to items
        public Color itemcolor;
        public bool Identified { get; private set; }
        public string name;
        public Sprite itemsprite;
        public Sprite UnidentifiedSprite;
        public enum ItemEffect { potion, poison, powerup, powerdown, speedup }
        public ItemEffect effect;
        string UnidentifiedName = "mystery item";


        public void UseItem(IUseItem useOn)
        {

            switch (effect)
            {
                case ItemEffect.potion:
                    useOn.UsePotion(6);
                    break;
                case ItemEffect.poison:
                    useOn.UsePoison(6);
                    break;
                default:
                    break;
            }

            //use switch statements for different item types
        }

        public string GetName()
        {
            if (Identified)
            {
                return name;
            }
            return UnidentifiedName;
        }

    public Sprite GetSprite()
    {
        if(Identified)
        {
            return itemsprite;
        }
        return UnidentifiedSprite;
    }

        public void Identify()
        {
            //float value = UnityEngine.Random.value;
            //if(value < .50)
            //{
            Identified = true;
            //}
        }

    }


