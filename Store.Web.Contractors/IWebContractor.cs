namespace Store.Web.Contractors;

public interface IWebContractor
{
    public string UniqueCode { get; }
    public string GetUri { get; }
}