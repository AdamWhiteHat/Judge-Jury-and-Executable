using System;
using System.Linq;
using System.Collections.Generic;

namespace FilePropertiesDataObject.Parameters
{
	public class FileEnumeratorReport
	{
		public List<FailSuccessCount> Counts { get; set; }
		public TimingMetrics Timings { get; set; }

		public FileEnumeratorReport(List<FailSuccessCount> counts, TimingMetrics timings)
		{
			Counts = counts;
			Timings = timings;
		}
	}
}
