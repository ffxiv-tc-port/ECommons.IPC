<!-- ffxiv-tc-port 繁體中文說明開始 -->
# ECommons.IPC(台服 fork)

由 NightmareXIV 開發的函式庫，收錄各插件的 IPC 呼叫封裝，避免每個插件各自複製貼上一份。

## 台服 fork 的目的

跟隨艦隊釘 API13 / net9，並把訂閱端的宣告對齊艦隊裡提供端插件的實際簽章——宣告與提供端
型別或參數個數不符時，每次呼叫都會失敗，而且 `SafeWrapper.IPCException` 攔不住那種例外。

## 與上游的差異

- **Questionable**：`ClearQuestPriority`／`AddQuestPriority` 的參數宣告原本對調，已改成與提供端一致。
- **AutoDuty**：`SetConfig` 第二個參數改成 `string`（提供端的實際型別）；`SetConfigValue` 收到多個值時
  記錄錯誤後略過，不再送出必定失敗的呼叫。
- **AutoRetainer**：`EnqueueHET` 參數宣告對齊提供端。
- **Lifestream**：`GetResidentialTerritory` 改用住宅區傳送點列舉 `ResidentialAetheryteKind`。
- **Gearsetter**：`SourceInventorySlot` 改為與提供端相同的 `int?`。
- **AllaganTools**：新增 `GetCharactersOwnedByActive`。
- 專案檔改成艦隊的 API13 / net9 建置方式，移除上游的發布 workflow。

上游之後的新增內容沒有合併進來（艦隊釘 API13，上游已往後走）。

## 誰在用它

目前 2 個插件 repo 以子模組使用：`AutoDuty`、`AutoRetainer`。

---

以下為上游原始 README，內容未經修改：

<!-- ffxiv-tc-port 繁體中文說明結束 -->

# ECommons.IPC
This library aims to collect IPC of various plugins to avoid/minimize amount of copypasting between plugins and for easier overall maintenance.

# How to use?
### 0. Setup
Follow the setup instructions in [the parent repository](https://github.com/NightmareXIV/ECommons)'s README.
### 1. Lazy initialization.
Just access static `ECommonsIPC` class and pick your plugin. For example: `ECommonsIPC.Teleporter.Teleport(9, 0);`
### 2. Create your own instance.
`new TeleporterIPC();` or with wrapper: `new TeleporterIPC(SafeWrapper.AnyException);`. Also can be used with ECommons SingletonServiceManager.

## Safe Wrapper
If you want to use SafeWrapper globally by default, reassign it in your plugin's constructor like this:
```
public Plugin(IDalamudPluginInterface pi)
{
    IPCBase.DefaultWrapper = SafeWrapper.AnyException;
    //continue initialization
}
```

# Guidelines for adding a plugin
Note: ECommons.IPC does not attempts to preemptively gather all and any plugins and their methods. They are being added as they are needed; if you need extra method or extra plugin - feel free to pull request. 

**Plugin should have it's source code publicly available.** It doesn't necessarily have to have FOSS license, just source has to be published. And, if it has FOSS license but is distributed privately, it is not allowed to be added to this library.

1. Create a subfolder for the plugin in Subscribers folder. Name it after plugin's internal name or display name, whichever makes more sense.
2. Create a `PluginNameIPC.cs` file, which should inherit from `IPCBase`
3. Override `InternalName` (mandatory). If plugin's prefix differs from internal name, override it too (optional). 
4. Add your IPC methods. If custom structs and enums are needed, bring them as separate classes.
5. Finally, navigate to `ECommonsIPC.cs` and add a static property with your IPC there, similar to how other subscribers are already added there.

## Additional notes
1. **Custom delegates vs `Action`/`Func`**. Custom delegates are not mandatory, but can be great help if it's not clear what is expected as arguments. Define custom delegates in a nested static class named `Delegates`. If function is self-explanatory or doesn't even accepts any arguments, feel free to just use `Action`/`Func`.
2. **Helper methods.** Feel free to bring them whenever they make sense, just don't overdo it. If you prefer, you can keep delegates private and provide public methods that call delegates instead. 
3. **Events.** For now I decided not to handle events as they are rarely used by plugins at all and additional overhead they provide. However, it can be reconsidered if needed. 
