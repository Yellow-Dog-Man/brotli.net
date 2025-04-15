namespace Brotli.NET.Tests
{
    public class BasicCompressionTest
    {
        [Fact]
        public void TestCreateInstance()
        {
            var input = new byte[] { 1, 2, 3, 4, 1, 2, 3, 4 };
            Byte[] output = input.CompressToBrotli();
            Assert.Equal(new byte[] { 0x8b, 0x03, 0x80, 1, 2, 3, 4, 1, 2, 3, 4, 0x3 }, output);
        }
    }
}
