using System;
using System.Collections;
using System.Collections.Generic;
using NTFSLib.Utilities;

namespace NTFSLib.IO
{
	internal class NtfsFileCache
	{
		// Key: ulong
		// Value: WeakReference
		private Hashtable _entries;
		ULongEqualityComparer hashtableComparer;

		internal NtfsFileCache()
		{
			hashtableComparer = new ULongEqualityComparer();
			_entries = new Hashtable();
		}
		

		public NtfsFileEntry Get(uint id, int filenameHashcode)
		{
			// Make combined key
			ulong key = ULongEqualityComparer.CreateKey(id, filenameHashcode);

			// Fetch			
			WeakReference tmp = null;
			if (_entries.ContainsKey(key))
			{
				tmp = (WeakReference)_entries[key];
			}

			if (tmp == null || !tmp.IsAlive)
			{
				return null;
			}

			return tmp.Target as NtfsFileEntry;
		}

		public void Set(uint id, ushort attributeId, NtfsFileEntry entry)
		{
			// Make combined key
			ulong key = ULongEqualityComparer.CreateKey(id, attributeId);

			// Set
			_entries[key] = new WeakReference(entry);
		}
	}
}