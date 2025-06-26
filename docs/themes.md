# Témakezelés

A Wrecept két beépített témát tartalmaz: **Light** és **Dark**. A `src/Themes` mappában lévő XAML szótárak határozzák meg az alkalmazás összes színét és alap stílusát. Az `App.ApplyTheme()` metódus tölti be a felhasználó által kiválasztott szótárat.

Az alap betűméret a `BaseFontSize` dinamikus erőforrásban van meghatározva, értéke 14 + `FontScale`.

A beállítás a `%LOCALAPPDATA%/Wrecept/settings.json` fájlban tárolódik. Hibás vagy hiányzó fájl esetén a program a `Light` témát használja.

## Színkulcsok
- `BackgroundColor` – ablakok háttérszíne
- `AccentColor` – kiemelések és kijelölések
- `TextColor` – alap szövegszín
- `ButtonColor` – gombok normál háttere
- `ButtonHoverColor` – gomb fölötti állapot
- `TableHeaderColor` – táblázat fejlécek
- `TableRowHighlight` – váltakozó sorok háttérszíne
- `ErrorColor` – hibás mezők kerete

A funkciógombok alapszíne minden témában egységes: Light alatt `#E0E0E0`, Dark alatt `#3A3A3A`.

## Stíluselvek
- Gombok és listaelemek egységes `Padding` értéke `8,4`.
- A táblázatok fejlécét a `TableHeaderColor`, a sorok váltakozó hátterét a `TableRowHighlight` adja.
- Hibás bevitelnél a vezérlő `ErrorColor` színű keretet kap, a hibaüzenet tooltipben jelenik meg.

A nézetek csak kulcsokra hivatkoznak, így a témák bővítése nem igényel XAML módosítást.
