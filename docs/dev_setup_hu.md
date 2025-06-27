# Fejlesztői környezet beállítása

## Szükséges eszközök
- Windows 10 vagy újabb (a buildhez Windows Desktop SDK szükséges)
- .NET SDK 8.0 vagy újabb
- SQLite parancssori eszközök
- Bash shell a `setup.sh` futtatásához

## Kezdeti lépések
1. Klónozd a repót.
2. Futtasd a `./setup.sh` scriptet a megoldás visszaállításához, buildeléséhez és teszteléséhez.
3. A helyi SQLite adatbázis a `%LOCALAPPDATA%/Wrecept/wrecept.db` útvonalon jön létre.
   Ha a `%LOCALAPPDATA%` nem elérhető, a futtatható állomány mappáját használjuk.
   A SQLite sémát már az Entity Framework Core hozza létre első futtatáskor.

## Hibakeresés
- Győződj meg róla, hogy a `dotnet` elérhető az útvonalon.
- Első futtatás előtt add végrehajtási jogot a scriptnek: `chmod +x setup.sh`.
- Ha a tesztek hibával leállnak, töröld a `bin` és `obj` mappákat és futtasd újra a scriptet.
- Ha indításkor nem jelenik meg a főablak, nézd meg az `errors.log` fájlt a `%LOCALAPPDATA%/Wrecept` mappában. A hibás `settings.json` automatikusan ignorálásra kerül, az adatbázis törlése általában nem szükséges.
- Indítási hiba esetén a program az `errors.log` fájlba naplóz és memóriában folytatja a működést.
- A beállítások mentése vagy a számla exportálása sikertelen lehet fájlengedély probléma esetén; a hiba szintén ide naplózódik és üzenet jelzi.
