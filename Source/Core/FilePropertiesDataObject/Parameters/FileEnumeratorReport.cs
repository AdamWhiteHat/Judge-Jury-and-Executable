using System;
using System.Linq;
using System.Collections.Generic;

namespace FilePropertiesDataObject.Parameters
{
	public class FileEnumeratorReport
	{
		public List<FailSuccessCount> Counts { get; private set; }

		public string[] Timings { get; private set; }

		public FileEnumeratorReport(List<FailSuccessCount> counts)
		{
			Counts = counts;
			Timings = TimingMetrics.GetReport();
		}
	}
}
