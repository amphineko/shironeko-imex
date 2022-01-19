namespace Shironeko.Core.Extensions;

public interface IExtensionRegistry
{
    public Type? GetByGuid(Guid guid);

    public Type? GetByName(string name);
}