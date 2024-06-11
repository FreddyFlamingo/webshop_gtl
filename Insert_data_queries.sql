USE Library;

-- Insert data into Role table
INSERT INTO Role (Role_Name, Loan_Period, Grace_Period, Res_Pickup_period, Loan_max) VALUES
('Student', 21, 5, 7, 5),
('Faculty', 60, 15, 15, 20),
('Staff', 45, 7, 10, 15);


-- Insert data into Loaner table
DECLARE @i INT = 1;
DECLARE @max INT = 16000; -- Adjust this to the desired number of loaners
DECLARE @Card_Id INT;
DECLARE @Card_Expiration DATE;
DECLARE @Address NVARCHAR(100);
DECLARE @Email NVARCHAR(100);
DECLARE @Phone NVARCHAR(20);

WHILE @i <= @max
BEGIN
    -- Assign Card_Id
    SET @Card_Id = @i;

    -- Generate random Card_Expiration date (From today and 730 days in the future)
    SET @Card_Expiration = DATEADD(DAY, ABS(CHECKSUM(NEWID())) % 730, GETDATE());

    -- Assign Address
    SET @Address = 'Address ' + CAST(@Card_Id AS NVARCHAR(100));

    -- Generate Email
    SET @Email = 'user' + CAST(@Card_Id AS NVARCHAR(100)) + '@example.com';

    -- Generate Phone number in the format 'xxx-xxx-xxxx'
    SET @Phone = CAST(ABS(CHECKSUM(NEWID())) % 900 + 100 AS NVARCHAR(3)) + '-' +
                 CAST(ABS(CHECKSUM(NEWID())) % 900 + 100 AS NVARCHAR(3)) + '-' +
                 CAST(ABS(CHECKSUM(NEWID())) % 9000 + 1000 AS NVARCHAR(4));

    -- Insert into Loaner table
    INSERT INTO Loaner (Card_Id, Card_Expiration, Address, Email, Phone)
    VALUES (@Card_Id, @Card_Expiration, @Address, @Email, @Phone);

    -- Increment the counter
    SET @i = @i + 1;
END;


-- Insert data into Member table
DECLARE @i INT = 1;
DECLARE @max INT = 16000; -- Adjust this to the desired number of members
DECLARE @SSN NVARCHAR(15);
DECLARE @Card_Id INT;
DECLARE @Fname NVARCHAR(50);
DECLARE @Lname NVARCHAR(50);
DECLARE @Campus_Address NVARCHAR(100);
DECLARE @Role_Name NVARCHAR(50);

WHILE @i <= @max
BEGIN
    -- Generate random SSN in the format 'xxx-xx-xxxx'
    SET @SSN = CAST(ABS(CHECKSUM(NEWID())) % 1000 AS NVARCHAR(3)) + '-' +
               CAST(ABS(CHECKSUM(NEWID())) % 100 AS NVARCHAR(2)) + '-' +
               CAST(ABS(CHECKSUM(NEWID())) % 10000 AS NVARCHAR(4));

    -- Assign Card_Id
    SET @Card_Id = @i;

    -- Assign Fname and Lname
    SET @Fname = 'John';
    SET @Lname = CAST(@Card_Id AS NVARCHAR(50));

    -- Assign Campus_Address
    SET @Campus_Address = 'Address ' + CAST(@Card_Id AS NVARCHAR(100));

    -- Randomly select Role_Name with 90% probability for 'Student'
    SET @Role_Name = CASE 
                        WHEN ABS(CHECKSUM(NEWID())) % 10 < 9 THEN 'Student'
                        WHEN ABS(CHECKSUM(NEWID())) % 2 = 0 THEN 'Faculty'
                        ELSE 'Staff'
                     END;

    -- Insert into Member table
    INSERT INTO Member (SSN, Card_Id, Fname, Lname, Campus_Address, Role_Name)
    VALUES (@SSN, @Card_Id, @Fname, @Lname, @Campus_Address, @Role_Name);

    -- Increment the counter
    SET @i = @i + 1;
END;


-- Insert data into Item table
DECLARE @i INT = 1;
DECLARE @max INT = 100000; -- Adjust this to the desired number of items
DECLARE @Item_Id INT;
DECLARE @Publisher NVARCHAR(50);
DECLARE @Description NVARCHAR(4000);
DECLARE @Quantity INT;
DECLARE @On_Wishlist BIT;

WHILE @i <= @max
BEGIN
    -- Assign Item_Id
    SET @Item_Id = @i;

    -- Generate Publisher
    SET @Publisher = 'Publisher' + CAST(ABS(CHECKSUM(NEWID())) % 100 + 1 AS NVARCHAR(50));

    -- Generate Description
    SET @Description = 'Description of book ' + CAST(@Item_Id AS NVARCHAR(4000));

    -- Assign On_Wishlist with 1% probability
    SET @On_Wishlist = CASE WHEN ABS(CHECKSUM(NEWID())) % 100 < 1 THEN 1 ELSE 0 END;

    -- Assign Quantity
    SET @Quantity = CASE 
                      WHEN ABS(CHECKSUM(NEWID())) % 100 < 95 THEN ABS(CHECKSUM(NEWID())) % 3 + 1
                      ELSE ABS(CHECKSUM(NEWID())) % 16 + 5 
                    END;

    -- Insert into Item table
    INSERT INTO Item (Item_Id, Publisher, Description, Quantity, On_Wishlist)
    VALUES (@Item_Id, @Publisher, @Description, @Quantity, @On_Wishlist);

    -- Increment the counter
    SET @i = @i + 1;
END;


-- Insert data into Book table
DECLARE @i INT = 1;
DECLARE @max INT = 100000; -- Adjust this to the desired number of books
DECLARE @Item_Id INT;
DECLARE @ISBN NVARCHAR(20);
DECLARE @Title NVARCHAR(100);

WHILE @i <= @max
BEGIN
    -- Assign Item_Id
    SET @Item_Id = @i;

    -- Generate ISBN in the format 'xxxxxxxxx-x'
    SET @ISBN = CAST(ABS(CHECKSUM(NEWID())) % 1000000000 AS NVARCHAR(9)) + '-' + CAST(ABS(CHECKSUM(NEWID())) % 10 AS NVARCHAR(1));

    -- Generate Title
    SET @Title = 'Title ' + CAST(@Item_Id AS NVARCHAR(100));

    -- Insert into Book table
    INSERT INTO Book (Item_Id, ISBN, Title)
    VALUES (@Item_Id, @ISBN, @Title);

    -- Increment the counter
    SET @i = @i + 1;
END;


-- Insert data into LocalBook table
DECLARE @i INT = 1;
DECLARE @max INT = 100000; -- Adjust this to the desired number of local books
DECLARE @Item_Id INT;
DECLARE @Loanable BIT;
DECLARE @Shelf_Location NVARCHAR(100);

WHILE @i <= @max
BEGIN
    -- Assign Item_Id
    SET @Item_Id = @i;

    -- Assign Loanable with 1% probability of being 0 and 99% probability of being 1
    SET @Loanable = CASE WHEN ABS(CHECKSUM(NEWID())) % 100 < 99 THEN 1 ELSE 0 END;

    -- Generate Shelf_Location
    SET @Shelf_Location = 'Shelf ' + CAST(@Item_Id AS NVARCHAR(100));

    -- Insert into LocalBook table
    INSERT INTO LocalBook (Item_Id, Loanable, Shelf_Location)
    VALUES (@Item_Id, @Loanable, @Shelf_Location);

    -- Increment the counter
    SET @i = @i + 1;
END;


-- Insert data into Loan table
-- Drop the temporary table if it already exists
IF OBJECT_ID('tempdb..#ActiveLoans') IS NOT NULL
BEGIN
    DROP TABLE #ActiveLoans;
END;
GO

-- Create a script for generating loans
DECLARE @i INT = 1;
DECLARE @max INT = 20000; -- Adjust this to the desired number of loans
DECLARE @Loan_Date DATE;
DECLARE @Item_Id INT;
DECLARE @Card_Id INT;
DECLARE @Loaned_Quantity INT;
DECLARE @Returned_Quantity INT;
DECLARE @Returned_Date DATE;
DECLARE @Role_Name NVARCHAR(50);
DECLARE @Loan_Max INT;
DECLARE @Active_Loan_Sum INT;
DECLARE @RandomDays INT;

-- Get random loan date between 2023-01-01 and 2024-05-20
DECLARE @StartDate DATE = '2023-01-01';
DECLARE @EndDate DATE = '2024-05-20';

-- Temporary table to store active loan quantities per Card_Id
CREATE TABLE #ActiveLoans (
    Card_Id INT,
    ActiveLoanQuantity INT
);

-- Initialize the temporary table
INSERT INTO #ActiveLoans (Card_Id, ActiveLoanQuantity)
SELECT Card_Id, 0 FROM Loaner;

-- Function to get a random date between two dates
DECLARE @DaysBetween INT = DATEDIFF(DAY, @StartDate, @EndDate);

-- Loop to generate loan records
WHILE @i <= @max
BEGIN
    -- Get a random loan date
    SET @Loan_Date = DATEADD(DAY, ABS(CHECKSUM(NEWID())) % @DaysBetween, @StartDate);

    -- Get a random Item_Id
    SELECT TOP 1 @Item_Id = Item_Id FROM Item ORDER BY NEWID();

    -- Get a random Card_Id
    SELECT TOP 1 @Card_Id = Card_Id FROM Loaner ORDER BY NEWID();

    -- Get the Role_Name and Loan_Max for the selected Card_Id
    SELECT @Role_Name = Role_Name FROM Member WHERE Card_Id = @Card_Id;
    SELECT @Loan_Max = Loan_Max FROM Role WHERE Role_Name = @Role_Name;

    -- Get the current active loan sum for the selected Card_Id
    SELECT @Active_Loan_Sum = ActiveLoanQuantity FROM #ActiveLoans WHERE Card_Id = @Card_Id;

    -- Generate a random loaned quantity
    SET @Loaned_Quantity = ABS(CHECKSUM(NEWID())) % 5 + 1;

    -- Ensure the active loan sum does not exceed Loan_Max
    IF @Active_Loan_Sum + @Loaned_Quantity <= @Loan_Max
    BEGIN
        -- Generate a random returned quantity based on the loaned quantity and the desired return rate
        SET @Returned_Quantity = CASE WHEN ABS(CHECKSUM(NEWID())) % 100 < 97 THEN @Loaned_Quantity ELSE ABS(CHECKSUM(NEWID())) % @Loaned_Quantity END;

        -- Generate a random number of days for the returned date if the loan is fully returned
        IF @Loaned_Quantity = @Returned_Quantity
        BEGIN
            SET @RandomDays = CASE ABS(CHECKSUM(NEWID())) % 13
                                 WHEN 0 THEN 7
                                 WHEN 1 THEN 8
                                 WHEN 2 THEN 9
                                 WHEN 3 THEN 10
                                 WHEN 4 THEN 11
                                 WHEN 5 THEN 12
                                 WHEN 6 THEN 13
                                 WHEN 7 THEN 14
                                 WHEN 8 THEN 15
                                 WHEN 9 THEN 22
                                 WHEN 10 THEN 32
                                 WHEN 11 THEN 50
                                 ELSE 70
                             END;
            SET @Returned_Date = DATEADD(DAY, @RandomDays, @Loan_Date);
        END
        ELSE
        BEGIN
            SET @Returned_Date = '1900-01-01';
        END

        -- Insert the loan record
        INSERT INTO Loan (Loan_Date, Item_Id, Card_Id, Returned_Date, Loaned_Quantity, Returned_Quantity)
        VALUES (@Loan_Date, @Item_Id, @Card_Id, @Returned_Date, @Loaned_Quantity, @Returned_Quantity);

        -- Update the active loan sum for the Card_Id
        UPDATE #ActiveLoans
        SET ActiveLoanQuantity = ActiveLoanQuantity + (@Loaned_Quantity - @Returned_Quantity)
        WHERE Card_Id = @Card_Id;

        -- Increment the counter
        SET @i = @i + 1;
    END;
END;

-- Cleanup
DROP TABLE #ActiveLoans;



-- CURRENTLY NOT IMPLEMENTED:
-- Map
-- ExternalBook
-- Author
-- Catagory
-- Reservation
-- (TechLib)