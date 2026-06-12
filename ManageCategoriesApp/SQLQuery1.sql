-- =========================================
-- 1. T?O DATABASE
-- =========================================
-- Xóa database n?u ?ã t?n t?i (?? ch?y l?i không b? l?i)
IF DB_ID('Mystore') IS NOT NULL
BEGIN
    ALTER DATABASE Mystore SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE Mystore;
END
GO

CREATE DATABASE Mystore;
GO

USE Mystore;
GO

-- =========================================
-- 2. T?O CÁC B?NG (TABLES)
-- =========================================

-- B?ng Danh m?c (Categories)
CREATE TABLE Categories (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX) NULL
);
GO

-- B?ng Khách hàng (Customers)
CREATE TABLE Customers (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Email VARCHAR(100) NULL,
    Phone VARCHAR(20) NULL,
    Address NVARCHAR(255) NULL
);
GO

-- B?ng S?n ph?m (Products)
CREATE TABLE Products (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    CategoryID INT NOT NULL,
    ProductName NVARCHAR(150) NOT NULL,
    Price DECIMAL(18, 2) NOT NULL DEFAULT 0,
    Stock INT NOT NULL DEFAULT 0,
    -- T?o khóa ngo?i liên k?t v?i b?ng Categories
    FOREIGN KEY (CategoryID) REFERENCES Categories(ID) ON DELETE CASCADE
);
GO

-- B?ng ??n hàng (Orders)
CREATE TABLE Orders (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    CustomerID INT NOT NULL,
    OrderDate DATETIME DEFAULT GETDATE(),
    TotalAmount DECIMAL(18, 2) NOT NULL DEFAULT 0,
    Status NVARCHAR(50) DEFAULT N'Ch? x? lý',
    -- T?o khóa ngo?i liên k?t v?i b?ng Customers
    FOREIGN KEY (CustomerID) REFERENCES Customers(ID) ON DELETE CASCADE
);
GO

-- B?ng Chi ti?t ??n hàng (OrderDetails)
CREATE TABLE OrderDetails (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    OrderID INT NOT NULL,
    ProductID INT NOT NULL,
    Quantity INT NOT NULL DEFAULT 1,
    UnitPrice DECIMAL(18, 2) NOT NULL,
    -- T?o khóa ngo?i liên k?t v?i b?ng Orders và Products
    FOREIGN KEY (OrderID) REFERENCES Orders(ID) ON DELETE CASCADE,
    FOREIGN KEY (ProductID) REFERENCES Products(ID)
);
GO

-- =========================================
-- 3. THÊM D? LI?U M?U (DUMMY DATA)
-- =========================================

-- Thêm Danh m?c
INSERT INTO Categories (CategoryName, Description)
VALUES 
(N'?i?n tho?i', N'Các dòng smartphone m?i nh?t'),
(N'Laptop', N'Máy tính xách tay v?n phòng và gaming');
GO

-- Thêm S?n ph?m
INSERT INTO Products (CategoryID, ProductName, Price, Stock)
VALUES 
(1, N'iPhone 15 Pro Max', 29000000, 50),
(2, N'MacBook Air M2', 25000000, 30);
GO