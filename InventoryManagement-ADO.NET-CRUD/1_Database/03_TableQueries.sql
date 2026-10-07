/* =====================================================================
   PART 2 (continued) : QUERIES USED ON THE Products TABLE
   These are the same statements the C# code runs in 3_CRUD.
   ===================================================================== */

USE InventoryDB;
GO

-- CREATE  : add a product
INSERT INTO dbo.Products (ProductName, Category, Price, Quantity, Supplier)
VALUES (N'Laptop Stand', N'Accessories', 1299.00, 20, N'Digital Store');

-- READ    : all products
SELECT ProductId, ProductName, Category, Price, Quantity, Supplier
FROM   dbo.Products
ORDER  BY ProductId DESC;

-- READ    : one product by id
SELECT ProductId, ProductName, Category, Price, Quantity, Supplier
FROM   dbo.Products
WHERE  ProductId = 1;

-- READ    : search by name / category / supplier
SELECT ProductId, ProductName, Category, Price, Quantity, Supplier
FROM   dbo.Products
WHERE  ProductName LIKE '%mouse%' OR Category LIKE '%mouse%' OR Supplier LIKE '%mouse%';

-- UPDATE  : edit a product
UPDATE dbo.Products
SET    ProductName = N'Wireless Mouse Pro', Price = 899.00, Quantity = 30
WHERE  ProductId = 1;

-- DELETE  : remove a product
DELETE FROM dbo.Products WHERE ProductId = 6;

-- REPORTS : useful inventory queries
SELECT * FROM dbo.Products WHERE Quantity < 10;                              -- low stock
SELECT Category, COUNT(*) AS Items, SUM(Quantity) AS TotalUnits,
       SUM(Price * Quantity) AS StockValue
FROM   dbo.Products GROUP BY Category;                                       -- per-category summary
GO
