using System.Collections;
using System.Collections.Generic;
using System.Linq;

using dotnetCampus.MatrixRun;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MSTest.Extensions.Contracts;

namespace DotNETBuild.Tests
{
    [TestClass]
    public class CartesianProductTests
    {
        [ContractTestCase]
        public void EnumerateLists()
        {
            const int A = 101, B = 102, C = 103;
            const int x = 201, y = 202, z = 203;

            "����ע�����Ƶ����ӣ����Է���ע�����Ƶķ��ء�".Test(() =>
            {
                var lists = new List<int[]>
                {
                    new []{ A, B, C }, new []{ 1, 2 }, new []{ x, y, z },
                };
                var expectedResult = new List<int[]>
                {
                    new []{ A, 1, x }, new []{ A, 1, y }, new []{ A, 1, z },
                    new []{ A, 2, x }, new []{ A, 2, y }, new []{ A, 2, z },
                    new []{ B, 1, x }, new []{ B, 1, y }, new []{ B, 1, z },
                    new []{ B, 2, x }, new []{ B, 2, y }, new []{ B, 2, z },
                    new []{ C, 1, x }, new []{ C, 1, y }, new []{ C, 1, z },
                    new []{ C, 2, x }, new []{ C, 2, y }, new []{ C, 2, z },
                };

                var result = CartesianProduct.Enumerate(lists).ToList();
                Assert.AreEqual(expectedResult.Count, result.Count);
                for (var i = 0; i < expectedResult.Count; i++)
                {
                    var expectedList = expectedResult[i];
                    var list = result[i];
                    CollectionAssert.AreEqual(expectedList, (ICollection)list);
                }
            });
        }

        [ContractTestCase]
        public void EnumerateDictionary()
        {
            const int A = 101, B = 102, C = 103;
            const int x = 201, y = 202, z = 203;

            "����ע�����Ƶ����ӣ����Է���ע�����Ƶķ��ء�".Test(() =>
            {
                var dictionary = new Dictionary<string, IReadOnlyList<int>>
                {
                    { "��", new []{ A, B, C } },
                    { "��", new []{ 1, 2 } },
                    { "��", new []{ x, y, z } },
                };
                var expectedResult = new List<Dictionary<string, int>>
                {
                    new Dictionary<string, int>{ { "��", A }, { "��", 1 }, { "��", x } },
                    new Dictionary<string, int>{ { "��", A }, { "��", 1 }, { "��", y } },
                    new Dictionary<string, int>{ { "��", A }, { "��", 1 }, { "��", z } },
                    new Dictionary<string, int>{ { "��", A }, { "��", 2 }, { "��", x } },
                    new Dictionary<string, int>{ { "��", A }, { "��", 2 }, { "��", y } },
                    new Dictionary<string, int>{ { "��", A }, { "��", 2 }, { "��", z } },
                    new Dictionary<string, int>{ { "��", B }, { "��", 1 }, { "��", x } },
                    new Dictionary<string, int>{ { "��", B }, { "��", 1 }, { "��", y } },
                    new Dictionary<string, int>{ { "��", B }, { "��", 1 }, { "��", z } },
                    new Dictionary<string, int>{ { "��", B }, { "��", 2 }, { "��", x } },
                    new Dictionary<string, int>{ { "��", B }, { "��", 2 }, { "��", y } },
                    new Dictionary<string, int>{ { "��", B }, { "��", 2 }, { "��", z } },
                    new Dictionary<string, int>{ { "��", C }, { "��", 1 }, { "��", x } },
                    new Dictionary<string, int>{ { "��", C }, { "��", 1 }, { "��", y } },
                    new Dictionary<string, int>{ { "��", C }, { "��", 1 }, { "��", z } },
                    new Dictionary<string, int>{ { "��", C }, { "��", 2 }, { "��", x } },
                    new Dictionary<string, int>{ { "��", C }, { "��", 2 }, { "��", y } },
                    new Dictionary<string, int>{ { "��", C }, { "��", 2 }, { "��", z } },
                };

                var result = CartesianProduct.Enumerate(dictionary).ToList();
                Assert.AreEqual(expectedResult.Count, result.Count);
                for (var i = 0; i < expectedResult.Count; i++)
                {
                    var expectedDictionary = expectedResult[i];
                    var resultDictionary = result[i];
                    CollectionAssert.AreEqual(expectedDictionary, (ICollection)resultDictionary);
                }
            });
        }
    }
}
