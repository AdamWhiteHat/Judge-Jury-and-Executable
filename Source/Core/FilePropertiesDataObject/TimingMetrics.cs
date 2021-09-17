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
	public class TimingMetrics : IDisposable
	{

		#region Static Members

		private static Dictionary<TimingMetric, Stopwatch> _timingDictionary;

		static TimingMetrics()
		{
			ResetMetrics();
		}

		private static void StartTimer(TimingMetric section)
		{
			_timingDictionary[section].Start();
		}

		private static void StopTimer(TimingMetric section)
		{
			_timingDictionary[section].Stop();
		}

		private static void StopAllTimers()
		{
			foreach (var kvp in _timingDictionary)
			{
				if (kvp.Value.IsRunning)
				{
					kvp.Value.Stop();
				}
			}
		}

		public static void ResetMetrics()
		{
			if (_timingDictionary != null && _timingDictionary.Any())
			{
				_timingDictionary.Clear();
			}

			_timingDictionary = new Dictionary<TimingMetric, Stopwatch>();
			foreach (TimingMetric section in Enum.GetValues(typeof(TimingMetric)))
			{
				_timingDictionary.Add(section, new Stopwatch());
			}
		}

		public static Stopwatch GetTimer(TimingMetric section)
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

		public static string[] GetReport()
		{
			StopAllTimers();
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

		#endregion

		#region Instance Members

		private TimingMetric _section;

		public TimingMetrics(TimingMetric section)
		{
			_section = section;
			StartTimer(_section);
		}

		public void Dispose()
		{
			StopTimer(_section);
		}

		public override string ToString()
		{
			return string.Join(Environment.NewLine, GetReport());
		}

		#endregion

	}
}
