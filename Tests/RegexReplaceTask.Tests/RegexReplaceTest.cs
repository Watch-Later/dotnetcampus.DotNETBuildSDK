using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MSTest.Extensions.Contracts;

namespace RegexReplaceTask.Tests
{
    [TestClass]
    public class RegexReplaceTest
    {
        [ContractTestCase]
        public void RegexReplace()
        {
            "�����ļ��������滻���ݣ����Խ����ݽ����滻".Test(() =>
            {
                var content = "1.2.$GitRevision$";
                var file = Path.GetTempFileName();
                File.WriteAllText(file, content);

                var argList = new[]
                {
                    "-v",
                    "Foo",
                    "-r",
                    @"\d+\.\d+\.(\$GitRevision\$)",
                    "-f",
                    file
                };

                dotnetCampus.RegexReplaceTask.Program.Main(argList);

                content = File.ReadAllText(file);
                Assert.AreEqual("1.2.Foo", content);
            });
        }
    }
}
