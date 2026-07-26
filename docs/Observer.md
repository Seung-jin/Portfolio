## Observer
enum 값을 이벤트 키로 사용해 콜백을 등록하고 호출하는 간단한 옵저버<br>
인자가 없는 이벤트와 `params object[]` 인자를 받는 이벤트를 모두 지원

---

### AddListener
이벤트에 콜백 등록
```csharp
private Observer<BattleEventType> _observer = new Observer<BattleEventType>();

_observer.AddListener(BattleEventType.Attack, OnAttack);
_observer.AddListener(BattleEventType.Message, OnMessage);
```

---

### TriggerEvent
등록된 콜백 호출<br>
같은 이벤트에 일반 콜백과 인자 콜백이 모두 있으면 둘 다 실행
```csharp
_observer.TriggerEvent(BattleEventType.Attack, "Player", "Enemy", 10);
_observer.TriggerEvent(BattleEventType.BattleReset);
```

---

### RemoveListener
더 이상 이벤트를 받지 않을 때 콜백 해제
```csharp
private void OnDisable() {
    _observer.RemoveListener(BattleEventType.Attack, OnAttack);
}
```

---

### ClearAll
등록된 모든 이벤트 제거
```csharp
_observer.ClearAll();
```

---

### Example
데미지를 받으면 이벤트를 발송하고 UI나 로그에서 받아 처리
```csharp
private void TakeDamage(int damage) {
    _observer.TriggerEvent(BattleEventType.Damaged, "Enemy", damage);
}

private void OnDamaged(params object[] args) {
    string targetName = (string)args[0];
    int damage = (int)args[1];
}
```