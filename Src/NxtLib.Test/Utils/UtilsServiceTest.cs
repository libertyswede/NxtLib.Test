﻿using NxtLib.Utils;

namespace NxtLib.Test.Utils
{
    class UtilsServiceTest : TestBase
    {
        private readonly IUtilService _service;
        private const string QrCodeBase64 = "/9j/4AAQSkZJRgABAgAAAQABAAD/2wBDAAgGBgcGBQgHBwcJCQgKDBQNDAsLDBkSEw8UHRofHh0aHBwgJC4nICIsIxwcKDcpLDAxNDQ0Hyc5PTgyPC4zNDL/2wBDAQkJCQwLDBgNDRgyIRwhMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjL/wAARCAAZABkDASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwCPw94evYdS0l30aNIETShFCvhyWK4guEltTO7zm3UYwk5JMhBDflH4gXTjf6PZS+IYLOY3f2tprqJvKgUzxnyxJJKs6SI25XliSNpWDeaVaOVkv2Ful1f6LpQl26ha2m4wylbc38sk6q5jaFjIixm2Cqptw0CQjPlMqNGeJ7xLrxC2jahoe6G80+WG0mmulkOoTh4lgtZJDI0i+VMoSRYpfmkzIwVSxoAL3SIrrxBZXmn2MFxpBu7aaWa58Mvcm8sfsloEEbJasqfdmG1dmCeg7eMf8IJ4w/6FTXP/AAXTf/E17n4k10ta6vc3ksltpiaPE2kTzeZdJfafPLAtwCdqvvKrCu13DhpmJk2lXTrP+Ev/AOqj+Bv/AAH/APuygDg9Jl1G81LQ7m90u7ZLW30iS31OfTrP7OwlltgYYiLUFAvnybdsmQU+tULW90CKLVdA1qLWZotWedrkW8zTOsyTu0NpE0vzyPm4ikKhRIsjfvchpFj8r8Cf8lD8Nf8AYVtf/Rq16B8F/wDkB+IP+wroX/paKAJJItVt3Tw/oEWm6J4gsdHkXU5YmkgljWNbZzumWCNVLGEkO0jofNceYAyZ2/7R+EH/AENH/luWn/yDWBrH/JQ/hL/2CtH/APRprx+gD//Z";
        private const string NxtAddress = "NXT-HMVV-XMBN-GYXK-22BKK";

        internal UtilsServiceTest()
        {
            _service = TestSettings.ServiceFactory.CreateUtilService();
        }

        internal void RunAllTests()
        {
            TestDecodeQrCode();
            TestEncodeQrCode();
        }

        private void TestEncodeQrCode()
        {
            using (Logger = new TestsessionLogger())
            {
                var encodeQrCodeResult = _service.EncodeQrCode(NxtAddress).Result;
                if (encodeQrCodeResult.QrCodeBase64 != QrCodeBase64)
                {
                    Logger.Fail($"Expected: {QrCodeBase64}, but got {encodeQrCodeResult.QrCodeBase64}");
                }
            }
        }

        private void TestDecodeQrCode()
        {
            using (Logger = new TestsessionLogger())
            {
                var decodeQrCodeResult = _service.DecodeQrCode(QrCodeBase64).Result;
                if (decodeQrCodeResult.QrCodeData != NxtAddress)
                {
                    Logger.Fail($"Expected: {NxtAddress}, but got {decodeQrCodeResult.QrCodeData}");
                }
            }
        }
    }
}
