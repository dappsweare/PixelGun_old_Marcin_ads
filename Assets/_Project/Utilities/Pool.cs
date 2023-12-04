using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pool {
	public class Pool<T> : IEnumerable where T : Component {

		public List<T> members = new List<T>();
		public HashSet<T> unavailable = new HashSet<T>();
		public T prefab = null;
		public Transform parent = null;

		public Pool(T prefab, Transform parent, int poolSize) {
			this.prefab = prefab;
			this.parent = parent;

			for (int i = 0; i < poolSize; i++) {
				Create();
			}
		}

		public T Allocate() {
			for (int i = 0; i < members.Count; i++) {
				if (!unavailable.Contains(members[i])) {
					unavailable.Add(members[i]);
					return members[i];
				}
			}
			T newMember = Create();
			unavailable.Add(newMember);
			return newMember;
		}

		public void Release(T member) {
			unavailable.Remove(member);
		}

		T Create() {
			T member = GameObjects.GOInstantiate(prefab, parent);
			member.gameObject.SetActive(false);
			members.Add(member);
			return member;
		}

		IEnumerator IEnumerable.GetEnumerator() => (IEnumerator<T>)members.GetEnumerator();
	}
}