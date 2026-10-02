# Steam Multiplayer Project Template

The project is a **modular template for co-op multiplayer games on Steam**, made with **Unity 6000.3.16f1**. Networking runs on *FishNet* over *Steam P2P*, DI on *VContainer*, and every module is split into client, server and shared parts

This is what I start multiplayer projects from: the player, entities, items, triggers, HUD and menus are already in place, everything is synced over the network, and the core gameplay rules are covered by network tests

---

### What's already there
1. **Player**: movement and ground check, a state machine for *legs and hands*, input handlers and an *Operator* mode — a free spectator camera
2. **Entities**: health, damage and healing (dealers, receivers, zones), effects such as damage over time, destructible objects, explosions with knockback, health bars
3. **Items and inventory**: inventory slots, factories for items and their views, interactable objects, surface types for hits
4. **Triggers**: a data-driven *condition → reaction* system: spawn, despawn, enabling objects, animations, events, counters
5. **Levels**: level zones, lighting and fog configs, spawn and despawn zones
6. **HUD and UI**: cheat menu, hints, low-health popup, interaction prompts; main menu, pause, settings (volume, mouse sensitivity), loading screen
7. **Localization**: English and Russian via *Unity Localization*

---

### Networking
1. **FishNet + FishySteamworks**: players connect over *Steam P2P*. In the Editor the transport can be switched to plain FishNet for quick local tests — the `UseSteamInEditor` flag in `SteamEditorConfig`
2. **Authoritative server**: state travels through paired *ForServer / ForClient* broadcasts, wired together by `ServerBroadcastSynchronizer<T>`, `ClientBroadcastSynchronizer` and synchronizer mediators for the owner, server and clients
3. Simple values use small synchronizers built on `SyncVar` and `ServerRpc`, plus `Rigidbody` sync and a network timer
4. Scenes are loaded over the network through *Addressables*, and networked prefabs are registered from *Addressables* too
5. Connecting: host and client buttons in the main menu, the client enters the host's *SteamID* (it is remembered), and the host can copy their own *SteamID* with one button

---

### Architecture
1. **DI on VContainer**, with scopes split by lifetime:
   - `PersistentServicesScope` — for the whole app, registers every `IPersistentService`, `IPersistentFactory`, `IConfigsProviderService` and `IGameState` automatically
   - `MatchSharedServicesScope`, `MatchServerServicesScope`, `MatchClientServicesScope` — per match
   - `OwnerPlayerLifetimeScope` — for the local player
2. **Game state machine**: `Bootstrap → MainMenu → Match → Exit`, plus a separate server state machine
3. **Modules** live in `Assets/Modules`, each split into `Runtime/Client`, `Runtime/Server` and `Runtime/Shared` with their own assemblies, so server code never ends up in the client and vice versa
4. Patterns: event bus, mediators, repositories, factories, object pools. Async code on *UniTask*, reactive code on *UniRx*
5. *Addressables* with 26 groups and a loader that can fall back to `Resources`

---

### Tests
1. **Network play mode tests** for damage, healing and effects: they spin up a real host and client through *ParrelSync* clones and are enabled with the `PARREL_SYNC_TESTS` define
2. Unit tests for entity rules (`DoDamageTest`, `DoHealTest`) on *FluentAssertions*

---

### Editor Tools
1. **Moving assets between Addressables and Resources** to check the build both ways — `Tools/Core Module/Settings`
2. Define symbol tools, find asset by GUID, missing prefabs finder, layer finder
3. **4K game screenshot** — `Tools/Make Game Screenshot 4K`, and a shader preprocessor that strips unused variants

---

### How to run
1. Open the project in **Unity 6000.3.16f1**
2. Start the **Steam** client: `steam_appid.txt` is set to **480** — Valve's test app *Spacewar*, so you don't need your own App ID
3. Open the `Assets/Modules/AppModule/Runtime/Shared/Scenes/Initial.unity` scene and press Play
4. To test multiplayer locally, open a second Editor through *ParrelSync → Clones Manager*: host in one, join with the host's *SteamID* in the other. To test over Steam right from the Editor, turn on `UseSteamInEditor` in `SteamEditorConfig`

Build scenes: `Initial`, `MainMenu`, `Game`

---

### Stack
*Unity 6*, *C#*, *FishNet*, *FishySteamworks*, *Steamworks.NET*, *VContainer*, *UniTask*, *UniRx*, *Addressables*, *Input System*, *Unity Localization*, *URP*, *Animancer*, *DOTween*, *ParrelSync*, *FluentAssertions*

---

*Happy to hear any feedback or questions! :)*
