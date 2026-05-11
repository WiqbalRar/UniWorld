using System;
using System.Collections.Generic;

namespace VoxelEngine
{
	public class ObjectPool<T>
	{
		private int poolSize;

		private Stack<T> poolObjects;

		public int Count => poolObjects.Count;

		public ObjectPool(int poolSize)
		{
			this.poolSize = poolSize;
			poolObjects = new Stack<T>(poolSize);
		}

		public bool Add(T obj)
		{
			if (poolObjects.Count >= poolSize)
			{
				return false;
			}
			poolObjects.Push(obj);
			return true;
		}

		public T Get()
		{
			if (poolObjects.Count <= 0)
			{
				return Activator.CreateInstance<T>();
			}
			return poolObjects.Pop();
		}
	}
}
