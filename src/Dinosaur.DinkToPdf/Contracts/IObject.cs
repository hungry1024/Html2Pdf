namespace Dinosaur.DinkToPdf.Contracts
{
    public interface IObject : ISettings
    {
        byte[] GetContent();
    }
}
