CREATE TABLE IF NOT EXISTS Suppliers (
    Id TEXT PRIMARY KEY,
    Name TEXT NOT NULL,
    Address TEXT NOT NULL,
    TaxId TEXT NOT NULL,
    BankAccountNumber TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS ProductGroups (
    Id TEXT PRIMARY KEY,
    Name TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS TaxRates (
    Id TEXT PRIMARY KEY,
    Label TEXT NOT NULL,
    Percentage REAL NOT NULL
);

CREATE TABLE IF NOT EXISTS Units (
    Id TEXT PRIMARY KEY,
    Name TEXT NOT NULL,
    Symbol TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS PaymentMethods (
    Id TEXT PRIMARY KEY,
    Label TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS Products (
    Id TEXT PRIMARY KEY,
    Name TEXT NOT NULL,
    ProductGroupId TEXT NOT NULL REFERENCES ProductGroups(Id),
    TaxRateId TEXT NOT NULL REFERENCES TaxRates(Id),
    DefaultUnitId TEXT NOT NULL REFERENCES Units(Id)
);

CREATE TABLE IF NOT EXISTS Invoices (
    Id TEXT PRIMARY KEY,
    SerialNumber TEXT NOT NULL,
    IssueDate TEXT NOT NULL,
    SupplierId TEXT NOT NULL REFERENCES Suppliers(Id),
    PaymentMethodId TEXT NOT NULL REFERENCES PaymentMethods(Id),
    Notes TEXT
);

CREATE TABLE IF NOT EXISTS InvoiceItems (
    Id TEXT PRIMARY KEY,
    InvoiceId TEXT NOT NULL REFERENCES Invoices(Id),
    ProductId TEXT NOT NULL REFERENCES Products(Id),
    Quantity REAL NOT NULL,
    UnitId TEXT NOT NULL REFERENCES Units(Id),
    UnitPriceNet REAL NOT NULL,
    VatRatePercent REAL NOT NULL
);
