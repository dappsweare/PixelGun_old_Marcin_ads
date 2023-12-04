using System;
using UnityEngine;

namespace Global {
    public class TutorialManager : Singleton<TutorialManager> {

        private void Start() {
            Debug.Log("TutorialManager()");
        }
    }
}