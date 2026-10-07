/* =====================================================================
   PART 2 : QUERIES OF TABLES (create table + sample data)
   NOTE: this script DROPS and RECREATES dbo.Products so the structure is
         always correct. Existing rows in Products are replaced by sample data.
   ===================================================================== */

USE InventoryDB;
GO

DROP TABLE IF EXISTS dbo.Products;
GO

CREATE TABLE dbo.Products
(
    ProductId    INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Products PRIMARY KEY,
    ProductName  NVARCHAR(100)  NOT NULL,
    Category     NVARCHAR(50)   NOT NULL,
    Price        DECIMAL(18,2)  NOT NULL CONSTRAINT CK_Products_Price    CHECK (Price > 0),
    Quantity     INT            NOT NULL CONSTRAINT CK_Products_Quantity CHECK (Quantity >= 0),
    Supplier     NVARCHAR(100)  NOT NULL
);
GO

INSERT INTO dbo.Products (ProductName, Category, Price, Quantity, Supplier) VALUES
(N'Wireless Mouse',       N'Electronics',  799.00,  25,  N'Tech World'),
(N'Mechanical Keyboard',  N'Electronics', 2499.00,  15,  N'Computer Hub'),
(N'Notebook',             N'Stationery',    80.00, 100,  N'ABC Stationers'),
(N'Office Chair',         N'Furniture',   5499.00,  10,  N'Comfort Furniture'),
(N'USB-C Cable',          N'Accessories',  399.00,  50,  N'Digital Store');
GO

-- Check: should show 5 rows
SELECT * FROM dbo.Products;
GO
