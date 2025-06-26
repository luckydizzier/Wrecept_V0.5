# Felhasználói kézikönyv

## Gyorsbillentyűk
- **Alt+S** – Számlák menü megnyitása
- **Alt+T** – Törzsek menü megnyitása
- **Alt+L** – Listák menü megnyitása
- **Alt+H** – Súgó menü megnyitása
- **F1** – Billentyűs útmutató (Súgó → Billentyűk)
- **F2** vagy **Ctrl+L** – törzsadat kereső felugró ablak
- **PgUp/PgDn** – lista lapozása

A menüpontokban a szokásos nyíl- és Enter billentyűkkel lehet navigálni. A Kilépés pont az alkalmazás bezárását végzi.

## Szűrők használata
Az **Alt+L** menüben választható ki a Dátum, Szállító, Termékcsoport vagy Termék szerinti szűrés. A párbeszédablakokban Enter alkalmazza a szűrőt, Esc pedig megszakítja.

## Törzsadat karbantartás
Az **Alt+T** menüben a Szállítók és Termékek ablakok érhetők el. Új rekord az **Insert**, módosítás mentése az **F2**, törlés a **Del** billentyűvel végezhető.

## Számlatételek kezelése
A végösszeg alatt megjelenik az összeg szöveges formában is,
amit a HungarianNumberConverter számít ki.
A tételek alatti **Hozzáadás** gomb (vagy Enter az utolsó mezőben) rögzíti az új sort.
Ha kötelező mező hiányzik, a sor piros háttérrel jelzi a hibát.
Ha a termék neve nincs a törzsben, Enterrel megnyílik egy sor alatti gyors űrlap,
ahol megadhatjuk a hiányzó adatokat. A Mentés gomb (vagy Enter) azonnal
felveszi a terméket és kitölti a tételmezőt; Esc bezárja a formot.
F2 vagy Ctrl+L megnyitja a kereső dialógust, ahol a meglévő termékek, egységek vagy ÁFA kulcsok között válogathatunk.

## Bővítmények menü
A **Súgó → Bővítmények** alatt jelennek meg a `Plugins` mappából betöltött modulok.
Az egyes bővítmények saját párbeszédet vagy műveletet indíthatnak.
Részletesebb ismertetés: [plugin_command_bar.md](plugin_command_bar.md).

## Súgó és Névjegy
Az **Alt+H** menüben található a **Billentyűk** pont (F1), a Súgó és a Névjegy ablak. A Billentyűk ablak felsorolja a legfontosabb gyorsbillentyűket. A Súgó röviden összefoglalja a dokumentáció helyét, míg a Névjegy a program verzióját mutatja. Mindhárom ablak az Esc billentyűvel zárható.

## Beállítások és Témák
Az **Alt+H → Beállítások** menüpont nyitja meg a konfigurációs ablakot, ahol a Light vagy Dark téma és az alkalmazás nyelve választható ki. A beállítások a kilépéskor automatikusan mentésre kerülnek a `%LOCALAPPDATA%/Wrecept/settings.json` fájlba, és következő indításkor betöltődnek.

## Hibakezelés
Ha az alkalmazás nem tudja elérni a SQLite adatbázist, hibaüzenet jelenik meg,
majd a program memóriában fut tovább. Zárolt fájl esetén a program kéri a másik
példány bezárását. Sérült adatbázisnál felajánlja az új, üres fájl létrehozását,
a régit `.bak` kiterjesztéssel megőrzi. Hiányzó fájl automatikusan létrejön. A
részletek az `errors.log` fájlban találhatók a `%LOCALAPPDATA%/Wrecept` mappában.
