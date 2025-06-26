# Architecture Overview

Wrecept használata során a legfontosabb entitások az alábbiak:

- **Invoice** – fejlécadatok (sorszám, dátum, fizetési mód) és a hozzátartozó tételek listája.
- **InvoiceItem** – egy számla termék- vagy szolgáltatás sorát írja le.
- **Supplier** – név, cím és adószám; kiállító vagy vevő szerepben használható.
- **Product** és **ProductGroup** – terméktörzs elemei csoportokba rendezve.
- **Unit** – mennyiségi egységek megnevezése.
- **PaymentMethod** és **TaxRate** – fizetési és ÁFA adatok.

Ezek a típusok a `Wrecept.Core.CoreLib` projekt `Domain` mappájában
`record class` formában találhatók. A szolgáltatások és tárolók ezekre
épülnek, így a modell egy helyen, bővíthetően karbantartható.
