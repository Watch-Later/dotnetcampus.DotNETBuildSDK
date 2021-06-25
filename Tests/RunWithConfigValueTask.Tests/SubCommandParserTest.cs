using dotnetCampus.RunWithConfigValueTask;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTest.Extensions.Contracts;

namespace RunWithConfigValueTask.Tests
{
    [TestClass]
    public class SubCommandParserTest
    {
        [ContractTestCase]
        public void ParseCommandlineValue()
        {
            "���������м���� -- �����ݣ����Էָ�������".Test(() =>
            {
                var commandlineArgValue = new string[] { "foo", "lindexi", "--", "f1", "doubi" };
                var (ownerCommand, runningCommand) = SubCommandParser.ParseCommandlineValue(commandlineArgValue);

                Assert.AreEqual(2, ownerCommand.Count);
                Assert.AreEqual(2, runningCommand.Count);
            });

            "��ȡ��ĩβ���� -- �����ݣ�������ȫ��Ϊ������".Test(() =>
            {
                var commandlineArgValue = new string[] { "foo", "lindexi", "--" };
                var (ownerCommand, runningCommand) = SubCommandParser.ParseCommandlineValue(commandlineArgValue);

                Assert.AreEqual(2, ownerCommand.Count);
                Assert.AreEqual(0, runningCommand.Count);
            });

            "��ȡ������ -- �����ݣ�������ȫ��Ϊ����ִ�е�����".Test(() =>
            {
                var commandlineArgValue = new string[] { "foo", "lindexi" };
                var (ownerCommand, runningCommand) = SubCommandParser.ParseCommandlineValue(commandlineArgValue);

                Assert.AreEqual(0, ownerCommand.Count);
                Assert.AreEqual(2, runningCommand.Count);
            });
        }
    }
}
