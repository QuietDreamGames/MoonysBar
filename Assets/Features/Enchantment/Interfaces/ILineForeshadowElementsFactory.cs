using System.Collections.Generic;
using UnityEngine;

namespace Features.Enchantment.Interfaces
{
    public interface ILineForeshadowElementsFactory
    {
        GameObject CreateLineEnd();

        List<GameObject> CreateLineParts(int count);
    }
}
