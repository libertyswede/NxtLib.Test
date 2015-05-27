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

        internal TaggedDataServiceTest()
        {
            _taggedDataService = TestSettings.ServiceFactory.CreateTaggedDataService();
        }

        internal void RunAllTests()
        {
            UploadTaggedData();
        }

        internal void UploadTaggedData()
        {
            using (Logger = new TestsessionLogger())
            {
                CreateTransactionParameters parameters = CreateTransaction.CreateTransactionByPublicKey();
                if (TestSettings.RunCostlyTests)
                {
                    parameters = CreateTransaction.CreateTransactionBySecretPhrase(true);
                }
                var transaction = _taggedDataService.UploadTaggedData(Name, Data, parameters, Description, Tags, Channel, Type, true,
                        Filename).Result.Transaction;
                var attachment = (TaggedDataUploadAttachment)transaction.Attachment;

                AssertEquals(Name, attachment.Name, "Name");
                AssertEquals(Data, attachment.Data, "Data");
                AssertEquals(Description, attachment.Description, "Description");
                AssertEquals(Tags, attachment.Tags, "Tags");
                AssertEquals(Channel, attachment.Channel, "Channel");
                AssertEquals(Type, attachment.DataType, "DataType");
                AssertIsTrue(attachment.IsText, "IsText");
                AssertEquals(Filename, attachment.Filename, "Filename");
            }
        }

        internal void VerifyTaggedData()
        {
            using (Logger = new TestsessionLogger())
            {
                if (TestSettings.RunCostlyTests)
                {
                    // TODO: Stuff
                }
            }
        }
    }
}
