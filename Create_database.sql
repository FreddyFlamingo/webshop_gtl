CREATE DATABASE Library;
GO

USE Library;


CREATE TABLE Role (
    Role_Name NVARCHAR(50) PRIMARY KEY,
    Loan_Period INT,
    Grace_Period INT,
    Res_Pickup_Period INT,
    Loan_Max INT
);


CREATE TABLE Loaner (
    Card_Id INT PRIMARY KEY,
    Card_Expiration DATE,
    Address NVARCHAR(100),
    Email NVARCHAR(100),
    Phone NVARCHAR(20),
);

CREATE TABLE Member (
    SSN NVARCHAR(15) PRIMARY KEY,
    Card_Id INT,
    Fname NVARCHAR(50),
    Lname NVARCHAR(50),
    Campus_Address NVARCHAR(100),
    Role_Name NVARCHAR(50),
    FOREIGN KEY (Card_Id) REFERENCES Loaner(Card_Id),
    FOREIGN KEY (Role_Name) REFERENCES Role(Role_Name)
);

CREATE TABLE TechLib (
	University NVARCHAR(50) PRIMARY KEY,
    Card_Id INT,
    Loan_Agreement NVARCHAR(4000),
    Loan_Period INT,
    Grace_Period INT,
    Res_Pickup_period INT
	FOREIGN KEY (Card_Id) REFERENCES Loaner(Card_Id)
);

CREATE TABLE Item (
    Item_Id INT PRIMARY KEY,
    Publisher NVARCHAR(50),
    Description NVARCHAR(4000),
    Quantity INT,
    On_Wishlist BIT
);

CREATE TABLE Reservation (
    From_Date DATE,
    Item_Id INT,
    Card_Id INT,
    PRIMARY KEY (From_Date, Item_Id, Card_Id),
    FOREIGN KEY (Item_Id) REFERENCES Item(Item_Id),
    FOREIGN KEY (Card_Id) REFERENCES Loaner(Card_Id)
);

CREATE TABLE Loan (
    Loan_Date DATE,
    Item_Id INT,
    Card_Id INT,
    Returned_Date DATE DEFAULT '1900-01-01',
    Loaned_Quantity INT,
    Returned_Quantity INT,
    PRIMARY KEY (Loan_Date, Item_Id, Card_Id),
    FOREIGN KEY (Item_Id) REFERENCES Item(Item_Id),
    FOREIGN KEY (Card_Id) REFERENCES Loaner(Card_Id),
);

CREATE TABLE Map (
    Item_Id INT PRIMARY KEY,
    Type NVARCHAR(50),
    Area NVARCHAR(50),
    FOREIGN KEY (Item_Id) REFERENCES Item(Item_Id)
);

CREATE TABLE Book (
    Item_Id INT PRIMARY KEY,
    ISBN NVARCHAR(20) UNIQUE,
    Title NVARCHAR(100),
    FOREIGN KEY (Item_Id) REFERENCES Item(Item_Id)
);

CREATE TABLE ExternalBook (
    Item_Id INT PRIMARY KEY,
    Owner_Lib NVARCHAR(100),
    FOREIGN KEY (Item_Id) REFERENCES Book(Item_Id)
);

CREATE TABLE LocalBook (
    Item_Id INT PRIMARY KEY,
    Loanable BIT,
    Shelf_Location NVARCHAR(100),
    FOREIGN KEY (Item_Id) REFERENCES Book(Item_Id)
);

CREATE TABLE Author (
    Author_Name NVARCHAR(100),
    ISBN NVARCHAR(20),
    PRIMARY KEY (Author_Name, ISBN),
    FOREIGN KEY (ISBN) REFERENCES Book(ISBN)
);

CREATE TABLE Category (
    Category_Name NVARCHAR(50),
    ISBN NVARCHAR(20),
    PRIMARY KEY (Category_Name, ISBN),
    FOREIGN KEY (ISBN) REFERENCES Book(ISBN)
);
