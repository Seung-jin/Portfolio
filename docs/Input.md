## Input
Unity Input System으로 이동 입력과 흔들기 입력을 처리하는 예제<br>
`PlayerInput`에서 액션을 찾고, `InputLogic`으로 흔들기 키 바인딩을 변경

![입력 키 변경](InputExample.gif "입력 키 변경")
---

### PlayerInput
`PlayerInput`과 `InputAction`을 준비하고 C# 이벤트 방식으로 입력을 받음
```csharp
_playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
_moveAction = _playerInput.actions.FindAction("Move", true);
_shakeAction = _playerInput.actions.FindAction("Shake", true);
```

---

### OnEnable
입력 액션 이벤트 등록 후 액션 활성화
```csharp
_moveAction.performed += OnMove;
_moveAction.canceled += OnMove;
_shakeAction.performed += OnShake;
_playerInput.actions.Enable();
```

---

### Move
`Move` 액션의 `Vector2` 입력값으로 대상 오브젝트 이동
```csharp
private void OnMove(InputAction.CallbackContext context) {
    _moveInput = context.ReadValue<Vector2>();
}

private void Update() {
    Move(_moveInput);
}
```

---

### SetShakeKey
허용된 키만 사용하도록 보정한 뒤 `Shake` 액션의 바인딩 변경
```csharp
public void SetShakeKey(Key shakeKey) {
    shakeKey = InputLogic.NormalizeShakeKey(shakeKey);
    _inputLogic.SetShakeKey(shakeKey);
}
```

---

### Shake
흔들기 입력을 받으면 로그를 남기고 대상 오브젝트 흔들기 실행
```csharp
private void OnShake(InputAction.CallbackContext context) {
    AddLog("흔들기 성공");
    Shake();
}
```

---

### Example
UI 버튼에서 A/S/D/F 중 하나를 선택해 흔들기 키를 변경
```csharp
public void SetShakeKeyA() {
    SetShakeKey(Key.A);
}

public void SetShakeKeyS() {
    SetShakeKey(Key.S);
}
```