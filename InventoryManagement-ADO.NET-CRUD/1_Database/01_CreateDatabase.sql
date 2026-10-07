/* =====================================================================
   PART 1 : DATABASE CREATION (SQL Server)
   Run in: SSMS  OR  Visual Studio > View > SQL Server Object Explorer
           (right-click your server > New Query)
   ===================================================================== */

USE master;
GO

IF DB_ID(N'InventoryDB') IS NULL
BEGIN
    CREATE DATABASE InventoryDB;
    PRINT 'Database InventoryDB created.';
END
ELSE
    PRINT 'Database InventoryDB already exists.';
GO

USE InventoryDB;
GO
