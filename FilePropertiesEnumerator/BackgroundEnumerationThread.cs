using System;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using FilePropertiesDataObject;
using System.Threading;

namespace FilePropertiesEnumerator
{
	public class BackgroundEnumerationThread
	{
		private CancellationToken cancelToken;
		private BackgroundWorker worker = null;

		private FileEnumeratorParameters configuration = null;

		private Action<string> reportProgressFuntionDelegate = null;
		private Action<List<FailSuccessCount>> completedFuntionDelegate = null;

		bool hasStarted = false;

		public BackgroundEnumerationThread(FileEnumeratorParameters parameters)
		{
			configuration = parameters;
			cancelToken = configuration.CancelToken;
			cancelToken.Register(() => CancelBackgroundWorker());

			reportProgressFuntionDelegate = configuration.ReportOutputFunction;
			completedFuntionDelegate = configuration.ReportResultsFunction;

			worker = new BackgroundWorker();
			worker.WorkerReportsProgress = true;
			worker.WorkerSupportsCancellation = true;
			worker.RunWorkerCompleted += OnRunWorkerCompleted;
			worker.ProgressChanged += Worker_ProgressChanged;
			worker.DoWork += OnDoWork;
		}

		public void BeginProcessing()
		{
			if (!hasStarted)
			{
				worker.RunWorkerAsync(configuration);
			}
		}

		private void CancelBackgroundWorker()
		{
			if (hasStarted)
			{
				worker.CancelAsync();
			}
		}

		private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			throw new NotImplementedException();
		}

		private void OnDoWork(object sender, DoWorkEventArgs e)
		{
			throw new NotImplementedException();
		}

		private void OnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			throw new NotImplementedException();
		}
	}
}
