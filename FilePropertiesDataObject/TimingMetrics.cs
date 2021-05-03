using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

namespace FilePropertiesDataObject
{
	public enum TimingMetric
	{
		ParsingMFT,
		ReadingMFTBytes,
		FileHashing,
		YaraScanning,
		YaraRuleCompiling,
		CalculatingEntropy,
		GettingShellInfo,
		MiscFileProperties,
		PersistingFileProperties
	}

	/// <summary>
	/// Contains a Dictionary of Stopwatch(es) to track Timing Metrics for the application
	/// </summary>
	public class TimingMetrics
	{
		private Dictionary<TimingMetric, Stopwatch> _timingDictionary;

		public TimingMetrics()
		{
			_timingDictionary = new Dictionary<TimingMetric, Stopwatch>();
			foreach (TimingMetric section in Enum.GetValues(typeof(TimingMetric)))
			{
				_timingDictionary.Add(section, new Stopwatch());
			}
		}

		public void Start(TimingMetric section)
		{
			_timingDictionary[section].Start();
		}

		public void Stop(TimingMetric section)
		{
			_timingDictionary[section].Stop();
		}

		public void StopAll()
		{
			foreach (var kvp in _timingDictionary)
			{
				if (kvp.Value.IsRunning)
				{
					kvp.Value.Stop();
				}
			}
		}

		public Stopwatch GetMetric(TimingMetric section)
		{
			return _timingDictionary[section];
		}

		/// <summary>
		/// Formats a TimeSpan as such: 1d:2h:30m:5s:1023ms
		/// </summary>
		public static string FormatTimeSpan(TimeSpan timeSpan)
		{
			List<string> elapsedString = new List<string>();
			if (timeSpan.Days > 0)
			{
				elapsedString.Add($"{timeSpan.Days}d");
			}
			if (timeSpan.Hours > 0)
			{
				elapsedString.Add($"{timeSpan.Hours}h");
			}
			if (timeSpan.Minutes > 0)
			{
				elapsedString.Add($"{timeSpan.Minutes}m");
			}
			if (timeSpan.Seconds > 0)
			{
				elapsedString.Add($"{timeSpan.Seconds}s");
			}
			if (timeSpan.Milliseconds > 0)
			{
				elapsedString.Add($"{timeSpan.Milliseconds}ms");
			}
			return string.Join(":", elapsedString); // 1d:2h:30m:5s:1023ms
		}

		public string[] GetReport()
		{
			StopAll();
			var nonzeroKeyValuePairs = _timingDictionary.Where(kvp => kvp.Value.ElapsedTicks > 0);

			int rightPadding = nonzeroKeyValuePairs.Select(kvp => $"Metric '{Enum.GetName(typeof(TimingMetric), kvp.Key)}' took ".Length).Max();
			int leftPadding = nonzeroKeyValuePairs.Select(kvp => $"{FormatTimeSpan(kvp.Value.Elapsed)}.".Length).Max();

			// e.g. YaraFiltering took 2h:30m:5s:1023ms.
			List<string> results = nonzeroKeyValuePairs.Select(kvp => $"Metric '{Enum.GetName(typeof(TimingMetric), kvp.Key)}' took".PadRight(rightPadding) + $"{FormatTimeSpan(kvp.Value.Elapsed)}.".PadLeft(leftPadding)).ToList();

			TimeSpan sum = TimeSpan.Zero;
			foreach (TimeSpan value in nonzeroKeyValuePairs.Select(kvp => kvp.Value.Elapsed))
			{
				sum = sum.Add(value);
			}

			results.Add(new string(Enumerable.Repeat('-', rightPadding + leftPadding).ToArray()));
			results.Add("Metrics Total:".PadRight(rightPadding) + $"{FormatTimeSpan(sum)}".PadLeft(leftPadding));

			return results.ToArray();
		}

		public override string ToString()
		{
			return string.Join(Environment.NewLine, GetReport());
		}
	}
}
