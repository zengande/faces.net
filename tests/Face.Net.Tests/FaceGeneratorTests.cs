using System;
using System.IO;
using Xunit;

namespace Faces.Net.Tests
{
    public class FaceGeneratorTests
    {
        [Fact]
        public void Test_Generate_Successful()
        {
            var generator = new FaceGenerator();
            var face = generator.Generate();

            File.WriteAllBytes(@"C:\Users\Administrator\Desktop\face.jpg", face);
        }
    }
}
