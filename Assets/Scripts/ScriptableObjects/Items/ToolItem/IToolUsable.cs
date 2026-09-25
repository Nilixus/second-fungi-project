using Inventory.Item.Flags;

namespace Inventory.Item.ToolItems
{
    public interface IToolUsable
    { 
        ToolCapability Capability { get; }
        int Volume { get; }
    }
}