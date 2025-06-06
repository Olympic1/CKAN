using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Security.Cryptography;

namespace CKAN
{
    public partial class NetAsyncDownloader
    {
        public abstract class DownloadTarget
        {
            public List<Uri> urls     { get; protected set; }
            public long      size     { get; protected set; }
            public string    mimeType { get; protected set; }

            protected DownloadTarget(List<Uri> urls,
                                     long      size     = 0,
                                     string    mimeType = "")
            {
                this.urls     = urls;
                this.size     = size;
                this.mimeType = mimeType;
            }

            public abstract long CalculateSize();
            public abstract void DownloadWith(ResumingWebClient wc, Uri url,
                                              HashAlgorithm? hasher,
                                              CancellationToken? cancelToken = default);

            public override string ToString() => string.Join(", ", urls);
        }

        public sealed class DownloadTargetFile : DownloadTarget
        {
            public string filename { get; private set; }

            public DownloadTargetFile(List<Uri> urls,
                                      string?   filename = null,
                                      long      size     = 0,
                                      string    mimeType = "")
                : base(urls, size, mimeType)
            {
                this.filename = filename ?? Path.GetTempFileName();
            }

            public DownloadTargetFile(Uri     url,
                                      string? filename = null,
                                      long    size     = 0,
                                      string  mimeType = "")
                : this(new List<Uri> { url }, filename, size, mimeType)
            {
            }

            public override long CalculateSize()
            {
                size = new FileInfo(filename).Length;
                return size;
            }

            public override void DownloadWith(ResumingWebClient wc, Uri url,
                                              HashAlgorithm? hasher,
                                              CancellationToken? cancelToken = default)
            {
                wc.DownloadFileAsyncWithResume(url, filename, size, hasher, cancelToken);
            }

            public override string ToString() => $"{base.ToString()} => {filename}";
        }

        public sealed class DownloadTargetStream : DownloadTarget, IDisposable
        {
            public Stream contents { get; private set; }

            public DownloadTargetStream(List<Uri> urls,
                                        long      size     = 0,
                                        string    mimeType = "")
                : base(urls, size, mimeType)
            {
                contents = new MemoryStream();
            }

            public DownloadTargetStream(Uri    url,
                                        long   size     = 0,
                                        string mimeType = "")
                : this(new List<Uri> { url }, size, mimeType)
            {
            }

            public override long CalculateSize()
            {
                size = contents.Length;
                return size;
            }

            public override void DownloadWith(ResumingWebClient wc, Uri url,
                                              HashAlgorithm? hasher,
                                              CancellationToken? cancelToken = default)
            {
                wc.DownloadFileAsyncWithResume(url, contents, hasher);
            }

            public void Dispose()
            {
                // Close the stream
                contents.Dispose();
            }
        }

    }
}
