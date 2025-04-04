using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brotli.NET.Tests.Snapshot
{
    public class BrotliSnapshotting
    {
        private static string ROOT_PATH => CurrentFile.Directory();
        private static string SNAPSHOT_PATH => Path.Combine(ROOT_PATH, "..", "Resource/brotli");

        [Fact]
        public void ValidateBrotli()
        {
            var files = Directory.EnumerateFiles(SNAPSHOT_PATH);
            var uncompressed = files.Where(file => !IsCompressed(file));
            var compressed = files.Where(file => IsCompressed(file));

            foreach (string uncompressFilePath in uncompressed)
            {
                Console.WriteLine(uncompressFilePath);
                Console.WriteLine(Path.GetExtension(uncompressFilePath));
                var compressedFilePath = compressed.FirstOrDefault(file => file.StartsWith(uncompressFilePath));
                if (string.IsNullOrEmpty(compressedFilePath))
                {
                    Console.WriteLine("NO Match");
                    continue;
                }
                    

                var inputBytes = File.ReadAllBytes(uncompressFilePath);
                var expectedBytes = File.ReadAllBytes(compressedFilePath);

                var outputBytes = inputBytes.CompressToBrotli();

                Assert.Equal(expectedBytes, outputBytes);
            }
        }

        private bool IsCompressed(string file)
        {
            return file.EndsWith("compressed");
        }
    }
}
