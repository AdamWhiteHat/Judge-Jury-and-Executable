using NTFSLib.Objects;
using System.Diagnostics;
using System.Linq;

namespace NTFSLib.Helpers
{
	public static class DataFragmentHelpers
	{
		public static void CheckFragment(DataFragment fragment, int clusterCount, byte compressedClusters, int startingVcn, byte size, int lcn, bool isSparseExtent, bool isCompressedExtent)
		{
			Debug.Assert(clusterCount == (int)fragment.Clusters);
			Debug.Assert(startingVcn == (int)fragment.StartingVCN);
			Debug.Assert(size == fragment.FragmentSizeBytes);
			Debug.Assert(lcn == (int)fragment.LCN);
			Debug.Assert(compressedClusters == (int)fragment.CompressedClusters);

			Debug.Assert(isSparseExtent == fragment.IsSparseFragment);
			Debug.Assert(isCompressedExtent == fragment.IsCompressed);
		}

		public static byte[] SaveFragments(DataFragment[] fragments)
		{
			// Sum up the expected # of bytes needed. As compressed fragments have been compacted they have been removed - so add two bytes for each compressed fragment to compensate.
			int expectedLength = fragments.Sum(s => s.ThisObjectLength + (s.IsCompressed ? 2 : 0));

			int saveLength = DataFragment.GetSaveLength(fragments);

			Debug.Assert(expectedLength == saveLength);

			byte[] data = new byte[saveLength + 1];
			DataFragment.Save(data, 0, fragments);

			return data;
		}
	}
}