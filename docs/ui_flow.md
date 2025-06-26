# UI Flow


- Felső menüsor Alt billentyűvel aktiválható.
- Számlák menü: szerkesztő nézet megnyitása.
- Törzsek menü: törzsadat karbantartó nézet.
- Listák menü: dátum, szállító, termékcsoport és termék szerinti keresés.
- Súgó menü: **Billentyűk** (F1), Súgó, Névjegy, **Beállítások**, Bővítmények és Kilépés.
- Aktív menüben az `Esc` kilépési megerősítést kér.

### Dátum szűrő dialógus
1. A "Listák" menüben a **Dátum alapú keresés** pont választása megnyit egy kis ablakot két DatePicker mezővel.
2. A "Kezdő dátum" és "Záró dátum" mezők Tab sorrendben elérhetők, Enter bármelyik mezőn aktiválja a **Szűrés** gombot.
3. `Esc` bármikor bezárja a dialógust változtatás nélkül a **Mégse** gombon keresztül.
4. A Szűrés gomb a kiválasztott tartomány alapján beállítja a szűrőt és a fókuszt visszaadja a főablaknak.

## Invoice Editor Focus Flow
1. A nézet betöltésekor a legfrissebb számla adatai jelennek meg.
2. A szerkesztő négy UserControl-ból áll: **InvoiceSidebar**, **InvoiceHeader**, **InvoiceItemsGrid** és **InvoiceSummary**. Mindegyik saját ViewModelen keresztül kommunikál a fő nézettel.
3. A Tab sorrend a következő:
   1. Sidebar mezők – kereső és dátummezők azonnal szűrik a listát; a táblázat nem enged új sor felvételt
   2. Header mezők (Szállító → Cím → Adószám → Számlaszám → Dátum → Fizetési mód → Számítás módja)
   3. Tételgrid első sora szerkeszthető; Enter a legutolsó mezőn sorba illeszti az adatokat, Esc törli a bevitelt
   4. Összesítő tábla
4. `Esc` bármelyik nézetben bezárja az ablakot. Ha Esc-cel történik a zárás, a főablakban a következő Esc már a főmenüre teszi a fókuszt, és ekkor jelenik meg a státuszsorban **"Fókusz: főmenü"**.
5. `Ctrl+S` menti a módosításokat és bezárja az ablakot.
6. Lista elején vagy végén történő navigációnál rövid hangjelzés szólal meg és státusz üzenet jelenik meg.
7. A felső menüsor navigációs célú (pl. Törzsek, Listák), az alsó eszköztár akcióorientált: **Mentés**, **Nyomtatás**, **Bezárás**, **Export**.
8. A tételgrid mellett egy ➕ gomb helyezkedik el, amely az `AddItemCommand`-hez van kötve.
9. Az új sor csak akkor kerül mentésre, ha minden mező kitöltött; hibás mező piros háttérrel jelölve.
10. Ha a termék neve ismeretlen és Entert nyomunk, a sor alatt megjelenik egy inline űrlap.
    - A formon Termékcsoport, Mértékegység, Egységár és ÁFA adható meg.
    - Enter menti, Escape elveti; siker esetén a termék bekerül a listába és kiválasztódik.
    - A tervezett sor dőlt betűvel (ghost row) látszik.
11. F2 vagy Ctrl+L bármely név vagy egység mezőben kereső párbeszédet nyit.
