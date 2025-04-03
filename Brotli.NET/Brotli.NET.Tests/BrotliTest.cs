using System;
using Xunit;
using Brotli;
using System.IO;
using System.IO.Compression;
using BrotliStream = Brotli.BrotliStream;

namespace TestBrotli
{
    public static class Fixtures
    {
        public const string UncompressedPath = "Resource/BingCN.bin";
        public const string CompressedPath = "Resource/BingCN_Compressed.bin";
    }
    public class BrotliTest
    {
        public static byte[] GetBytes(string path)
        {
            return File.ReadAllBytes(path);
        }

        [Fact]
        public void TestErrorDetection()
        {
            Boolean errorDetected = false;            
            var errorCode = 0;
            using (System.IO.MemoryStream msInvalid = new System.IO.MemoryStream())
            {
                var rawBytes = new Byte[] { 0x1, 0x2, 0x3, 0x4 };
                msInvalid.Write(rawBytes, 0, rawBytes.Length);
                msInvalid.Seek(0, System.IO.SeekOrigin.Begin);

                using (BrotliStream bs = new BrotliStream(msInvalid, System.IO.Compression.CompressionMode.Decompress))
                using (System.IO.MemoryStream msOut = new System.IO.MemoryStream())
                {
                    int bufferSize = 64 * 1024;
                    Byte[] buffer = new Byte[bufferSize];
                    while (true)
                    {
                        try
                        {
                            var cnt = bs.Read(buffer, 0, bufferSize);
                            if (cnt <= 0) break;
                            msOut.Write(buffer, 0, cnt);
                        }
                        catch (BrotliDecodeException bde )
                        {
                            errorDetected = true;
                            errorCode = bde.Code;
                            break;
                        }
                    }
                }
            }
            Assert.True(errorDetected);
            Assert.Equal(2, errorCode);
        }

        [Fact]
        public void TestEmptyStream()
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var brotliStream = new BrotliStream(memoryStream, CompressionMode.Compress, true))
                {
                    brotliStream.Flush();
                }
                memoryStream.Seek(0, SeekOrigin.Begin);
                var data = memoryStream.ToArray();
                using (var bs = new BrotliStream(memoryStream, CompressionMode.Decompress))
                using (var msOutput = new MemoryStream())
                {
                    bs.CopyTo(msOutput); // goes bang
                    var output = msOutput.ToArray();
                    Assert.True(output.Length == 0);
                }
            }
        }

        [Fact]
        public void TestEncode()
        {
            var input = GetBytes(Fixtures.UncompressedPath);
            var output = input.CompressToBrotli(11,22);
            Assert.Equal(GetBytes(Fixtures.CompressedPath), output);
        }

        [Fact]
        public void TestDecode()
        {
            var input = GetBytes(Fixtures.CompressedPath);
            var output = input.DecompressFromBrotli();
            Assert.Equal(GetBytes(Fixtures.UncompressedPath), output);
        }

        [Fact]
        public void TestRoundTrip()
        {
            var input = GetBytes(Fixtures.UncompressedPath);
            var compressed = input.CompressToBrotli(11, 22);
            var decompressed = compressed.DecompressFromBrotli();
            Assert.Equal(input, decompressed);
        }
    }
}
