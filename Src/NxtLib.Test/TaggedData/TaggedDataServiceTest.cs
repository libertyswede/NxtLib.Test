using NxtLib.TaggedData;

namespace NxtLib.Test.TaggedData
{
    class TaggedDataServiceTest : TestBase
    {
        private readonly ITaggedDataService _taggedDataService;
        const string Name = "testname";
        const string Data = "abc123";
        const string Description = "description";
        const string Tags = "tag1,tag2";
        const string Channel = "channel?";
        const string Type = "type?";
        const string Filename = "test.txt";
        const bool IsText = true;

        internal TaggedDataServiceTest()
        {
            _taggedDataService = TestSettings.ServiceFactory.CreateTaggedDataService();
        }

        internal void RunAllTests()
        {
            TestGetTaggedDataExtendTransactions();
            TestExtendTaggedData();
            TestUploadTaggedData();
            TestVerifyTaggedData();
        }
        
        private void TestGetTaggedDataExtendTransactions()
        {
            using (Logger = new TestsessionLogger())
            {
                var result = _taggedDataService.GetTaggedDataExtendTransactions(8870885735414850668).Result;
                // Verify result
            }
        }

        private void TestExtendTaggedData()

        {
            using (Logger = new TestsessionLogger())
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
            using (Logger = new TestsessionLogger())
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
            using (Logger = new TestsessionLogger())
            {
                var verifyTaggedDataReply = _taggedDataService.VerifyTaggedData(TestSettings.TaggedDataTransactionId, Name, Data, null,
                        Description, Tags, Channel, Type, IsText, Filename).Result;

                VerifyMembers(verifyTaggedDataReply);
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
