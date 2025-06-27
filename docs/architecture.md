# Architecture Overview

Wrecept használata során a legfontosabb entitások az alábbiak:

- **Invoice** – fejlécadatok (sorszám, dátum, fizetési mód, számítás módja) és a hozzátartozó tételek listája.
- **InvoiceItem** – egy számla termék- vagy szolgáltatás sorát írja le.
- **Supplier** – név, cím és adószám; kiállító vagy vevő szerepben használható.
- **Product** és **ProductGroup** – terméktörzs elemei csoportokba rendezve.
- **Unit** – mennyiségi egységek megnevezése.
- **PaymentMethod** és **TaxRate** – fizetési és ÁFA adatok.

A fizetési módok listáját az `IPaymentMethodService` tölti be futásidőben, és az
`InvoiceHeaderViewModel` automatikusan frissíti a ComboBox adatait.

Ezek a típusok a `Wrecept.Core.CoreLib` projekt `Domain` mappájában
`record class` formában találhatók. A szolgáltatások és tárolók ezekre
épülnek, így a modell egy helyen, bővíthetően karbantartható.

## InlineCreator komponens
Az `InvoiceItemsGrid` speciális esete, amikor a beírt terméknév még nem létezik.
Ilyenkor egy újrahasználható **InlineCreator** jelenik meg soron belül, ahol a
kiegészítő mezőket Enterrel menthetjük, Esc kilép. A komponens ViewModelje
generikus, így később beszállító vagy fizetési mód rögzítésére is alkalmazható.

## Inline Lookup
Az **LookupBox** komponens teszi lehetővé, hogy a név mezők alatt azonnal
megjelenjen egy szűrhető találati lista. A fókusz megérkezése automatikusan
kinyitja a listát, a fel/le nyilak lépkednek az elemek között, `Enter` elfogad,
`Esc` bezár. A vezérlő az invoice szerkesztő fej- és tételrészeiben használatos,
közvetlenül a keresőmezők alatt jelenik meg.

## Hungarian Number Converter
A `HungarianNumberConverter` osztály a pénzösszegek szöveges kiírását végzi. A `GrandTotal.AmountText` ezen keresztül adja vissza a számla végösszegét magyarul.

## Input Locking
Az `Infrastructure.AppContext.InputLocked` jelző megakadályozza a párbeszédablakok közbeni billentyűfeldolgozást. A `NavigationService` `InputLockScope` osztállyal vezérli ezt, amely `IDisposable`-ként automatikusan visszaállítja a jelzőt kivétel esetén is.

## Keyboard Bindings
A master data nézetek gyorsbillentyűit `InputBindings` köti össze a ViewModelben definiált parancsokkal. Így az Insert, F2 és Delete gombok közvetlenül az `AddCommand`, `SaveCommand` és `DeleteCommand` műveleteket hívják meg, míg az Esc továbbra is a `NavigationService` segítségével zárja be az aktuális ablakot.

## Feedback Service
A new `IFeedbackService` centralises sound cues. The default `FeedbackService` plays short beep patterns (startup, exit, accept, reject, error) via `Console.Beep`. `VisualFeedback` helpers flash controls in warning, error or success colours. The service is registered in the DI container for global access.

## Persistence Layer
Data is stored in a local SQLite file accessed through **Entity Framework Core**. A single `WreceptDbContext` maps the domain entities to tables and is created via the DI container on startup. When the database is unavailable or corrupt, in-memory repositories provide a fallback.

## Dependency Injection
Az alkalmazás minden szolgáltatását a `Microsoft.Extensions.DependencyInjection` konténer kezeli. A ViewModel-ek konstruktoron keresztül jutnak a szükséges függőségekhez, így a statikus `AppContext` szerepe megszűnt. A tesztek saját szolgáltatásgyűjteményt építenek fel, így a komponensek könnyen izolálhatók.
