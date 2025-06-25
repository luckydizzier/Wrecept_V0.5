# Felhasználói kézikönyv

## Gyorsbillentyűk
- **Alt+S** – Számlák menü megnyitása
- **Alt+T** – Törzsek menü megnyitása
- **Alt+L** – Listák menü megnyitása
- **Alt+H** – Súgó menü megnyitása
- **F1** – Billentyűs útmutató megjelenítése

A menüpontokban a szokásos nyíl- és Enter billentyűkkel lehet navigálni. A Kilépés pont az alkalmazás bezárását végzi.

## Szűrők használata
Az **Alt+L** menüben választható ki a Dátum, Szállító, Termékcsoport vagy Termék szerinti szűrés. A párbeszédablakokban Enter alkalmazza a szűrőt, Esc pedig megszakítja.

## Törzsadat karbantartás
Az **Alt+T** menüben a Szállítók és Termékek ablakok érhetők el. Új rekord az **Insert**, módosítás mentése az **F2**, törlés a **Del** billentyűvel végezhető.

## Beállítások és Témák
A **Beállítások** ablakban választható ki a Light vagy Dark téma. A beállítások a kilépéskor automatikusan mentésre kerülnek a `%LOCALAPPDATA%/Wrecept/settings.json` fájlba, és következő indításkor betöltődnek.

## Hibakezelés
Ha az alkalmazás nem tudja elérni a SQLite adatbázist, hibaüzenet jelenik meg,
majd a program memóriában fut tovább. Zárolt fájl esetén a program kéri a másik
példány bezárását. Sérült adatbázisnál felajánlja az új, üres fájl létrehozását,
a régit `.bak` kiterjesztéssel megőrzi. Hiányzó fájl automatikusan létrejön. A
részletek az `errors.log` fájlban találhatók a `%LOCALAPPDATA%/Wrecept` mappában.
