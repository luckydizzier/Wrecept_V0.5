# UI Flow

## Startup flow
1. Az alkalmazás indításakor az **InvoiceEditorWindow** nyílik meg teljes szélességben, automatikusan igazodva a képernyőhöz.
2. A számlalistában a legfelső tétel azonnal kiválasztott állapotú; a kurzor is ide kerül, így a Fel nyíllal új számlát lehet létrehozni.
3. Első indításkor egy átlátszó súgóablak jelenik meg a főbb gyorsbillentyűkkel, `Esc`-pel vagy a **Bezár** gombbal zárható, később `F1` hívja elő.

## Invoice editor layout
* A fő felület négy részre tagolt: **InvoiceSidebar**, **InvoiceHeader**, **InvoiceItemsGrid** és **InvoiceSummary**.
* A fejléc bal oldala a szállító adatait, jobb oldala a számla jellemzőit tartalmazza.
* A grid kiemelt vizuális elemmé válik: nagyobb betűméret, váltakozó sorszínek, a placeholder sor halványabban jelenik meg.
* Fejlett mezők (pl. megjegyzés, számítás mód) alapból összehúzva, csak szerkesztés után nyílnak le.

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
* A **Hozzáadás** gomb alapértelmezett, így Enterrel is aktiválható.

## Keyboard & focus logic
1. A Tab sorrend: Sidebar kereső → Header mezők → ItemsGrid → Summary → alsó eszköztár.
2. Minden új nézetre lépéskor a logikus első mező kap fókuszt, a lista ablak már nem igényel kézi átméretezést.
3. `Ctrl+S` mentésre, `Esc` az aktuális sor vagy ablak bezárására szolgál; Esc-sorozat esetén először a szerkesztő, majd a főmenü aktiválódik.
4. A menüsor Alt-tal, a gombok AccessKey jelöléssel érhetők el; az Enter és Esc útvonal minden dialógusban egységes.

## Feedback & affordance rules
* Indításkor "tu-ta-ti" hang, kilépéskor "ti-ta-tu" csendül fel.
* Sikeres műveletkor zöld villanás és "ta-ti" hang jelzi a mentést.
* Hiba esetén piros vagy sárga háttér, kétszer "tu" hang, illetve tooltip magyarázat jelenik meg.
* Gombok vizuálisan kiemelt státuszban vannak, a **Hozzáadás** gomb primer színű.
* Sor mentésénél rövid zöld villanás, hibánál rezgő animáció segít a visszajelzésben.

## Accessibility/Resizability notes
* A teljes felület rugalmasan skálázódik, nincs szükség manuális ablakméretezésre.
* A billentyűs navigáció minden elemre kiterjed; vizuális fókuszkeret segíti a tájékozódást.
* A kontrasztarány és betűméret megfelel a `themes.md` irányelveinek, a hangjelzések letilthatók a beállításokban.

