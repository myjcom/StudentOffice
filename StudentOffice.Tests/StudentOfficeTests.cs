using NUnit.Framework;
using StudentOffice;
using System;

namespace StudentOffice.Tests
{
    [TestFixture]
    public class StudentOfficeTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void IsValidDateTest()
        {
            string validDate = DateTime.Now.Date.ToString();
            string invalidDateZ = "11.11.1111";
            string invalidDateO = "00.00.000";
            string invalidDateV = "29.02.2022";

            Assert.IsTrue(Utils.Checker.IsValidDate(validDate));
            Assert.IsFalse(Utils.Checker.IsValidDate(invalidDateZ));
            Assert.IsFalse(Utils.Checker.IsValidDate(invalidDateO));
            Assert.IsFalse(Utils.Checker.IsValidDate(invalidDateV));
        }

        [Test]
        public void IsValidEmailTest()
        {
            string invalidEmailZ = "voz@@voz.com";
            string validEmailZ = "mail@mail.com";

            Assert.IsFalse(Utils.Checker.IsValidEmail(invalidEmailZ));
            Assert.IsTrue(Utils.Checker.IsValidEmail(validEmailZ));
        }

        [Test]
        public void IsValidName()
        {
            string validName = "Евгений";
            string invalidName = "";
            string invalidNameZ = " Eвгений!";
            Assert.IsTrue(NewStudent.ValidateValue(validName, Utils.Checker.IsValidName));
            Assert.IsFalse(NewStudent.ValidateValue(invalidName, Utils.Checker.IsValidName));
            Assert.IsFalse(NewStudent.ValidateValue(invalidNameZ, Utils.Checker.IsValidName));
        }
        [Test]
        public void IsValidNumber()
        {
            string validNum = "1234";
            string validNumZ = "123456";
            string invalidNum = "";
            string invalidNumZ = "1234567";

            Assert.IsTrue(NewStudent.ValidateValue(validNum, Utils.Checker.IsValidNumber));
            Assert.IsTrue(NewStudent.ValidateValue(validNumZ, Utils.Checker.IsValidNumber));
            Assert.IsFalse(NewStudent.ValidateValue(invalidNum, Utils.Checker.IsValidNumber));
            Assert.IsFalse(NewStudent.ValidateValue(invalidNumZ, Utils.Checker.IsValidNumber));
        }

        [Test]
        public void IsValidPhone()
        {
            string validPhone = "+7(911)123-45-67";
            string invalidPhone = "123456";
            Assert.IsTrue(NewStudent.ValidateValue(validPhone, Utils.Checker.IsValidPhone));
            Assert.IsFalse(NewStudent.ValidateValue(invalidPhone, Utils.Checker.IsValidPhone));
        }

    }
}