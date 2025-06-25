# UI Flow

## Invoice List
- On startup the most recent invoice is selected automatically.
- Arrow Up/Down moves the selection.
- If the first row is selected and Up is pressed, a confirmation dialog appears asking to create a new invoice.
- Enter on a row opens the invoice editor in read-only mode.
- Confirming creation opens the editor in edit mode with empty data.
- Escape closes the editor or dismisses the confirmation.
- Repeated Up or Down at the edges plays a beep and keeps the current row.
- Deleting a row selects the last remaining invoice if any.
- When navigation is blocked a short message appears in the status bar.
- Selection is clamped within the list after any change.
- Az első indításkor megjelenik egy billentyűs súgó, amely F1 segítségével később is előhozható.

## Menu System
- Felső menüsor Alt billentyűvel aktiválható.
- Számlák menü: kezelő nézet megnyitása és lista frissítése.
- Törzsek menü: törzsadat karbantartó nézet.
- Listák menü: dátum, szállító, termékcsoport és termék szerinti keresés.
- Súgó menü: súgóablak, névjegy, valamint kilépés.

### Dátum szűrő dialógus
1. A "Listák" menüben a **Dátum alapú keresés** pont választása megnyit egy kis ablakot két DatePicker mezővel.
2. A "Kezdő dátum" és "Záró dátum" mezők Tab sorrendben elérhetők, Enterrel aktiválható a **Szűrés** gomb.
3. `Esc` bármikor bezárja a dialógust változtatás nélkül.
4. A Szűrés gomb a kiválasztott tartomány alapján frissíti a számlalistát és a fókuszt visszaadja a főablaknak.

## Invoice Editor Focus Flow
1. A nézet betöltésekor a számlalistán a legfrissebb sor aktív.
2. A szerkesztő négy UserControl-ból áll: **InvoiceSidebar**, **InvoiceHeader**, **InvoiceItemsGrid** és **InvoiceSummary**. Mindegyik saját ViewModelen keresztül kommunikál a fő nézettel.
3. A Tab sorrend a következő:
   1. Számlalistát tartalmazó Sidebar mezők
   2. Header mezők (Szállító → Cím → Adószám → Számlaszám → Dátum → Fizetési mód → Számítás módja)
   3. Tételgrid parancs gombok és lista
   4. Összesítő tábla
4. `Esc` bármelyik nézetben bezárja az ablakot és a státusz sorban megjelenik **"Fókusz: főmenü"**.
5. `Ctrl+S` menti a módosításokat.
6. Lista elején vagy végén történő navigációnál rövid hangjelzés szólal meg és státusz üzenet jelenik meg.
