using UnityEngine;

namespace InteractableSelect
{
    interface ISelectionResponse
    {
        
        void OnSelect(Transform item);
        void OnDeselect();
        
    }
}
