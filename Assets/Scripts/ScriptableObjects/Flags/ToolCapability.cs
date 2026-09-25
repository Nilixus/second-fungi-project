using System;

namespace Inventory.Item.Flags
{
    [Flags]
    public enum ToolCapability
    {
        Hand   = 0,
        Till   = 1,
        Loosen = 2,
        Water  = 3,
        Chop   = 4
    }
}