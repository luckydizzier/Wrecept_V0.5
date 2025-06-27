# 📌 TASK\_TEMPLATE.md – Új feladatok rögzítési sablonja

Ezt a sablont használd új fejlesztési, refaktorálási vagy dokumentációs feladatok `TASKLOG.md`-be való felvételéhez. A cél az egységes formátum, teljes traceability, és az ügynökalapú fejlesztés támogatása.

---

## 🧾 Feladatsablon

```markdown
- [ ] <Feladat leírása egy mondatban> — *<agent>*
  - **Milestone:** M<szám> – <rövid név>
  - **Spec:** Specs/<fájlnév>.md *(ha van)*
  - **Progress log expected:** `docs/progress/<UTC timestamp>_<agent>.md`
  - **Notes:** <opcionális megjegyzés>
  - **Status:** open | in_progress | done | blocked
  - **Decision:** *(csak ha NEEDS_HUMAN_DECISION)*
```

### Példa:

```markdown
- [ ] Add keyboard-navigable LookupDialog to Product field — *CodeGen-XAML*
  - **Milestone:** M7 – Entity Lookup Integration
  - **Spec:** Specs/LookupDialogs.md
  - **Progress log expected:** docs/progress/2025-06-27T14:12:00_CodeGen-XAML.md
  - **Notes:** Opens with F2 or Ctrl+L, closes with Esc
  - **Status:** open
```

---

## 🛠 Ajánlott mezők kitöltési segédlete

| Mező           | Példa                                 | Kötelező? |
| -------------- | ------------------------------------- | --------- |
| Feladat leírás | Implement JSON price history tracking | ✅         |
| Agent          | CodeGen-CSharp                        | ✅         |
| Milestone      | M7 – Entity Lookup Integration        | ✅         |
| Spec           | Specs/LookupDialogs.md                | ❌         |
| Progress log   | UTC timestamp + agent névvel          | ✅         |
| Notes          | Shortcuts, UX context, etc.           | ❌         |
| Status         | open / in\_progress / done / blocked  | ✅         |
| Decision       | NEEDS\_HUMAN\_DECISION (ha szükséges) | ❌         |

---

> A kitöltött feladatot másold át a `TASKLOG.md` megfelelő milestone alá.

