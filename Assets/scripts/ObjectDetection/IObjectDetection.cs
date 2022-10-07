using UnityEngine;
namespace InteractableSelect
{
    interface IObjectDetection
    {
        bool Check();
        Transform GetSelection();
    }
}
