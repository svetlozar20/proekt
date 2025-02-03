using System;

namespace Document;

public class Document 
{
    public string SiteCode { get; set; }
    public string RegisterNumber { get; set; }
    public DateTime RegisterDate { get; set; }
    public string DoubleRegisterNumber { get; set; }
    public DateTime? DoubleRegisterNumber { get; set; }
    public string Volume {  get; set; }
    public string Page {  get; set; }
    public string Book {  get; set; }
    public string ActType { get; set; }
    public double WorthValue { get; set; }
    public string WorthValueCurrency { get; set; }
    public double? Fee { get; set; }
    public string FeeCurrency { get; set; }
}
