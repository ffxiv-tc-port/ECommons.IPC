using ECommons.DalamudServices;
using System;

namespace ECommons.IPC.Subscribers.AutoDuty;

using EzIpcManager;
using static AutoDutyIPC.Delegates;

public sealed class AutoDutyIPC : IPCBase
{
    public AutoDutyIPC()
    {
    }

    public AutoDutyIPC(SafeWrapper wrapper) : base(wrapper)
    {
    }

    public override string InternalName { get; } = "AutoDuty";

    public static class Delegates
    {
        public delegate void RunDelegate(uint territoryType, int loops = 0, bool bareMode = false);
    }

    /**
     * @param config The name of the config to get.
     */
    [EzIPC("GetConfig")] public Func<string, string> GetConfig { get; private set; }

    /*
     * AutoDuty 提供端的真實簽章是 void SetConfig(string config, string setting) —— 第二個參數是
     * 單一 string,不是 object。宣告成 Action<string, object> 的話,傳 string[] 會在
     * CallGateChannel 走進 ConvertObject(string[] -> string) 而擲 IpcTypeMismatchError。
     */
    [EzIPC("SetConfig")] private Action<string, string> SetConfig { get; set; }

    public void SetConfigValue(string config, string value) =>
        this.SetConfig(config, value);

    /**
     * AutoDuty 的 SetConfig 端點每次只轉發一個值給 ConfigHelper.ModifyConfig,
     * 也就是 configValues 永遠只有一個元素。清單類設定要用的 set/add/del/insert 等子指令
     * 需要多個 token,只有聊天指令路徑(/ad cfg ...)才送得進去,IPC 無法表達。
     * 因此:單一元素照常送出(等同上面的單值多載);多元素則記錄錯誤後不送出,
     * 以免在 Dalamud 內部擲出難以追查的 IpcTypeMismatchError。
     */
    public void SetConfigValue(string config, string[] values)
    {
        if(values == null || values.Length == 0)
        {
            Svc.Log.Error($"[AutoDutyIPC] SetConfigValue(\"{config}\") 沒有收到任何值,已略過。");
            return;
        }
        if(values.Length > 1)
        {
            Svc.Log.Error($"[AutoDutyIPC] AutoDuty 的 SetConfig 每次只接受一個值,設定 \"{config}\" 收到 {values.Length} 個,已略過。清單類設定請改用 AutoDuty 的聊天指令 /ad cfg。");
            return;
        }
        this.SetConfig(config, values[0]);
    }

    /**
     * @param territoryType The territory type ID to run the path in. 0 to use current territory.<br/>
     * @param loops Number of loops to run. Use 0 to use the current loops already set.<br/>
     * @param bareMode Only run the dungeon and skip any pre-, between-, and post-dungeon actions.
     */
    [EzIPC("Run")] public RunDelegate Run { get; private set; }
    /**
     * Starts navigating the current path.<br/>
     * @param startFromZero Whether to start the path from the beginning or stay at current index.
     */
    [EzIPC("Start")] public Action<bool> Start { get; private set; }
    [EzIPC("Stop")] public Action Stop { get; private set; }
    [EzIPC("IsNavigating")] public Func<bool> IsNavigating { get; private set; }
    [EzIPC("IsLooping")] public Func<bool> IsLooping { get; private set; }
    [EzIPC("IsStopped")] public Func<bool> IsStopped { get; private set; }
    /**
     * @param territoryType The territory type ID to check.
     */
    [EzIPC("ContentHasPath")] public Func<uint, bool> ContentHasPath { get; private set; }
}