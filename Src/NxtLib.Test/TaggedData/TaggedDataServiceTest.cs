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
            ExtendTaggedData();
            UploadTaggedData();
            VerifyTaggedData();
        }

        private void ExtendTaggedData()
        {
            using (Logger = new TestsessionLogger())
            {
                var parameters = CreateTransaction.CreateTransactionByPublicKey();

                var transaction = _taggedDataService.ExtendTaggedData(TestSettings.TaggedDataTransactionId, parameters, Name, Data,
                        Description, Tags, Channel, Type, IsText, Filename).Result.Transaction;
                var attachment = (TaggedDataExtendAttachment)transaction.Attachment;

                VerifyMembers(attachment);
                AssertEquals(TestSettings.TaggedDataTransactionId, attachment.TaggedDataId, "TaggedDataId");
            }
        }

        internal void UploadTaggedData()
        {
            using (Logger = new TestsessionLogger())
            {
                var parameters = CreateTransaction.CreateTransactionByPublicKey();

                var transaction = _taggedDataService.UploadTaggedData(Name, Data, parameters, Description, Tags, Channel, Type, IsText,
                        Filename).Result.Transaction;
                var attachment = (TaggedDataUploadAttachment)transaction.Attachment;

                VerifyMembers(attachment);
            }
        }

        internal void VerifyTaggedData()
        {
            using (Logger = new TestsessionLogger())
            {
                var verifyTaggedDataReply = _taggedDataService.VerifyTaggedData(TestSettings.TaggedDataTransactionId, Name, Data,
                        Description, Tags, Channel, Type, IsText, Filename).Result;

                VerifyMembers(verifyTaggedDataReply);
                AssertIsTrue(verifyTaggedDataReply.Verify, "Verify");
            }
        }

        private static void VerifyMembers(TaggedDataAttachment attachment)
        {
            AssertEquals(Name, attachment.Name, "Name");
            AssertEquals(Data, attachment.Data, "Data");
            AssertEquals(Description, attachment.Description, "Description");
            AssertEquals(Tags, attachment.Tags, "Tags");
            AssertEquals(Channel, attachment.Channel, "Channel");
            AssertEquals(Type, attachment.Type, "DataType");
            AssertIsTrue(attachment.IsText, "IsText");
            AssertEquals(Filename, attachment.Filename, "Filename");
        }
    }
}
