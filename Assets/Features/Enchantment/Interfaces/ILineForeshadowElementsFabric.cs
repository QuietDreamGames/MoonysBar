using System.Collections.Generic;
using UnityEngine;

namespace Features.Enchantment.Interfaces
{
    public interface ILineForeshadowElementsFabric
    {
        GameObject CreateLineEnd();

        List<GameObject> CreateLineParts(int count);
    }
}
