using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nea
{
	/// <summary>
	/// An interface that specifies a publicly readable Key string, mainly for use
	/// with the KeyedList class
	/// </summary>
	public interface IKeyed
	{
		String Key { get; }
	}

	/// <summary>
	/// A custom sorted list for objects that implement the IKeyed interface.
	/// </summary>
	/// <typeparam name="I">The type of object the list contains.</typeparam>
	public class KeyedList<I> : IEnumerable where I : IKeyed
	{
		I[] list;
		public int Count { get; private set; }

		/// <summary>
		/// Returns object in the list with the Key key. If it isn't in the list,
		/// the KeyedList alternative will be checked, and if it isn't found in 
		/// either, the default value for the object will be returned.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="alternative"></param>
		/// <returns></returns>
		public I this[String key, KeyedList<I> alternative = null] {
			get {
				int pos = list.Length / 2;
				int diff = list.Length / 4;

				while ((pos > Count) || (list[pos - 1].Key != key)) {

					if ((pos > Count) || (list[pos - 1].Key.CompareTo(key) > 0)) {
						pos -= diff;
					}
					else {
						pos += diff;
					}

					if (diff == 0) {
						if (alternative != null) {
							return alternative[key];
						}
						return default(I);
					}
					diff /= 2;

				}
				return list[pos - 1];
			}
		}

		/// <summary>
		/// Returns the object at the position index in the list. If the index is
		/// out of bounds, the default value for the object will be returned. 
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public I this[int index] {
			get {
				if (index < Count && index >= 0) {
					return list[index];
				}
				else return default(I);
			}
		}

		/// <summary>
		/// Initializes the KeyedList with an initial capacity determined by capacity.
		/// </summary>
		/// <param name="capacity"></param>
		public KeyedList(int capacity = 16) {
			Count = 0;
			int i = 1;
			for (; i < capacity; i += i) { }
			list = new I[i];
		}

		/// <summary>
		/// Adds one ore more items to the list.
		/// </summary>
		/// <param name="items"></param>
		public void Add(params I[] items) {
			foreach (I item in items) {
				list[Count++] = item;
				SortOne();

				if (Count >= list.Length) {
					I[] temp = new I[list.Length * 2];
					for (int i = 0; i < list.Length; i++) {
						temp[i] = list[i];
					}
					list = temp;
				}
			}
		}

		/// <summary>
		/// Removes the object with the Key key from the list.
		/// </summary>
		/// <param name="key"></param>
		public void Remove(String key) {
			int pos = 0;
			while (list[pos].Key.CompareTo(key) != 0) {
				pos++;
				if (pos == Count) return;
			}
			while (pos < Count - 1) {
				Swap(pos, pos + 1);
				pos++;
			}
			Count--;
			list[pos] = default(I);
		}

		/// <summary>
		/// Removes the item from the list.
		/// </summary>
		/// <param name="item"></param>
		public void Remove(I item) {
			Remove(item.Key);
		}

		private void SortOne() {
			int pos = Count - 1;
			while ((pos > 0) && (list[pos].Key.CompareTo(list[pos - 1].Key) < 0)) {
				Swap(pos, pos - 1);
				pos--;
			}
		}

		private void Swap(int a, int b) {
			I temp = list[a];
			list[a] = list[b];
			list[b] = temp;
		}

		/// <summary>
		/// Returns true, if the list contains an object with the Key key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool Contains(String key) {
			I result = this[key];
			if (result == null) {
				return false;
			}
			else
				return true;
		}

		/// <summary>
		/// Returns true, if the item is contained in the list.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(I item) {
			return Contains(item.Key);
		}

		public IEnumerator GetEnumerator() {
			return new KeyedListEnum<I>(list, Count);
		}

		public I[] ToArray() {
			return list;
		}
	}

	/// <summary>
	/// An enumerator class for KeyedList
	/// </summary>
	/// <typeparam name="I"></typeparam>
	public class KeyedListEnum<I> : IEnumerator
	{
		public I[] list;
		int position = -1;
		int count;

		public KeyedListEnum(I[] l, int c) {
			list = l;
			count = c;
		}

		public bool MoveNext() {
			position++;
			return (position < count);
		}

		public void Reset() {
			position = -1;
		}

		object IEnumerator.Current {
			get {
				return Current;
			}
		}

		public I Current {
			get {
				if (position < count && position >= 0) {
					return list[position];
				}
				else throw new InvalidOperationException();
			}
		}
	}

	#region generic

	/// <summary>
	/// An interface that specifies a publicly readable Key string, mainly for use
	/// with the KeyedList class
	/// </summary>
	public interface IKeyed<K> where K : IComparable
	{
		K Key { get; }
	}

	/// <summary>
	/// A custom sorted list for objects that implement the IKeyed interface.
	/// </summary>
	/// <typeparam name="I">The type of object the list contains.</typeparam>
	public class KeyedList<K, I> : IEnumerable
		where K : IComparable
		where I : IKeyed<K>
	{
		I[] list;
		public int Count { get; private set; }

		/// <summary>
		/// Returns object in the list with the Key key. If it isn't in the list,
		/// the KeyedList alternative will be checked, and if it isn't found in 
		/// either, the default value for the object will be returned.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="alternative"></param>
		/// <returns></returns>
		public I this[K key, KeyedList<K, I> alternative = null] {
			get {
				int pos = list.Length / 2;
				int diff = list.Length / 4;

				while ((pos > Count) || (list[pos - 1].Key.CompareTo(key) != 0)) {

					if ((pos > Count) || (list[pos - 1].Key.CompareTo(key) > 0)) {
						pos -= diff;
					}
					else {
						pos += diff;
					}

					if (diff == 0) {
						if (alternative != null) {
							return alternative[key];
						}
						return default(I);
					}
					diff /= 2;

				}
				return list[pos - 1];
			}
		}

		/// <summary>
		/// Returns the object at the position index in the list. If the index is
		/// out of bounds, the default value for the object will be returned. 
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public I this[int index] {
			get {
				if (index < Count && index >= 0) {
					return list[index];
				}
				else return default(I);
			}
		}

		/// <summary>
		/// Initializes the KeyedList with an initial capacity determined by capacity.
		/// </summary>
		/// <param name="capacity"></param>
		public KeyedList(int capacity = 16) {
			Count = 0;
			int i = 1;
			for (; i < capacity; i += i) { }
			list = new I[i];
		}

		/// <summary>
		/// Adds one ore more items to the list.
		/// </summary>
		/// <param name="items"></param>
		public void Add(params I[] items) {
			foreach (I item in items) {
				list[Count++] = item;
				SortOne();

				if (Count >= list.Length) {
					I[] temp = new I[list.Length * 2];
					for (int i = 0; i < list.Length; i++) {
						temp[i] = list[i];
					}
					list = temp;
				}
			}
		}

		/// <summary>
		/// Removes the object with the Key key from the list.
		/// </summary>
		/// <param name="key"></param>
		public void Remove(K key) {
			int pos = 0;
			while (list[pos].Key.CompareTo(key) != 0) {
				pos++;
				if (pos == Count) return;
			}
			while (pos < Count - 1) {
				Swap(pos, pos + 1);
				pos++;
			}
			Count--;
			list[pos] = default(I);
		}

		/// <summary>
		/// Removes the item from the list.
		/// </summary>
		/// <param name="item"></param>
		public void Remove(I item) {
			Remove(item.Key);
		}

		private void SortOne() {
			int pos = Count - 1;
			while ((pos > 0) && (list[pos].Key.CompareTo(list[pos - 1].Key) < 0)) {
				Swap(pos, pos - 1);
				pos--;
			}
		}

		private void Swap(int a, int b) {
			I temp = list[a];
			list[a] = list[b];
			list[b] = temp;
		}

		/// <summary>
		/// Returns true, if the list contains an object with the Key key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool Contains(K key) {
			I result = this[key];
			if (result == null) {
				return false;
			}
			else
				return true;
		}

		/// <summary>
		/// Returns true, if the item is contained in the list.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(I item) {
			return Contains(item.Key);
		}

		public IEnumerator GetEnumerator() {
			return new KeyedListEnum<K, I>(list, Count);
		}

		public I[] ToArray() {
			return list;
		}
	}

	/// <summary>
	/// An enumerator class for KeyedList
	/// </summary>
	/// <typeparam name="I"></typeparam>
	public class KeyedListEnum<K, I> : IEnumerator
	{
		public I[] list;
		int position = -1;
		int count;

		public KeyedListEnum(I[] l, int c) {
			list = l;
			count = c;
		}

		public bool MoveNext() {
			position++;
			return (position < count);
		}

		public void Reset() {
			position = -1;
		}

		object IEnumerator.Current {
			get {
				return Current;
			}
		}

		public I Current {
			get {
				if (position < count && position >= 0) {
					return list[position];
				}
				else throw new InvalidOperationException();
			}
		}
	}

	/// <summary>
	/// Defines a key/value pair. Implements IKeyed for use with KeyedList.
	/// </summary>
	/// <typeparam name="K"></typeparam>
	/// <typeparam name="V"></typeparam>
	public class ValuePair<K, V> : IKeyed<K>
		where K : IComparable
	{
		public K Key { get; private set; }
		public V Value { get; set; }

		public ValuePair(K key, V value) {
			Key = key;
			Value = value;
		}
	}
	#endregion
}
