using System;
using System.Collections.Generic;

namespace VoxelEngine
{
	public class ChunkQueue
	{
		private struct ChunkQueueItem : IComparable<ChunkQueueItem>
		{
			public float distance;

			public Vector3i pos;

			public ChunkQueueItem(float distance, Vector3i pos)
			{
				this.distance = distance;
				this.pos = pos;
			}

			public int CompareTo(ChunkQueueItem other)
			{
				return distance.CompareTo(other.distance);
			}
		}

		private const int MAX_QUEUE_SIZE = 256;

		private List<ChunkQueueItem> items = new List<ChunkQueueItem>(256);

		private HashSet<Vector3i> hashSet = new HashSet<Vector3i>();

		public int Count => items.Count;

		public void Enqueue(float distance, Vector3i pos)
		{
			if (items.Count == 0)
			{
				items.Add(new ChunkQueueItem(distance, pos));
				hashSet.Add(pos);
				return;
			}
			if (items.Count >= 256)
			{
				if (items[items.Count - 1].distance < distance)
				{
					return;
				}
				hashSet.Remove(items[255].pos);
				items.RemoveAt(255);
			}
			ChunkQueueItem item = new ChunkQueueItem(distance, pos);
			int num = items.BinarySearch(item);
			if (num >= 0)
			{
				items.Insert(num, item);
			}
			else
			{
				items.Insert(~num, item);
			}
			hashSet.Add(pos);
		}

		public bool Dequeue(out Vector3i pos)
		{
			if (items.Count == 0)
			{
				pos = default(Vector3i);
				return false;
			}
			pos = items[0].pos;
			items.RemoveAt(0);
			return true;
		}

		public void Remove(Vector3i pos)
		{
			hashSet.Remove(pos);
		}

		public bool IsFull()
		{
			return hashSet.Count >= 256;
		}

		public bool Contains(Vector3i pos)
		{
			return hashSet.Contains(pos);
		}

		public void Clear()
		{
			items.Clear();
			hashSet.Clear();
		}
	}
}
