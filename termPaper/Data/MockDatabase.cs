namespace termPaper.Data
{
    using System;
    using System.Collections.Generic;
    using Document;

    public static class MockDatabase
    {
        public static List<Document> Documents = new()
    {
        new Document
        {
            SiteCode = "001",
            RegisterNumber = "123456",
            RegisterDate = new DateTime(2023, 01, 01),
            Volume = "1",
            Page = "10",
            Book = "A",
            ActType = "TypeA",
            WorthValue = 5000.00,
            WorthValueCurrency = "BGN",
            Fee = 50.00,
            FeeCurrency = "BGN"
        },
        new Document
        {
            SiteCode = "002",
            RegisterNumber = "7891011",
            RegisterDate = new DateTime(2023, 02, 01),
            Volume = "2",
            Page = "20",
            Book = "B",
            ActType = "TypeB",
            WorthValue = 10000.00,
            WorthValueCurrency = "BGN",
            Fee = 100.00,
            FeeCurrency = "BGN"
        }
    };
    }
}
