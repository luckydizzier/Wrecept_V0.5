# UI Flow

## Startup flow

1. Az alkalmazás indításakor az InvoiceEditorWindow nyílik meg teljes szélességben, automatikusan igazodva a képernyőhöz.
2. A számlalistában a legfelső (legutóbbi) tétel automatikusan kiválasztásra kerül; az Enter billentyű megnyomásával a meglévő szerkesztő új tétel beviteli sorára ugrik.
3. Első indításkor egy átlátszó súgóablak jelenik meg a főbb gyorsbillentyűkkel, Esc-pel vagy a Bezár gombbal zárható, később F1 hívja elő.

## Invoice editor layout

* A fő felület négy részre tagolt: InvoiceSidebar, InvoiceHeader, InvoiceItemsGrid és InvoiceSummary.
* A bal panel tetején "📄 Számlák" felirat látható, alatta középre igazított számlalista kap helyet.
* A fejléc bal oldala a szállító adatait, jobb oldala a számla jellemzőit tartalmazza.
* A grid kiemelt vizuális elemmé válik: nagyobb betűméret, váltakozó sorszínek, a placeholder sor halványabban jelenik meg.
* Fejlett mezők (pl. megjegyzés, számítás mód) alapból összehúzva, csak szerkesztés után nyílnak le.
* A „Adószám” mező és más származtatott mezők csak akkor jelennek meg, ha a szükséges előfeltételek (pl. szállító kiválasztva) teljesülnek.
* A menüsáv felső bejegyzései (pl. „Számlák”, „Listák”) nincsenek duplikálva, az aktív nézet mindig visszajelzést ad.

```
------------------------------
| Sidebar | Header          |
|         |-----------------|
|         | ItemsGrid       |
|         |-----------------|
|         | Summary         |
------------------------------
```

## Field interaction map

* Szállító, termék és más törzsadat mezők fókuszba kerülésekor automatikusan legördül a keresőlista.
* A lista első eleme előválasztott, a fel/le nyíllal mozogható, Enterrel kiválasztható, gépelésre pedig azonnal szűkül.
* A tételgrid utolsó mezőjén Enter új sort nyit; Escape a szerkesztést szakítja meg és a placeholder sor visszaáll.
* A Hozzáadás gomb alapértelmezett, így Enterrel is aktiválható.

## Keyboard & focus logic

1. A Tab sorrend: Sidebar lista → Header mezők → ItemsGrid → Summary → alsó eszköztár.
2. Minden új nézetre lépéskor a logikus első mező (Sidebar lista) kap fókuszt. A fókusz a `FocusManager.FocusedElement` beállítással indul az InvoiceList.
3. Ctrl+S mentésre, Esc az aktuális sor vagy ablak bezárására szolgál; Esc-sorozat esetén először a szerkesztő, majd a főmenü aktiválódik.
4. A menüsor Alt-tal, a gombok AccessKey jelöléssel érhetők el; az Enter és Esc útvonal minden dialógusban egységes.
5. Fontos mezők gyorsbillentyűi: Alt+N – Szállító, Alt+P – Számlaszám, Alt+D – Dátum, Alt+T – Tranzakciószám.
6. Az OnboardingOverlay megnyitásakor a Bezár gombon van a fókusz.
7. A szűrő- és beállítóablakok a `FocusManager.FocusedElement` tulajdonsággal jelölik ki az első mezőt.

🧾 Exit & Save flow
A szerkesztőből kilépés kizárólag az Esc megnyomásával történik.

Esc hatására megjelenik egy megerősítő párbeszédablak:

Szöveg: „Mentsem a számlát?”

Enter = Mentés → Számla mentése… felirat jelenik meg, majd eltűnik.

Esc = Elutasítás → visszatérés a számlaszerkesztőbe, fókusz az utolsó mezőre.

Mentés után a fókusz visszakerül a számlalistára, amely az újonnan mentett tétellel frissül.

Minden mentés vizuális (zöld villanás) és hangos visszajelzéssel történik.

## Feedback & affordance rules

* Indításkor "tu-ta-ti" hang, kilépéskor "ti-ta-tu" csendül fel.
* Sikeres műveletkor zöld villanás és "ta-ti" hang jelzi a mentést.
* Hiba esetén piros vagy sárga háttér, kétszer "tu" hang, illetve tooltip magyarázat jelenik meg.
* Gombok vizuálisan kiemelt státuszban vannak, a Hozzáadás gomb primer színű.
* Sor mentésénél rövid zöld villanás, hibánál rezgő animáció segít a visszajelzésben.

## Accessibility/Resizability notes

* A teljes felület rugalmasan skálázódik, nincs szükség manuális ablakméretezésre.
* A billentyűs navigáció minden elemre kiterjed; vizuális fókuszkeret segíti a tájékozódást.
* A kontrasztarány és betűméret megfelel a themes.md irányelveinek, a hangjelzések letilthatók a beállításokban.
