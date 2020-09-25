using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Serialization;
using dotnetCampus.BuildMd5Task;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTest.Extensions.Contracts;

namespace PickTextValueTask.Tests
{
    [TestClass]
    public class BuildMd5TaskTest
    {
        [ContractTestCase]
        public void BuildFolderMd5WithMultiSearchPatternTest()
        {
            "����ͨ���������У�����ͨ������ļ�".Test(() =>
            {
                // ʹ�õ�ǰ�ļ���
                var directory = new DirectoryInfo(".");
                var outputFile = Options.DefaultOutputFileName;

                if (File.Exists(outputFile))
                {
                    File.Delete(outputFile);
                }

                var multiSearchPattern = @"*.dll|*.exe";

                Md5Provider.BuildFolderAllFilesMd5(directory, outputFile, multiSearchPattern);
                // �ȴ�У���ļ�д��
                Thread.Sleep(1000);

                var checksumFile = new FileInfo(outputFile);
                var xmlSerializer = new XmlSerializer(typeof(List<FileMd5Info>));
                using var fileStream = checksumFile.OpenRead();
                var fileMd5InfoList = (List<FileMd5Info>)xmlSerializer.Deserialize(fileStream);

                // Ĭ�ϴ��� exe �� dll �ļ�
                var existExe = fileMd5InfoList.Any(temp =>
                    Path.GetExtension(temp.RelativeFilePath).Equals(".exe", StringComparison.OrdinalIgnoreCase));
                var existDll = fileMd5InfoList.Any(temp =>
                    Path.GetExtension(temp.RelativeFilePath).Equals(".dll", StringComparison.OrdinalIgnoreCase));
                var existPdb = fileMd5InfoList.Any(temp =>
                    Path.GetExtension(temp.RelativeFilePath).Equals(".pdb", StringComparison.OrdinalIgnoreCase));

                // ����ͨ���д�� exe �� dll �ļ��������� pdb �ļ�
                Assert.AreEqual(true, existExe && existDll);
                Assert.AreEqual(false, existPdb);
            });

            "Ĭ�ϲ�����ͨ���������У�������ļ�".Test(() =>
            {
                // ʹ�õ�ǰ�ļ���
                var directory = new DirectoryInfo(".");
                var outputFile = Options.DefaultOutputFileName;

                if (File.Exists(outputFile))
                {
                    File.Delete(outputFile);
                }

                Md5Provider.BuildFolderAllFilesMd5(directory, outputFile);
                // �ȴ�У���ļ�д��
                Thread.Sleep(1000);

                var checksumFile = new FileInfo(outputFile);
                var xmlSerializer = new XmlSerializer(typeof(List<FileMd5Info>));
                using var fileStream = checksumFile.OpenRead();
                var fileMd5InfoList = (List<FileMd5Info>)xmlSerializer.Deserialize(fileStream);

                // Ĭ�ϴ��� exe �� dll �ļ�
                var existExe = fileMd5InfoList.Any(temp =>
                    Path.GetExtension(temp.RelativeFilePath).Equals(".exe", StringComparison.OrdinalIgnoreCase));
                var existDll = fileMd5InfoList.Any(temp =>
                     Path.GetExtension(temp.RelativeFilePath).Equals(".dll", StringComparison.OrdinalIgnoreCase));
                var existPdb = fileMd5InfoList.Any(temp =>
                    Path.GetExtension(temp.RelativeFilePath).Equals(".pdb", StringComparison.OrdinalIgnoreCase));

                Assert.AreEqual(true, existExe && existDll && existPdb);
            });
        }

        [ContractTestCase]
        public void BuildFolderMd5Test()
        {
            "������ļ��е������ļ�����У���ļ�����������У��ɹ�".Test(() =>
            {
                // ʹ�õ�ǰ�ļ���
                var directory = new DirectoryInfo(".");
                var outputFile = Options.DefaultOutputFileName;

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
                Assert.AreEqual(true, verifyResult.AreAllMatched);
            });

            "������ļ��е������ļ�����У���ļ�Ȼ���޸�ĳ���ļ���������������ĵ��ļ�".Test(() =>
            {
                // ʹ�õ�ǰ�ļ���
                var directory = new DirectoryInfo(".");
                var outputFile = Options.DefaultOutputFileName;

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
                Assert.AreEqual(false, verifyResult.AreAllMatched);
                Assert.AreEqual(testFile, verifyResult.NoMatchedFileInfoList[0].RelativeFilePath);
            });

            "������ļ��е������ļ�����У���ļ�Ȼ��ɾ��ĳ���ļ������������ɾ�����ļ�".Test(() =>
            {
                // ʹ�õ�ǰ�ļ���
                var directory = new DirectoryInfo(".");
                var outputFile = Options.DefaultOutputFileName;

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
                Assert.AreEqual(false, verifyResult.AreAllMatched);
                Assert.AreEqual(testFile, verifyResult.NoMatchedFileInfoList[0].RelativeFilePath);
                Assert.AreEqual(true, verifyResult.NoMatchedFileInfoList[0].IsNotFound);
            });
        }
    }
}