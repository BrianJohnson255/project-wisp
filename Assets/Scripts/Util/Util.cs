using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapGen {
	public class Util {
		public static Queue<T> ListToQueue<T>(List<T> list) {
			Queue<T> queue = new Queue<T>();

			for (int i = 0; i < list.Count; i++) {
				queue.Enqueue(list[i]);
			}

			return queue;
		}
		
		public static void ShuffleList<T>(ref List<T> list, System.Random rng = null) {
			if (rng == null) {
				rng = new System.Random();
			}

			List<T> newList = new List<T>();
			int oldCount = list.Count;

			while (newList.Count != oldCount) {
				int randIndex = rng.Next(0, list.Count);
				newList.Add(list[randIndex]);
				list.RemoveAt(randIndex);
			}

			list = newList;
		}
	}
}
