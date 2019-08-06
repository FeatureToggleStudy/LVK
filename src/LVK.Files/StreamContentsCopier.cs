using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Humanizer;

using JetBrains.Annotations;

using LVK.Core;

namespace LVK.Files
{
    internal class StreamContentsCopier
    {
        private const int _BufferSize = 256 * 1024; // 256KB;

        [NotNull]
        private readonly FileStream _SourceStream;

        [NotNull]
        private readonly FileStream _TargetStream;

        [NotNull]
        private readonly IProgress<string> _ProgressReporter;

        private DateTime _NextReportAt = DateTime.Now;

        private long _LeftToCopy;
        private long _TotalCopied;
        private readonly long _TotalToCopy;

        [NotNull]
        private byte[] _Buffer1 = new byte[_BufferSize];

        [NotNull]
        private byte[] _Buffer2 = new byte[_BufferSize];

        public StreamContentsCopier(
            [NotNull] FileStream sourceStream, [NotNull] FileStream targetStream, [NotNull] IProgress<string> progressReporter)
        {
            _SourceStream = sourceStream;
            _TargetStream = targetStream;
            _ProgressReporter = progressReporter;

            _LeftToCopy = _SourceStream.Length;
            _TotalToCopy = _LeftToCopy;
        }

        private void ReportProgress()
        {
            if (DateTime.Now < _NextReportAt)
                return;

            double percent;
            if (_TotalToCopy == 0)
                percent = 100;
            else
                percent = _TotalCopied * 100.0 / _TotalToCopy;

            _ProgressReporter.Report($"{_TotalCopied.Bytes().ToString("0.#")} / {_TotalToCopy.Bytes().ToString("0.#")} ({percent:0}%)");
            _NextReportAt = DateTime.Now.AddSeconds(5);
        }

        private async Task<int> ReadFromSourceAsync([NotNull] byte[] buffer, CancellationToken cancellationToken)
        {
            int inBuffer = await _SourceStream.ReadAsync(buffer, 0, _BufferSize, cancellationToken).NotNull();
            if (inBuffer == 0 && _LeftToCopy > 0)
                throw new InvalidOperationException("Unable to complete file copy");

            return inBuffer;
        }

        [NotNull]
        public async Task Copy(CancellationToken cancellationToken)
        {
            int inBuffer1 = await ReadFromSourceAsync(_Buffer1, cancellationToken);
            if (inBuffer1 == 0)
                return;

            ReportProgress();

            while (_LeftToCopy > 0 && inBuffer1 > 0)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var writeTask = WriteToTargetAsync(_Buffer1, inBuffer1, cancellationToken);
                int inBuffer2 = await ReadFromSourceAsync(_Buffer2, cancellationToken);
                await writeTask;

                _TotalCopied += inBuffer1;
                ReportProgress();

                _LeftToCopy -= inBuffer1;

                (_Buffer1, _Buffer2) = (_Buffer2, _Buffer1);
                inBuffer1 = inBuffer2;
            }

            _NextReportAt = DateTime.Now;
            ReportProgress();

            if (_LeftToCopy > 0)
                throw new InvalidOperationException("Unable to complete operation");
        }

        [NotNull]
        private Task WriteToTargetAsync(byte[] buffer, int inBuffer, CancellationToken cancellationToken)
            => _TargetStream.WriteAsync(buffer, 0, inBuffer, cancellationToken).NotNull();
    }
}