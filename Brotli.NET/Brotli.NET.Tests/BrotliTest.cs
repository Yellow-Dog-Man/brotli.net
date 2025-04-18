﻿using System.IO.Compression;

namespace Brotli.NET.Tests
{
    public class BrotliTest
    {
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
    }
}
