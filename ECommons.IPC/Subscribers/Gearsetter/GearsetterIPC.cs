using System;
using System.Collections.Generic;

namespace ECommons.IPC.Subscribers.Gearsetter
{
    using EzIpcManager;
    using FFXIVClientStructs.FFXIV.Client.Game;
    using FFXIVClientStructs.FFXIV.Client.UI.Misc;

    public sealed class GearsetterIPC : IPCBase
    {
        public GearsetterIPC()
        {
        }

        public GearsetterIPC(SafeWrapper wrapper) : base(wrapper)
        {
        }

        public override string InternalName { get; } = "Gearsetter";

        /*
         * Gearsetter 提供端 (Gearsetter/External/GearsetterIpc.cs) 回傳的 tuple 第三元是 int?,這裡要逐字相同。
         * 宣告成 byte? 是窄化:值不超過 255 時靜默成功,超過 255 才在 CallGateChannel 的 JSON 轉換擲
         * IpcTypeMismatchError —— 而 SafeWrapper.IPCException 只攔 IpcNotReadyError,攔不住它。
         */
        [EzIPC] public Func<byte, List<(uint ItemId, InventoryType? SourceInventory, int? SourceInventorySlot, RaptureGearsetModule.GearsetItemIndex TargetSlot)>> GetRecommendationsForGearset { get; private set; }
    }
}
