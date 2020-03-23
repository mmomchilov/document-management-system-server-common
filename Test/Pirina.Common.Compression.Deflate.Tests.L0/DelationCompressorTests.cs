using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Glasswall.Common.Compression.Deflation;
using NUnit.Framework;

namespace Glasswall.Common.Compression.Deflate.Tests.L0
{
    [TestFixture]
    [Category("Glasswall.Common.Compression.Deflate.Tests.L0")]
    public class DelationCompressorTests
    {
        private readonly byte[] _helloWorldBytes = {72, 101, 108, 108, 111, 32, 87, 111, 114, 108, 100, 33};
        
        [TestFixture]
        public class CompressMethod : DelationCompressorTests
        {
            [Test]
            public void If_Stream_Is_Null_The_An_Argument_Null_Will_Be_Thrown()
            {
                //Arrange
                var compressor = new DeflationCompressor();
                
                //Act
                async Task<Stream> TestDelegate() => await compressor.Compress(null);

                //Assert
                Assert.That(TestDelegate, Throws.ArgumentNullException.With.Property("ParamName").EqualTo("stream"));
            }

            [Test]
            public async Task Compression_Of_Small_Stream_Will_Result_In_Larger_Stream()
            {
                //Arrange
                var ms = new MemoryStream(_helloWorldBytes);
                var compressor = new DeflationCompressor();

                //Act
                var result = await compressor.Compress(ms);

                //Assert
                Assert.That(result.Length, Is.GreaterThan(ms.Length));
            }

            [Test]
            public async Task Stream_Can_Be_Compressed()
            {
                //Arrange
                var stream = GetTextStream();
                var compressor = new DeflationCompressor();

                //Act
                var result = await compressor.Compress(stream);

                //Assert
                Assert.That(result.Length, Is.LessThan(stream.Length));
            }
        }

        [TestFixture]
        public class DecompressMethod : DelationCompressorTests
        {
            [Test]
            public void If_Stream_Is_Null_The_An_Argument_Null_Will_Be_Thrown()
            {
                //Arrange
                var compressor = new DeflationCompressor();

                //Act
                async Task<Stream> TestDelegate() => await compressor.Decompress(null);

                //Assert
                Assert.That(TestDelegate, Throws.ArgumentNullException.With.Property("ParamName").EqualTo("stream"));
            }

            [Test]
            public async Task Decompressing_A_Small_Stream_Will_Result_In_Smaller_Stream()
            {
                //Arrange
                var ms = new MemoryStream(_helloWorldBytes);
                var expectedText = new StreamReader(ms).ReadToEnd();
                ms.Position = 0;
                var compressor = new DeflationCompressor();
                var compressedStream = await compressor.Compress(ms);
                var compressedStreamLength = compressedStream.Length;

                //Act
                var result = await compressor.Decompress(compressedStream);
                var actualText = new StreamReader(result).ReadToEnd();

                //Assert
                Assert.That(result.Length, Is.LessThan(compressedStreamLength));
                Assert.AreEqual(expectedText, actualText);
            }

            [Test]
            public async Task Stream_Can_Be_Decompressed()
            {
                //Arrange
                var stream = GetTextStream();
                var expectedText = new StreamReader(stream).ReadToEnd();
                stream.Position = 0;
                var compressor = new DeflationCompressor();
                var compressedStream = await compressor.Compress(stream);
                var compressedStreamLength = compressedStream.Length;

                //Act
                var result = await compressor.Decompress(compressedStream);
                var actualText = new StreamReader(result).ReadToEnd();

                //Assert
                Assert.That(result.Length, Is.GreaterThan(compressedStreamLength));
                Assert.AreEqual(expectedText, actualText);
            }
        }

        private Stream GetTextStream()
        {
            return Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("Glasswall.Common.Compression.Deflate.Tests.L0.Lorem ipsum.txt");
        }
    }
}
