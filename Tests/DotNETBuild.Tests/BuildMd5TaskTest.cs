using System.IO;
using System.Threading;
using dotnetCampus.BuildMd5Task;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTest.Extensions.Contracts;

namespace PickTextValueTask.Tests
{
    [TestClass]
    public class BuildMd5TaskTest
    {
        [ContractTestCase]
        public void BuildFolderMd5Test()
        {
            "������ļ��е������ļ�����У���ļ�����������У��ɹ�".Test(() =>
            {
                // ʹ�õ�ǰ�ļ���
                var directory = new DirectoryInfo(".");
                var outputFile = Options.DefaultOutputFile;

                if (File.Exists(outputFile))
                {
                    File.Delete(outputFile);
                }

                Md5Provider.BuildFolderAllFilesMd5(directory, outputFile);

                // �ȴ�У���ļ�д��
                Thread.Sleep(1000);

                // ��ȡУ���ļ���Ȼ������Ƿ��ļ���ȫ��ͬ
                var verifyResult = Md5Provider.VerifyFolderMd5(directory, new FileInfo(outputFile));
                // Ԥ�������ж���ͬ
                Assert.AreEqual(true, verifyResult.IsAllMatch);
            });

            "������ļ��е������ļ�����У���ļ�Ȼ���޸�ĳ���ļ���������������ĵ��ļ�".Test(() =>
            {
                // ʹ�õ�ǰ�ļ���
                var directory = new DirectoryInfo(".");
                var outputFile = Options.DefaultOutputFile;

                if (File.Exists(outputFile))
                {
                    File.Delete(outputFile);
                }

                // ����һ���������ĵ��ļ�
                var testFile = "test.txt";
                File.WriteAllText(testFile, "����");

                Md5Provider.BuildFolderAllFilesMd5(directory, outputFile);

                // �޸Ĳ����ļ�
                File.WriteAllText(testFile, "�ֵ����Ƕ���");

                // �ȴ�У���ļ�д��
                Thread.Sleep(1000);

                // ��ȡУ���ļ���Ȼ������Ƿ��ļ���ȫ��ͬ
                var verifyResult = Md5Provider.VerifyFolderMd5(directory, new FileInfo(outputFile));

                // Ԥ�����ҵ��޸ĵ��ļ�
                Assert.AreEqual(false, verifyResult.IsAllMatch);
                Assert.AreEqual(testFile, verifyResult.NoMatchFileInfoList[0].File);
            });

            "������ļ��е������ļ�����У���ļ�Ȼ��ɾ��ĳ���ļ������������ɾ�����ļ�".Test(() =>
            {
                // ʹ�õ�ǰ�ļ���
                var directory = new DirectoryInfo(".");
                var outputFile = Options.DefaultOutputFile;

                if (File.Exists(outputFile))
                {
                    File.Delete(outputFile);
                }

                // ����һ���������ĵ��ļ�
                var testFile = "test.txt";
                File.WriteAllText(testFile, "����");

                Md5Provider.BuildFolderAllFilesMd5(directory, outputFile);

                // ɾ�������ļ�
                File.Delete(testFile);

                // �ȴ�У���ļ�д��
                Thread.Sleep(1000);

                // ��ȡУ���ļ���Ȼ������Ƿ��ļ���ȫ��ͬ
                var verifyResult = Md5Provider.VerifyFolderMd5(directory, new FileInfo(outputFile));

                // Ԥ�����ҵ��޸ĵ��ļ�
                Assert.AreEqual(false, verifyResult.IsAllMatch);
                Assert.AreEqual(testFile, verifyResult.NoMatchFileInfoList[0].File);
                Assert.AreEqual(true, verifyResult.NoMatchFileInfoList[0].IsNotFound);
            });
        }
    }
}