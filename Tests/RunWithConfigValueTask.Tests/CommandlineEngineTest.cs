using System;
using dotnetCampus.Configurations;
using dotnetCampus.Configurations.Core;
using dotnetCampus.RunWithConfigValueTask;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTest.Extensions.Contracts;

namespace RunWithConfigValueTask.Tests
{
    [TestClass]
    public class CommandlineEngineTest
    {
        [ContractTestCase]
        public void FillCommandline()
        {
            "��������ð���Ĭ��ֵ�����������ò�����ʱʹ��Ĭ��ֵ".Test(() =>
            {
                var commandline = new string[] { "lindexi", "$(��������)??F1", "$(����������)??F2" };
                var memoryConfigurationRepo = new MemoryConfigurationRepo();
                var defaultConfiguration = memoryConfigurationRepo.CreateAppConfigurator().Default;
                defaultConfiguration["��������"] = "doubi";

                var result = CommandlineEngine.FillCommandline(commandline, defaultConfiguration);

                var expectedResult = new string[] { "lindexi", "doubi", "F2" };

                Assert.AreEqual(3, result.Length);

                for (var i = 0; i < result.Length; i++)
                {
                    Assert.AreEqual(expectedResult[i], result[i]);
                }
            });

            "��������������ڵ����ݣ������滻Ϊ�������������".Test(() =>
            {
                var commandline = new string[] { "lindexi", "$(Foo)" };

                var memoryConfigurationRepo = new MemoryConfigurationRepo();
                var defaultConfiguration = memoryConfigurationRepo.CreateAppConfigurator().Default;
                defaultConfiguration["Foo"] = "doubi";

                var result = CommandlineEngine.FillCommandline(commandline, defaultConfiguration);

                var expectedResult = new string[] { "lindexi", "doubi" };

                Assert.AreEqual(2, result.Length);

                for (var i = 0; i < result.Length; i++)
                {
                    Assert.AreEqual(expectedResult[i], result[i]);
                }
            });

            "�����������治���ڵ����ݣ������׳�����".Test(() =>
            {
                var commandline = new string[] { "lindexi", "$(Foo)" };

                Assert.ThrowsException<ArgumentException>(() =>
                {
                    var memoryConfigurationRepo = new MemoryConfigurationRepo();
                    var defaultConfiguration = memoryConfigurationRepo.CreateAppConfigurator().Default;
                    CommandlineEngine.FillCommandline(commandline, defaultConfiguration);
                });
            });

            "���벻������Ҫ�滻����������ԭ�е�������ͬ".Test(() =>
            {
                var commandline = new string[] { "lindexi", "foo" };

                var result = CommandlineEngine.FillCommandline(commandline, new DefaultConfiguration());

                Assert.AreEqual(2, result.Length);

                for (var i = 0; i < result.Length; i++)
                {
                    Assert.AreEqual(commandline[i], result[i]);
                }
            });
        }
    }
}