using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FilePropertiesDataObject
{
	public class FailSuccessCount
	{
		public string Description { get; private set; }
		public int SucceededCount { get; private set; }
		public int FailedCount { get; private set; }

		public FailSuccessCount(string description)
			: this(description, 0, 0)
		{ }

		public FailSuccessCount(string description, int succeededCount, int failedCount)
		{
			Description = description;
			SucceededCount = succeededCount;
			FailedCount = failedCount;
		}

		public void IncrementSucceededCount()
		{
			SucceededCount += 1;
		}

		public void IncrementFailedCount()
		{
			FailedCount += 1;
		}

		public void CombineCounts(FailSuccessCount counts)
		{
			SucceededCount += counts.SucceededCount;
			FailedCount += counts.FailedCount;
		}

		public List<string> ToStrings()
		{
			return new List<string> {
				$"Succeeded: {SucceededCount} {Description}.",
				$"Failed: {FailedCount} {Description}.",
				""
			};
		}
	}
}
