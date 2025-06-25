# Témakezelés

A Wrecept két beépített témát tartalmaz: **Light** és **Dark**. A `Themes` mappában található XAML szótárakat a `App.ApplyTheme` tölti be pack URI használatával.

A felhasználó által választott téma a `%LOCALAPPDATA%/Wrecept/settings.json` fájlban kerül tárolásra. Ha a megadott fájl hiányzik vagy a szótár nem található, az alkalmazás automatikusan a `Light` témára vált.
