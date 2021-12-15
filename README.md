[![License](https://img.shields.io/github/license/wsmd/ws-multipath.svg)](https://github.com/wsmd/ws-multipath/blob/master/LICENSE)
# unity-pool 物件池
```csharp
GOPool<ExempleItem> pool = new GOPool<ExempleItem>(
    prefab,
    defaultCount,
    maxCout,
    autoNew,
    patent,
    spwnPosition
);

ExempleItem item = pool.Spawn();

pool.Releas(item);

pool.ReleasAll();
```