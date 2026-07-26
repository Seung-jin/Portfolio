## Pooling

### ObjectPooler
프리팹을 풀에서 꺼내고 다시 반환하는 제너릭 풀러
```csharp
private ObjectPooler<TestPoolingObject> _pooler;

_pooler = new ObjectPooler<TestPoolingObject>(_prefab, _poolParent);
TestPoolingObject obj = _pooler.Get();
```

---

### PoolingObject
풀링 대상 오브젝트의 기본 클래스<br>
꺼낼 때 활성화하고 반환할 때 비활성화
```csharp
public class TestPoolingObject : PoolingObject<TestPoolingObject> {
    public override void OnReleaseToPool() {
        base.OnReleaseToPool();
    }
}
```

---

### Release
사용이 끝난 오브젝트를 풀로 반환
```csharp
_pooler.Release(poolingObject);
```

---

### Clear
풀에 들어있는 오브젝트 제거
```csharp
_pooler.Clear();
```

---

### Example
입력한 개수만큼 오브젝트를 가져오고, 리셋 시 다시 풀로 반환
```csharp
for (int i = 0; i < count; i++) {
    TestPoolingObject poolingObject = _pooler.Get();
    poolingObject.SetId(_nextId);
}
```