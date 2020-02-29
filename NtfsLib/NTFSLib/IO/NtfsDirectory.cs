using System.Linq;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using NTFSLib.NTFS;
using NTFSLib.Objects;
using NTFSLib.Utilities;
using NTFSLib.Objects.Enums;
using NTFSLib.Objects.Specials;
using NTFSLib.Objects.Attributes;

namespace NTFSLib.IO
{
	public class NtfsDirectory : NtfsFileEntry
	{
		private const string DirlistAttribName = "$I30";

		private AttributeIndexRoot _indexRoot;
		private AttributeIndexAllocation[] _indexes;

		internal NtfsDirectory(NTFSWrapper ntfsWrapper, FileRecord record, AttributeFileName fileName)
			: base(ntfsWrapper, record, fileName)
		{
			Debug.Assert(record.Flags.HasFlag(FileEntryFlags.Directory));
			PrepRecord();
		}

		private void PrepRecord()
		{
			// Ensure we have all INDEX attributes at hand
			bool parseLists = false;
			foreach (AttributeList list in MFTRecord.Attributes.OfType<AttributeList>())
			{
				foreach (AttributeListItem item in list.Items)
				{
					if (item.BaseFile.Equals(MFTRecord.FileReference) &&
						(item.Type == AttributeType.INDEX_ROOT || item.Type == AttributeType.INDEX_ALLOCATION))
					{
						// We need to parse lists
						parseLists = true;
					}
				}
			}

			if (parseLists)
			{
				NTFSWrapper.ParseAttributeLists(MFTRecord);
			}

			bool containsItem = MFTRecord.Attributes.Any(s => s.AttributeName == DirlistAttribName);
			if (containsItem)
			{
				IEnumerable<AttributeIndexRoot> matches = MFTRecord.Attributes.OfType<AttributeIndexRoot>().Where(s => s.AttributeName == DirlistAttribName);
				if (!matches.Any()) // Does list contain any items?
				{
					return; // Bail if no IndexRoot Attributes where AttributeName == DirlistAttribName
				}

				// IEnumerable.Single(predicate) returns the single item that matches the predicate,
				// and throws if the number of matches are not exactly one. That is, if it matches zero items or many items.
				// In our case, the exception message was "Sequence contains no matching element", so it was empty.
				//_indexRoot = MFTRecord.Attributes.OfType<AttributeIndexRoot>().Single(s => s.AttributeName == DirlistAttribName);


				// First will return the first item in the list, ignoring the rest.
				// It will throw if there is no items in the list, but wont if there are many. 
				_indexRoot = matches.First();

			}
			else
			{
				// No point in continuing in this case, as it will just waste time parsing IndexAllocations below, 
				// but never iterate them because _indexRoot == null
				return;

				//using (System.IO.StreamWriter w = System.IO.File.AppendText("D:\\Log\\faillog.txt"))
				//{
				//    w.WriteLine("failed to get indexroot for mft number " + MFTRecord.MFTNumber.ToString());
				//}
			}


			// Get allocations
			_indexes = MFTRecord.Attributes.OfType<AttributeIndexAllocation>().Where(s => s.AttributeName == DirlistAttribName).ToArray();

			foreach (AttributeIndexAllocation index in _indexes)
			{
				NTFSWrapper.ParseNonResidentAttribute(index);
			}

			// Get bitmap of allocations
			// var bitmap = MFTRecord.Attributes.OfType<AttributeBitmap>().Single(s => s.AttributeName == DirlistAttribName);
		}

		public IEnumerable<NtfsDirectory> ListDirectories()
		{
			return ListChildsLazy().OfType<NtfsDirectory>();
		}

		public IEnumerable<NtfsFile> ListFiles()
		{
			return ListChildsLazy().OfType<NtfsFile>();
		}
		
		public IEnumerable<NtfsFileEntry> ListChildsLazy()
		{
			if (_indexRoot == null)
			{
				yield break;
			}

			if (_indexRoot.IndexFlags.HasFlag(MFTIndexRootFlags.LargeIndex))
			{
				foreach (AttributeIndexAllocation index in _indexes)
				{
					foreach (IndexEntry indexEntry in index.Entries)
					{
						NtfsFileEntry entry = CreateEntry(indexEntry);
						yield return entry;
					}
				}
			}

			foreach (IndexEntry rootEntry in _indexRoot.Entries)
			{
				NtfsFileEntry entry = CreateEntry(rootEntry);
				yield return entry;
			}

			yield break;
		}

		public override string ToString()
		{
			return FileName.FileName + "\\";
		}

		#region Dead Code

		//public IEnumerable<NtfsFileEntry> ListChilds(bool uniqueOnly)
		//{
		//	IEnumerable<NtfsFileEntry> results = null;

		//	if (_indexRoot != null)
		//	{
		//		IEnumerable<IndexEntry> enumerableEntries = _indexRoot.Entries.Select(e => e);

		//		if (_indexRoot.IndexFlags.HasFlag(MFTIndexRootFlags.LargeIndex))
		//		{
		//			enumerableEntries = enumerableEntries.Concat(_indexes.SelectMany(i => i.Entries.Select(e => e)));
		//		}

		//		if (uniqueOnly)
		//		{
		//			enumerableEntries = enumerableEntries.Distinct();
		//		}

		//		results = enumerableEntries.Select(ie => CreateEntry(ie));
		//	}

		//	return results;


		//	//if (uniqueOnly && _indexRoot != null)
		//	//{
		//	//FileNamespaceComparer comparer = new FileNamespaceComparer();

		//	//// Key: uint
		//	//// Value: NtfsFileEntry
		//	//UIntEqualityComparer hashtableComparer = new UIntEqualityComparer();
		//	//Hashtable entries = new Hashtable(hashtableComparer);

		//	//foreach (IndexEntry entry in _indexRoot.Entries)
		//	//{
		//	//	if (entries.ContainsKey(entry.FileRefence.FileId))
		//	//	{
		//	//		// Is this better?
		//	//		int comp = comparer.Compare(entry.ChildFileName.FilenameNamespace, ((NtfsFileEntry)entries[entry.FileRefence.FileId]).FileName.FilenameNamespace);

		//	//		if (comp == 1)
		//	//		{
		//	//			// New entry is better
		//	//			entries[entry.FileRefence.FileId] = CreateEntry(entry.FileRefence.FileId, entry.ChildFileName);
		//	//		}
		//	//	}
		//	//	else
		//	//	{
		//	//		entries[entry.FileRefence.FileId] = CreateEntry(entry.FileRefence.FileId, entry.ChildFileName);
		//	//	}
		//	//}
		//	//foreach (AttributeIndexAllocation index in _indexes)
		//	//{
		//	//	foreach (IndexEntry entry in index.Entries)
		//	//	{
		//	//		if (entries.ContainsKey(entry.FileRefence.FileId))
		//	//		{
		//	//			// Is this better?
		//	//			int comp = comparer.Compare(entry.ChildFileName.FilenameNamespace, ((NtfsFileEntry)entries[entry.FileRefence.FileId]).FileName.FilenameNamespace);
		//	//			if (comp == 1)
		//	//			{
		//	//				// New entry is better
		//	//				entries[entry.FileRefence.FileId] = CreateEntry(entry.FileRefence.FileId, entry.ChildFileName);
		//	//			}
		//	//		}
		//	//		else
		//	//		{
		//	//			entries[entry.FileRefence.FileId] = CreateEntry(entry.FileRefence.FileId, entry.ChildFileName);
		//	//		}
		//	//	}
		//	//}
		//	//}

		//	//foreach (NtfsFileEntry value in entries.Values)
		//	//{
		//	//	yield return value;
		//	//}
		//	//}
		//	//else if (_indexRoot != null)
		//	//{
		//	//	foreach (IndexEntry entry in _indexRoot.Entries)
		//	//	{
		//	//		yield return CreateEntry(entry.FileRefence.FileId, entry.ChildFileName);
		//	//	}

		//	//	if (_indexRoot.IndexFlags.HasFlag(MFTIndexRootFlags.LargeIndex))
		//	//	{
		//	//		foreach (AttributeIndexAllocation index in _indexes)
		//	//		{
		//	//			foreach (IndexEntry entry in index.Entries)
		//	//			{
		//	//				yield return CreateEntry(entry.FileRefence.FileId, entry.ChildFileName);
		//	//			}
		//	//		}
		//	//	}
		//	//}
		//}

		#endregion

	}
}
