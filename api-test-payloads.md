# API Test Payloads

## Success Cases

### 1. CUST-001 - VIEW_BALANCE - ACC-12345
```json
{
  "subjectId": "CUST-001",
  "permission": "VIEW_BALANCE",
  "resourceId": "ACC-12345"
}
```
**Expected:** `allowed: true`  
**Reason:** CUST-001 has `AccountHolder` role which assigns `VIEW_BALANCE` entitlement which grants access to `ACC-12345`.

---

### 2. CUST-001 - TRANSFER_FUNDS - ACC-12345
```json
{
  "subjectId": "CUST-001",
  "permission": "TRANSFER_FUNDS",
  "resourceId": "ACC-12345"
}
```
**Expected:** `allowed: true`  
**Reason:** CUST-001 has `AccountHolder` role which assigns `TRANSFER_FUNDS` entitlement which grants access to `ACC-12345`.

---

### 3. CUST-001 - VIEW_DASHBOARD - DASHB-001
```json
{
  "subjectId": "CUST-001",
  "permission": "VIEW_DASHBOARD",
  "resourceId": "DASHB-001"
}
```
**Expected:** `allowed: true`  
**Reason:** CUST-001 has `BusinessOwner` role which assigns `VIEW_DASHBOARD` entitlement which grants access to `DASHB-001`.

---

### 4. CUST-002 - VIEW_BALANCE - ACC-12345
```json
{
  "subjectId": "CUST-002",
  "permission": "VIEW_BALANCE",
  "resourceId": "ACC-12345"
}
```
**Expected:** `allowed: true`  
**Reason:** CUST-002 has `AccountHolder` role which assigns `VIEW_BALANCE` entitlement which grants access to `ACC-12345`.

---

### 5. CUST-002 - VIEW_HISTORY - ACC-12345
```json
{
  "subjectId": "CUST-002",
  "permission": "VIEW_HISTORY",
  "resourceId": "ACC-12345"
}
```
**Expected:** `allowed: true`  
**Reason:** CUST-002 has `ThirdParty` role which assigns `VIEW_HISTORY` entitlement which grants access to `ACC-12345`.

---

## Failure Cases

### 1. CUST-001 - VIEW_DASHBOARD - ACC-12345 (wrong resource)
```json
{
  "subjectId": "CUST-001",
  "permission": "VIEW_DASHBOARD",
  "resourceId": "ACC-12345"
}
```
**Expected:** `allowed: false`  
**Reason:** `VIEW_DASHBOARD` only grants access to `DASHB-001`, not `ACC-12345`.

---

### 2. CUST-002 - VIEW_DASHBOARD - DASHB-001 (wrong permission for role)
```json
{
  "subjectId": "CUST-002",
  "permission": "VIEW_DASHBOARD",
  "resourceId": "DASHB-001"
}
```
**Expected:** `allowed: false`  
**Reason:** CUST-002 has `AccountHolder` and `ThirdParty` roles - neither assigns `VIEW_DASHBOARD`.

---

### 3. CUST-001 - VIEW_HISTORY - ACC-12345 (wrong permission for role)
```json
{
  "subjectId": "CUST-001",
  "permission": "VIEW_HISTORY",
  "resourceId": "ACC-12345"
}
```
**Expected:** `allowed: false`  
**Reason:** CUST-001 has `AccountHolder` and `BusinessOwner` roles - neither assigns `VIEW_HISTORY`.

---

### 4. Non-existent Subject
```json
{
  "subjectId": "CUST-999",
  "permission": "VIEW_BALANCE",
  "resourceId": "ACC-12345"
}
```
**Expected:** `allowed: false`  
**Reason:** Subject `CUST-999` does not exist in the graph.

---

### 5. Non-existent Permission
```json
{
  "subjectId": "CUST-001",
  "permission": "ADMIN_ACCESS",
  "resourceId": "ACC-12345"
}
```
**Expected:** `allowed: false`  
**Reason:** `ADMIN_ACCESS` permission does not exist in the graph.

---

### 6. Non-existent Resource
```json
{
  "subjectId": "CUST-001",
  "permission": "VIEW_BALANCE",
  "resourceId": "RES-99999"
}
```
**Expected:** `allowed: false`  
**Reason:** Resource `RES-99999` does not exist in the graph.

---

## Bad Request Cases (400)

### Missing subjectId
```json
{
  "permission": "VIEW_BALANCE",
  "resourceId": "ACC-12345"
}
```

### Missing permission
```json
{
  "subjectId": "CUST-001",
  "resourceId": "ACC-12345"
}
```

### Missing resourceId
```json
{
  "subjectId": "CUST-001",
  "permission": "VIEW_BALANCE"
}
```

### Empty subjectId
```json
{
  "subjectId": "",
  "permission": "VIEW_BALANCE",
  "resourceId": "ACC-12345"
}
```