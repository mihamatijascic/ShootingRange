using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level", order = 1)]
    public class Level : ScriptableObject
    {
        [SerializeField] public float minSpeed = 0.001f;
        [SerializeField] public float maxSpeed = 0.002f;
        [SerializeField] public float timeUpForTarget = 3f;
        [SerializeField] public float posibilityOfCreatingCivilian = 0f;
        [SerializeField] public float posibilityOfMoving = 0.1f;
        [SerializeField] public float bringUpTargetTime = 5f;
        [SerializeField] public int spawnNumber = 3;
        //[SerializeField] public float duraiton = 30f;
    }
}
