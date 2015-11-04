using Microsoft.Framework.Logging;
using NxtLib.TaggedData;

namespace NxtLib.Test.TaggedData
{
    public interface ITaggedDataServiceTest : ITest
    {
    }

    public class TaggedDataServiceTest : TestBase, ITaggedDataServiceTest
    {
        private readonly ITaggedDataService _taggedDataService;
        private readonly ILogger _logger;
        const string Name = "testname";
        const string Data = "abc123";
        const string Description = "description";
        const string Tags = "tag1,tag2";
        const string Channel = "channel?";
        const string Type = "type?";
        const string Filename = "test.txt";
        const bool IsText = true;

        public TaggedDataServiceTest(ITaggedDataService taggedDataService, ILogger logger)
        {
            _taggedDataService = taggedDataService;
            _logger = logger;
        }

        public void RunAllTests()
        {
            TestGetTaggedDataExtendTransactions();
            TestExtendTaggedData();
            TestUploadTaggedData();
            TestVerifyTaggedData();
        }
        
        private void TestGetTaggedDataExtendTransactions()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var result = _taggedDataService.GetTaggedDataExtendTransactions(8870885735414850668).Result;
                // Verify result
            }
        }

        private void TestExtendTaggedData()

        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var parameters = CreateTransaction.CreateTransactionByPublicKey();

                var transaction = _taggedDataService.ExtendTaggedData(TestSettings.TaggedDataTransactionId, parameters, Name, Data, null,
                        Description, Tags, Channel, Type, IsText, Filename).Result.Transaction;
                var attachment = (TaggedDataExtendAttachment)transaction.Attachment;

                VerifyMembers(attachment);
                AssertEquals(TestSettings.TaggedDataTransactionId, attachment.TaggedDataId, "TaggedDataId");
            }
        }

        internal void TestUploadTaggedData()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var parameters = CreateTransaction.CreateTransactionByPublicKey();

                var transaction = _taggedDataService.UploadTaggedData(Name, Data, parameters, null, Description, Tags, Channel, Type, IsText,
                        Filename).Result.Transaction;
                var attachment = (TaggedDataUploadAttachment)transaction.Attachment;

                VerifyMembers(attachment);
            }
        }

        internal void TestVerifyTaggedData()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var verifyTaggedDataReply = _taggedDataService.VerifyTaggedData(TestSettings.TaggedDataTransactionId, Name, Data, null,
                        Description, Tags, Channel, Type, IsText, Filename).Result;
                
                AssertIsTrue(verifyTaggedDataReply.Verify, "Verify");
            }
        }

        private static void VerifyMembers(ITaggedData taggedData)
        {
            AssertEquals(Name, taggedData.Name, "Name");
            AssertEquals(Data, taggedData.Data, "Data");
            AssertEquals(Description, taggedData.Description, "Description");
            AssertEquals(Tags, taggedData.Tags, "Tags");
            AssertEquals(Channel, taggedData.Channel, "Channel");
            AssertEquals(Type, taggedData.Type, "DataType");
            AssertIsTrue(taggedData.IsText, "IsText");
            AssertEquals(Filename, taggedData.Filename, "Filename");
        }
    }
}
