# Témakezelés

A Wrecept két beépített témát tartalmaz: **Light** és **Dark**. A `src/Themes` mappában lévő XAML szótárak határozzák meg az összes színt, betűtípust és vezérlőstílust. Az `App.ApplyTheme()` metódus tölti be a felhasználó által kiválasztott szótárat.

A kiválasztott téma a `%LOCALAPPDATA%/Wrecept/settings.json` fájlban tárolódik. Ha a fájl hiányzik vagy hibás, az alkalmazás automatikusan a `Light` témát alkalmazza.

A nézetek csak kulcsokra hivatkoznak (például `WindowBackground`, `PrimaryBrush`), így a témák cseréje nem igényel XAML módosítást.
