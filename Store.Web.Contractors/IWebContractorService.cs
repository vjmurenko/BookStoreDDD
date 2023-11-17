namespace Store.Web.Contractors;

public interface IWebContractorService
{
    public string Name { get; }
    Uri StartSession(IReadOnlyDictionary<string, string> parameters, Uri returnUri);
}