using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Glasswall.Kernel.Compression;

namespace Glasswall.Common.Compression.Deflation
{
    public class DeflationCompressor : ICompressor
    {
        public async Task<Stream> Compress(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            var compressStream = new MemoryStream();

            var compressor = new DeflateStream(compressStream, CompressionMode.Compress);
            {
                await stream.CopyToAsync(compressor);
                compressor.Flush();
            }
            compressStream.Position = 0;
            return compressStream;
        }

        public async Task<Stream> Decompress(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            var output = new MemoryStream();

            using (var decompressor = new DeflateStream(stream, CompressionMode.Decompress))
                await decompressor.CopyToAsync(output);

            output.Position = 0;
            return output;
        }
    }
}
