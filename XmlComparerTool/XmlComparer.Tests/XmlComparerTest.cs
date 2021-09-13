using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTest.Extensions.Contracts;

namespace dotnetCampus.Comparison.Tests
{
    [TestClass]
    public class XmlComparerTest
    {
        [ContractTestCase]
        public void IgnoreElement()
        {
            "���Ϻ��Ե�Ԫ���б����Ժ��Դ�Ԫ�صĲ�ͬ��û���׳���ƥ���쳣".Test(() =>
            {
                var xmlString1 = @"<Foo>
<Id>123123</Id>
<F2>1.123</F2>
<F2>2.123</F2>
<F2>3.123</F2>
<F2>4.123</F2>
<F2>5.123</F2>
<F2>6.123</F2>
</Foo>";
                var xDocument1 = XDocument.Parse(xmlString1);
                var xmlString2 = @"<Foo>
<Id>123</Id>
<F2>1.123</F2>
<F2>2.123</F2>
<F2>3.123</F2>
<F2>4.123</F2>
<F2>5.123</F2>
<F2>6.123</F2>
</Foo>";
                var xDocument2 = XDocument.Parse(xmlString2);

                XmlComparer.VerifyXmlEquals(xDocument1, xDocument2, new XmlComparerSettings()
                {
                    IgnoreElementNameList = new [] { "Id" }
                });
            });
        }

        [ContractTestCase]
        public void VerifyXmlEquals()
        {
            "��������ͬ�б�Ԫ�ص� XML ���ݣ�û���׳���ƥ���쳣".Test(() =>
            {
                var xmlString = @"<Foo>
<F2>1.123</F2>
<F2>2.123</F2>
<F2>3.123</F2>
<F2>4.123</F2>
<F2>5.123</F2>
<F2>6.123</F2>
</Foo>";

                var xmlString1 = xmlString;
                var xDocument1 = XDocument.Parse(xmlString1);
                var xmlString2 = xmlString;
                var xDocument2 = XDocument.Parse(xmlString2);

                XmlComparer.VerifyXmlEquals(xDocument1, xDocument2);
            });

            "����һ��Ƕ�������һ��Ƕ��һ��� XML ���ݣ��׳��쳣".Test(() =>
            {
                var xmlString1 = @"<Foo><F2>1.123</F2></Foo>";
                var xDocument1 = XDocument.Parse(xmlString1);
                var xmlString2 = @"<Foo>1.12301</Foo>";
                var xDocument2 = XDocument.Parse(xmlString2);

                Assert.ThrowsException<ElementNoMatchException>(() =>
                {
                    XmlComparer.VerifyXmlEquals(xDocument1, xDocument2);
                });
            });

            "����Ƕ�׶���������ͬ�� XML ���ݣ�û���׳���ƥ���쳣".Test(() =>
            {
                var xmlString = @"<Foo><F2>1.123</F2></Foo>";
                var xmlString1 = xmlString;
                var xDocument1 = XDocument.Parse(xmlString1);
                var xmlString2 = xmlString;
                var xDocument2 = XDocument.Parse(xmlString2);

                XmlComparer.VerifyXmlEquals(xDocument1, xDocument2);
            });

            "���븡��ֵ�����ھ�����û���׳���ƥ���쳣".Test(() =>
            {
                var xmlString1 = @"<Foo>1.123</Foo>";
                var xDocument1 = XDocument.Parse(xmlString1);
                var xmlString2 = @"<Foo>1.12301</Foo>";
                var xDocument2 = XDocument.Parse(xmlString2);

                XmlComparer.VerifyXmlEquals(xDocument1, xDocument2);
            });

            "����ֵ����ͬ��XML���ݣ��׳��쳣".Test(() =>
            {
                var xmlString1 = @"<Foo>Foo</Foo>";
                var xDocument1 = XDocument.Parse(xmlString1);
                var xmlString2 = @"<Foo>F1</Foo>";
                var xDocument2 = XDocument.Parse(xmlString2);

                Assert.ThrowsException<ElementNoMatchException>(() =>
                {
                    XmlComparer.VerifyXmlEquals(xDocument1, xDocument2);
                });
            });

            "����������ȫ��ͬ��XML���ݣ�û���׳���ƥ���쳣".Test(() =>
            {
                var xmlString = @"<Foo>Foo</Foo>";
                var xDocument1 = XDocument.Parse(xmlString);
                var xDocument2 = XDocument.Parse(xmlString);

                XmlComparer.VerifyXmlEquals(xDocument1, xDocument2);
            });
        }
    }
}
