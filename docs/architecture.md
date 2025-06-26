# Architecture Overview

Wrecept használata során a legfontosabb entitások az alábbiak:

- **Invoice** – számlák fejlécadatai és tételei.
- **InvoiceItem** – egy számla termék- vagy szolgáltatás sorát írja le.
- **Supplier** – a számla kiállítója vagy vevője.
- **Product** és **ProductGroup** – rögzített termékek és csoportjaik.
- **Unit** – mennyiségi egységek megnevezése.
- **PaymentMethod** és **TaxRate** – fizetési és ÁFA adatok.

Ezek a típusok a `Wrecept.Core.CoreLib` projekt `Domain` mappájában
`record class` formában találhatók. A szolgáltatások és tárolók ezekre
épülnek, így a modell egy helyen, bővíthetően karbantartható.
