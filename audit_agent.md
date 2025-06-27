### audit_agent.md – UX Compliance & Refactor Prompt

#### 🎯 Célkitűzés

Ez az ügynök felel a felhasználói felület viselkedésének auditálásáért és a korábban érvényes, de mostanra **elavult shortcut-logika** automatikus felderítéséért és refaktorálásáért.

#### 📘 Hivatkozási alap

* `docs/ui_flow.md`: Az egyetlen érvényes referencia a UI, billentyűkezelés, nézetváltás és visszajelzések logikájához
* `docs/SUMMARY.md`: Modul-struktúra, kódrétegek és nézet-hierarchia áttekintése

---

### 🔍 Audit Feladatok

1. **Shortcut Audit**
   * Keress minden `Ctrl+S`, `Ctrl+L`, `F2` eseménykezelőt (ViewModel, code-behind, service)
   * Jelöld meg ezeket "DEPRECATED" kommenttel, ha nem lettek még eltávolítva

2. **Focus és Escape/Enter Audit**
   * Vizsgáld meg, hogy minden dialógus, overlay és mező:
     * Enter → pozitív (confirm)
     * Escape → negatív (cancel)
   * Különösen vizsgálandó fájlok:
     * `InvoiceEditorWindow.xaml(.cs)`
     * `InvoiceItemRowViewModel.cs`
     * `LookupDialog*.xaml(.cs)`
     * `SettingsWindow.xaml(.cs)`

3. **Sidebar Navigáció Ellenőrzése**
   * Bizonyosodj meg róla, hogy **minden szerkesztés** az `InvoiceSidebar`-on keresztül történik:
     * ↑: új számla
     * Enter: szerkesztés
     * Escape: kilépés mentéssel (Enter), vagy visszalépés

4. **Konvenció Audit**
   * A `TabIndex`, `FocusManager.FocusedElement` és `AccessKey` attribútumok szerepelnek-e a főbb nézetekben
   * A "Hozzáadás" gomb `IsDefault="True"` beállítással rendelkezik-e

---

### 🔧 Refaktor Feladatok

1. **Távolítsd el vagy refaktoráld** a következő shortcutokat:
   * `Ctrl+S` → nincs mentés-gomb, automatikus mentés történik az `InvoiceSidebar` visszalépéskor
   * `F2`, `Ctrl+L` → minden lookup mező automatikusan legördül fókuszkor, Enterrel választ

2. **Egységesítsd a fókuszkezelést**:
   * Minden fő dialógusban: Enter/Esc logika kötelező
   * A `HelpWindow`, `SettingsWindow`, `AboutWindow` overlay típusú viselkedést kövessen (nem blokkol, visszalépés ESC-pel)

3. **Unit test javaslatok**:
   * A `tests/ui_tests/` mappába kerüljön legalább:
     * `InvoiceSidebarNavigationTest`
     * `KeyboardShortcutRefactorTest`
     * `DialogEnterEscTest`

---

### ✅ Kimeneti elvárások

* Az audit során jelzett helyek dokumentálva legyenek a `docs/progress/YYYY-MM-DD_audit_agent.md` fájlban
* Minden refaktor előtt és után `git diff` készül, és PR-ban szerepel
* Az elavult shortcutokra issue nyílik, ha még nem került refaktorálásra
* Az `AGENTS.md` naprakész utasítást adjon a shortcut-mentes működésre
* ❗ **Ha egy ügynök nem végezte el a rá bízott módosításokat, köteles azt haladéktalanul pótolni.** Az `audit_agent` rögzítse az elmaradás tényét, és aktiválja újra az adott refaktoráló ügynököt.

---

### 🔁 Iterációs gyakoriság

* minden major feature PR előtt kötelező (CI pipelinen belül is használható)
* 2 óránként manuálisan, ha nincs PR-tevékenység
