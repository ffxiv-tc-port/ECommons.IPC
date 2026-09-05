﻿using ECommons.EzIpcManager;
using System;
using static ECommons.IPC.Subscribers.AutoRetainer.AutoRetainerIPC.Delegates;

namespace ECommons.IPC.Subscribers.AutoRetainer;

public sealed class AutoRetainerIPC : IPCBase
{
    public AutoRetainerIPC()
    {
    }

    public AutoRetainerIPC(SafeWrapper wrapper) : base(wrapper)
    {
    }

    public static class Delegates
    {
        public delegate bool? AreAnyEnabledVesselsNotDeployed(ulong contentId);
        public delegate bool? AreAnyEnabledVesselsReady(ulong contentId);
    }

    public override string InternalName { get; } = "AutoRetainer";

    [EzIPC("AutoRetainer.GC.EnqueueInitiation", false)] public Action EnqueueInitiation { get; private set; }
    [EzIPC("PluginState.AbortAllTasks")] public Action AbortAllTasks { get; private set; }
    [EzIPC("PluginState.DisableAllFunctions")] public Action DisableAllFunctions { get; private set; }
    [EzIPC("PluginState.EnableMultiMode")] public Action EnableMultiMode { get; private set; }
    [EzIPC("PluginState.IsBusy")] public Func<bool> IsBusy { get; private set; }
    [EzIPC("PluginState.GetInventoryFreeSlotCount")] public Func<int> GetInventoryFreeSlotCount { get; private set; }
    /// <summary>
    /// 提供端是 AutoRetainer 的 <c>IPC_PluginState.EnqueueHET(Action onFailure)</c> —— 只有一個參數,
    /// 型別是失敗時的回呼 <c>Action</c>,不是兩個 bool。
    /// 原本宣告成 <c>Action&lt;bool, bool&gt;</c>,參數個數不符 ⇒
    /// <c>CallGateChannel.CheckAndConvertArgs</c> 擲 <c>IpcLengthMismatchError</c>,每一次呼叫都必定失敗。
    /// </summary>
    [EzIPC("PluginState.EnqueueHET")] public Action<Action> EnqueueHET { get; private set; }
    [EzIPC("PluginState.IsItemProtected")] public Func<uint, bool> IsItemProtected { get; private set; }
    [EzIPC("PluginState.GetMultiModeStatus")] public Func<bool> GetMultiModeStatus { get; private set; }
    [EzIPC("PluginState.GetClosestRetainerVentureSecondsRemaining")] public Func<ulong, long?> GetClosestRetainerVentureSecondsRemaining { get; private set; }
    [EzIPC("PluginState.AreAnyRetainersAvailableForCurrentChara")] public Func<bool> AreAnyRetainersAvailableForCurrentChara { get; private set; }
    [EzIPC("PluginState.AreAnyEnabledVesselsNotDeployed")] public AreAnyEnabledVesselsNotDeployed AreAnyEnabledVesselsNotDeployed { get; private set; }
    [EzIPC("PluginState.AreAnyEnabledVesselsReady")] public AreAnyEnabledVesselsReady AreAnyEnabledVesselsReady { get; private set; }
    /// <summary>
    /// When argument is set to null, user's choice from AR is used
    /// </summary>
    [EzIPC("PluginState.EnableSingleMultiMode")] public Action<MultiModeType?> EnableSingleMultiMode { get; private set; }
}