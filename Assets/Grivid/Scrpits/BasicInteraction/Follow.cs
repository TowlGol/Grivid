using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using UnityEngine;
public class Follow :MonoBehaviour{
    private void Start() {
        gameObject.AddComponent<FollowMeToggle>();
        gameObject.AddComponent<SolverHandler>();
        gameObject.AddComponent<RadialView>();
    }
}