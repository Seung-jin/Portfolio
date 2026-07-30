## Factory
ID에 맞는 총알 프리팹을 팩토리에서 가져오고, 사용이 끝난 총알은 다시 풀로 반환하는 예제<br>
`BulletFactory`가 ID별 풀을 관리하고 `FactoryExample`은 UI 입력값으로 총알을 생성

---

### BulletPoolData
총알 ID와 프리팹을 연결하는 데이터
```csharp
[Serializable]
public class BulletPoolData {
    public int Id;
    public BulletObject Prefab;
}
```

---

### GetBullet
ID에 맞는 풀에서 총알을 가져옴<br>
없는 ID를 요청하면 `null` 반환
```csharp
BulletObject bullet = _bulletFactory.GetBullet(bulletId);

if (bullet == null) {
    return;
}
```

---

### ReturnBullet
사용한 총알을 다시 풀로 반환
```csharp
_bulletFactory.ReturnBullet(bullet);
```

---

### ResetBullets
현재 생성되어 있는 총알을 모두 반환하고 목록 초기화
```csharp
for (int i = _activeBullets.Count - 1; i >= 0; i--) {
    _bulletFactory.ReturnBullet(_activeBullets[i]);
}

_activeBullets.Clear();
```

---

### Example
입력한 ID의 총알을 지정한 개수만큼 생성하고, 다시 만들기 전에 이전 총알을 반환
```csharp
public void CreateBullet() {
    ResetBullets();

    int bulletId = GetBulletId();

    for (int i = 0; i < _createCount; i++) {
        BulletObject bullet = _bulletFactory.GetBullet(bulletId);

        if (bullet == null) {
            continue;
        }

        _activeBullets.Add(bullet);
    }
}
```
