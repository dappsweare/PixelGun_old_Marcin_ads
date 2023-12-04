using System.Collections.Generic;
using UnityEngine;

namespace Utilities {
    [System.Serializable]
    public class Locker {

        [Header("Info")]
        private List<string> currentTags = null;
        private bool overrideSameTags = true;

        public Locker(bool overrideSameTags = true) {
            this.currentTags = new List<string>();
            this.overrideSameTags = overrideSameTags;
        }

        public void AddTag(string tag) {
            string finalTag = GetTag(tag);
            if (Contains(finalTag) && !overrideSameTags) return;
            currentTags.Add(finalTag);
        }

        public void RemoveTag(string tag) => currentTags.Remove(GetTag(tag));


        public bool IsLocked() => currentTags.Count > 0;

        public bool Contains(string tag) => currentTags.Contains(GetTag(tag));


        private string GetTag(string tag) => tag.ToUpper();
    }
}