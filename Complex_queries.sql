-- Query 1: How many of an Item is still available / on the shelf
SELECT item_id, quantity, (quantity - out_qty) AS available_qty
FROM (
    SELECT Item.item_id, Item.quantity, COALESCE(subquery.out_qty, 0) AS out_qty
    FROM (
        SELECT item_id, quantity
        FROM Item
        WHERE item_id = 510
    ) AS Item
    LEFT JOIN (
        SELECT item_id, SUM(loaned_quantity) - SUM(returned_quantity) AS out_qty
        FROM (
            SELECT item_id, loaned_quantity, returned_quantity
            FROM Loan
            WHERE item_id = 1 AND loaned_quantity > returned_quantity
        ) AS subquery
        GROUP BY item_id
    ) AS subquery ON Item.item_id = subquery.item_id
) AS Result;


-- Query 2: Find description for en specifik bog 
SELECT description
FROM Item
WHERE item_id = 1;


-- Query 3: Find Items that is overdue its loan period
-- Todo: Add expected return date and missing quantity
-- Techlib:
SELECT T3.loan_date, T3.item_id, T3.card_id, T3.loan_period, T3.university
FROM (
    SELECT T1.loan_date, T1.item_id, T1.card_id, T2.loan_period, T2.university
    FROM (
        SELECT loan_date, item_id, card_id
        FROM Loan
        WHERE returned_date = '1900-01-01'
    ) AS T1
    JOIN (
        SELECT loan_period, card_id, university
        FROM TechLib
    ) AS T2 ON T1.card_id = T2.card_id
) AS T3
WHERE DATEADD(day, T3.loan_period, T3.loan_date) < GETDATE();

-- Member:
SELECT T5.loan_date, T5.item_id, T5.card_id, T5.fname, T5.lname, T5.role_name, T5.email
FROM (
    SELECT T1.loan_date, T1.item_id, T1.card_id, T4.fname, T4.lname, T4.role_name, T4.email, T4.loan_period
    FROM (
        SELECT loan_date, item_id, card_id
        FROM Loan
        WHERE returned_date = '1900-01-01'
    ) AS T1
    JOIN (
        SELECT T3.card_id, T3.fname, T3.lname, T3.role_name, T3.loan_period, Loaner.email
        FROM (
            SELECT T2.card_id, T2.fname, T2.lname, T2.role_name, Role.loan_period
            FROM (
                SELECT card_id, fname, lname, role_name
                FROM Member
            ) AS T2
            JOIN Role ON T2.role_name = Role.role_name
        ) AS T3
        JOIN Loaner ON T3.card_id = Loaner.card_id
    ) AS T4 ON T1.card_id = T4.card_id
) AS T5
WHERE DATEADD(day, T5.loan_period, T5.loan_date) < GETDATE();


-- Query 4: Find most active members ( x or more loan days) this year
-- Active loans is also counted
SELECT card_id, loan_days
FROM (
    SELECT card_id, COUNT(DISTINCT loan_date) AS loan_days
    FROM (
        SELECT loan_date, card_id, item_id
        FROM Loan
        WHERE loan_date > DATEFROMPARTS(YEAR(GETDATE()), 1, 1)
    ) AS T1
    GROUP BY card_id
) AS T2
WHERE loan_days >= 1;


-- Query 5: Find inactive members (never made loans)
SELECT card_id, fname, lname
FROM Member
WHERE card_id NOT IN (SELECT DISTINCT card_id FROM Loan);

-- Query 5a: Find inactive members (no loans made this year)
SELECT card_id, fname, lname
FROM Member
WHERE card_id NOT IN (
    SELECT DISTINCT card_id
    FROM Loan
    WHERE loan_date >= DATEFROMPARTS(YEAR(GETDATE()), 1, 1)
);


-- Query 6: Calculate the percentage of the quantity of total (completed) loans from the top percentile of members this year
-- Note: Floors the values -> not enough memebers might return null value
DECLARE @input_value INT = 1;

WITH LoansThisYear AS (
    SELECT card_id, SUM(loaned_quantity) AS n_loans
    FROM Loan
    WHERE loan_date >= DATEFROMPARTS(YEAR(GETDATE()), 1, 1)
      AND Returned_Date != '1900-01-01'
    GROUP BY card_id
),
MemberLoans AS (
    SELECT L.card_id, L.n_loans, M.fname, M.lname, 
           ROW_NUMBER() OVER (ORDER BY L.n_loans DESC) AS row_num
    FROM LoansThisYear AS L
    JOIN Member AS M ON L.card_id = M.card_id
),
TopPercentile AS (
    SELECT card_id, n_loans
    FROM MemberLoans
    WHERE row_num <= (
        SELECT CAST((COUNT(*) / 100.0) * @input_value AS INT)
        FROM Member
    )
),
TotalLoansOfPercentile AS (
    SELECT SUM(n_loans) AS total_n_loans
    FROM TopPercentile
),
TotalQuantityOfItemsLoaned AS (
    SELECT SUM(loaned_quantity) AS total_quantity
    FROM Loan
    WHERE loan_date >= DATEFROMPARTS(YEAR(GETDATE()), 1, 1)
      AND Returned_Date != '1900-01-01'
)
SELECT CAST((TotalLoansOfPercentile.total_n_loans / CAST(TotalQuantityOfItemsLoaned.total_quantity AS DECIMAL)) * 100.0 AS DECIMAL) AS loan_percentage, 
       total_n_loans
FROM TotalLoansOfPercentile, TotalQuantityOfItemsLoaned;