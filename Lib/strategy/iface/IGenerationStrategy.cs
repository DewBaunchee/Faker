namespace Lib.Strategy.IFace
{
    public interface IGenerationStrategy
    {
        object Generate();
        bool IsDefaultValue(object value);
    }
}