# Dependency Injection Migration

## Context
Wrecept jelenleg egy statikus `AppContext` osztályon keresztül éri el a szolgáltatásokat és adattárolókat. Ez megnehezíti a komponensek cseréjét és a tesztelést, mert a globális állapotot minden modul ismeri.

## Objectives
- Microsoft.Extensions.DependencyInjection bevezetése egységes szolgáltatásregisztrációval.
- Minden ViewModel és szolgáltatás konstruktoron keresztül kapja a függőségeit.
- A statikus `AppContext` fokozatos kivezetése, hogy a kód modulárisabb legyen.

## Constraints
- Offline-first működés és az egyszerű telepítés továbbra is követelmény.
- A meglévő interfészek megmaradnak, hogy a régi és új kód együtt működjön a migráció alatt.

## Tasks
1. **CodeGen-CSharp** – hozzon létre DI konfigurációt `Startup` vagy `Program` osztályban, és regisztrálja a jelenlegi szolgáltatásokat.
2. **CodeGen-CSharp** – refaktorálja a ViewModel-eket és szolgáltatásokat úgy, hogy a `AppContext` helyett konstruktoron keresztül kapják a függőségeiket.
3. **TestWriter** – frissítse a teszteket, hogy a DI konténerből építsék fel a példányokat.
4. **DocWriter** – dokumentálja az új indítási folyamatot és az injektált szolgáltatásokat az `architecture.md` és `dev_setup.md` fájlokban.
5. **Architect** – ellenőrizze a TODO és MILESTONES frissítését, valamint a régi `AppContext` teljes eltávolítását a végső lépésben.
