using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using DelphiProjectHandler.Model;
using DelphiProjectHandler.Operations;
using System.IO;

namespace DelphiProjectHandler.Tests
{
    [TestFixture]
    public class BulkOperationLoaderTests
    {
        protected const string cXml =
            @"<?xml version=""1.0"" encoding=""UTF-8""?>" +
            @"<projects>" +
            @"    <project name=""c:\source\fpm\dpr\Murte2mm.dpr"">" +
            @"        <remove>" +
            @"            <file>c:\source\fpm\u\Utils\RteSegsUtils.pas</file>" +
            @"            <file>c:\source\fpm\u\Utils\NextDataRefreshTimeState.pas</file>" +
            @"        </remove>" +
            @"        <add>" +
            @"            <file>c:\source\fpm\u\Utils\AbsoluteIdUtils.pas</file>" +
            @"            <file>c:\source\fpm\u\Utils\CycleDateState.pas</file>" +
            @"        </add>" +
            @"    </project>" +
            @"    <project name=""c:\source\fpm\dpr\Code.dpr"">" +
            @"        <remove>" +
            @"            <file>c:\source\fpm\u\Utils\AbsoluteIdUtils.pas</file>" +
            @"        </remove>" +
            @"        <add>" +
            @"            <file>c:\source\fpm\u\IO\CodeTableInterface.pas</file>" +
            @"            <file>c:\source\fpm\u\IO\CodeTable.pas</file>" +
            @"        </add>" +
            @"    </project>" +
            @"</projects>";


        [Test]
        public void Load()
        {
            Stream vMemoryStream = new MemoryStream();
            TextWriter vWriter = new StreamWriter(vMemoryStream);
            vWriter.Write(cXml);
            vWriter.Flush();
            vMemoryStream.Position = 0;
            ProjectBulkOperations vOperations = ProjectBulkOperationLoader.FromStream(vMemoryStream);
            Assert.AreEqual(2, vOperations.Count, "Invalid no. of projects read");
            Assert.AreEqual(2, vOperations[0].Add.Count, "Invalid no. of files to add - Project 0");
            Assert.AreEqual(2, vOperations[1].Add.Count, "Invalid no. of files to add - Project 1");
            Assert.AreEqual(2, vOperations[0].Remove.Count, "Invalid no. of files to remove - Project 0");
            Assert.AreEqual(1, vOperations[1].Remove.Count, "Invalid no. of files to remove - Project 1");
            Assert.AreEqual(@"c:\source\fpm\u\Utils\RteSegsUtils.pas", vOperations[0].Remove[0], "Wrong file to remove. Project 0, Item 0");
            Assert.AreEqual(@"c:\source\fpm\u\Utils\NextDataRefreshTimeState.pas", vOperations[0].Remove[1], "Wrong file to remove. Project 0, Item 1");
            Assert.AreEqual(@"c:\source\fpm\u\Utils\AbsoluteIdUtils.pas", vOperations[0].Add[0], "Wrong file to add. Project 0, Item 0");
            Assert.AreEqual(@"c:\source\fpm\u\Utils\CycleDateState.pas", vOperations[0].Add[1], "Wrong file to add. Project 0, Item 1");
            Assert.AreEqual(@"c:\source\fpm\u\Utils\AbsoluteIdUtils.pas", vOperations[1].Remove[0], "Wrong file to remove. Project 1, Item 0");
            Assert.AreEqual(@"c:\source\fpm\u\IO\CodeTableInterface.pas", vOperations[1].Add[0], "Wrong file to add. Project 1, Item 0");
            Assert.AreEqual(@"c:\source\fpm\u\IO\CodeTable.pas", vOperations[1].Add[1], "Wrong file to add. Project 1, Item 1");
        }
    }
}
