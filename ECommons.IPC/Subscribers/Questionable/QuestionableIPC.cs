﻿using ECommons.EzIpcManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommons.IPC.Subscribers.Questionable;

public sealed class QuestionableIPC : IPCBase
{
    public QuestionableIPC()
    {
    }

    public QuestionableIPC(SafeWrapper wrapper) : base(wrapper)
    {
    }

    public override string InternalName { get; } = "Questionable";

    [EzIPC("IsRunning")] public Func<bool> IsRunning{get; private set;}
    [EzIPC("GetCurrentQuestId")] public Func<string?> GetCurrentQuestId{get; private set;}
    [EzIPC("GetCurrentStepData")] public Func<StepData?> GetCurrentStepData{get; private set;}
    [EzIPC("GetCurrentlyActiveEventQuests")] public Func<List<string>> GetCurrentlyActiveEventQuests{get; private set;}
    [EzIPC("StartQuest")] public Func<string, bool> StartQuest{get; private set;}
    [EzIPC("StartSingleQuest")] public Func<string, bool> StartSingleQuest{get; private set;}
    [EzIPC("IsQuestLocked")] public Func<string, bool> IsQuestLocked{get; private set;}
    [EzIPC("ImportQuestPriority")] public Func<string, bool> ImportQuestPriority{get; private set;}
    // 這兩個端點的宣告原本是對調的。提供端(Questionable/External/QuestionableIpc.cs)註冊的是
    //   GetIpcProvider<bool>("Questionable.ClearQuestPriority")       -> ClearQuestPriority() 不吃參數
    //   GetIpcProvider<string, bool>("Questionable.AddQuestPriority") -> AddQuestPriority(string questId)
    // 參數個數不符時 CallGateChannel.CheckAndConvertArgs 直接擲 IpcLengthMismatchError,
    // 也就是這兩支在對調期間**每一次呼叫都必定失敗**;而 SafeWrapper.IPCException 只攔
    // IpcNotReadyError,攔不住它,例外會一路擲到呼叫端。
    [EzIPC("ClearQuestPriority")] public Func<bool> ClearQuestPriority{get; private set;}
    [EzIPC("AddQuestPriority")] public Func<string, bool> AddQuestPriority{get; private set;}
    [EzIPC("InsertQuestPriority")] public Func<int, string, bool> InsertQuestPriority{get; private set;}
    [EzIPC("ExportQuestPriority")] public Func<string> ExportQuestPriority{get; private set;}
    [EzIPC("Stop")] public Func<string, bool> Stop { get; private set; }
}
