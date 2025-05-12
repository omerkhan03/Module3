Use TravelEase

/*
-- Insert a new user (will trigger audit log)
INSERT INTO [User] (username, password, email, role)
VALUES ('testuser', 'hashedpass123', 'test@email.com', 'traveler');

-- Update a user (will trigger audit log)
UPDATE [User]
SET phone_number = '555-0123'
WHERE username = 'testuser';

-- Insert a new traveler (will trigger audit log)
INSERT INTO Traveler (user_id, first_name, last_name)
VALUES ((SELECT user_id FROM [User] WHERE username = 'testuser'), 'John', 'Doe');

-- Update a booking status (will trigger audit log)
UPDATE Booking
SET status = 'Cancelled', 
    is_cancelled = 1,
    cancellation_reason = 'Change of plans'
WHERE booking_id = 1;

-- Insert a new payment (will trigger audit log)
INSERT INTO Payment (booking_id, amount, payment_method, status)
VALUES (1, 299.99, 'Credit Card', 'Completed');

-- Check the audit log to see what was recorded
SELECT * FROM AuditLog ORDER BY action_date DESC;

-- Test TourOperator trigger
INSERT INTO TourOperator (user_id, company_name, description)
VALUES (1, 'New Tour Co', 'Test description');

-- Test Review trigger
INSERT INTO Review (traveler_id, trip_id, rating, comment)
VALUES (1, 1, 5, 'Great experience!');

-- Check the audit log
SELECT * FROM AuditLog ORDER BY action_date DESC;

---------------------------------------------------------
*/
-- Create AuditLog table
CREATE TABLE AuditLog (
    log_id INT IDENTITY(1,1) PRIMARY KEY,
    table_name VARCHAR(100) NOT NULL,
    action_type VARCHAR(10) NOT NULL, -- 'INSERT', 'UPDATE', 'DELETE'
    record_id VARCHAR(50) NOT NULL,   -- Primary key of the affected record
    action_date DATETIME NOT NULL DEFAULT GETDATE(),
    user_name VARCHAR(100) NOT NULL DEFAULT SYSTEM_USER
);

select * from AuditLog

-- Trigger for User table
CREATE OR ALTER TRIGGER tr_User_Audit
ON [User]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @action_type VARCHAR(10)
    
    IF EXISTS (SELECT * FROM INSERTED) AND EXISTS (SELECT * FROM DELETED)
        SET @action_type = 'UPDATE'
    ELSE IF EXISTS (SELECT * FROM INSERTED)
        SET @action_type = 'INSERT'
    ELSE
        SET @action_type = 'DELETE'
    
    INSERT INTO AuditLog (table_name, action_type, record_id)
    SELECT 
        'User',
        @action_type,
        CASE 
            WHEN @action_type = 'DELETE' THEN CAST(d.user_id AS VARCHAR)
            ELSE CAST(i.user_id AS VARCHAR)
        END
    FROM INSERTED i
    FULL OUTER JOIN DELETED d ON i.user_id = d.user_id;
END;

-- Trigger for Traveler table
CREATE OR ALTER TRIGGER tr_Traveler_Audit
ON Traveler
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @action_type VARCHAR(10)
    
    IF EXISTS (SELECT * FROM INSERTED) AND EXISTS (SELECT * FROM DELETED)
        SET @action_type = 'UPDATE'
    ELSE IF EXISTS (SELECT * FROM INSERTED)
        SET @action_type = 'INSERT'
    ELSE
        SET @action_type = 'DELETE'
    
    INSERT INTO AuditLog (table_name, action_type, record_id)
    SELECT 
        'Traveler',
        @action_type,
        CASE 
            WHEN @action_type = 'DELETE' THEN CAST(d.traveler_id AS VARCHAR)
            ELSE CAST(i.traveler_id AS VARCHAR)
        END
    FROM INSERTED i
    FULL OUTER JOIN DELETED d ON i.traveler_id = d.traveler_id;
END;

-- Trigger for Booking table
CREATE OR ALTER TRIGGER tr_Booking_Audit
ON Booking
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @action_type VARCHAR(10)
    
    IF EXISTS (SELECT * FROM INSERTED) AND EXISTS (SELECT * FROM DELETED)
        SET @action_type = 'UPDATE'
    ELSE IF EXISTS (SELECT * FROM INSERTED)
        SET @action_type = 'INSERT'
    ELSE
        SET @action_type = 'DELETE'
    
    INSERT INTO AuditLog (table_name, action_type, record_id)
    SELECT 
        'Booking',
        @action_type,
        CASE 
            WHEN @action_type = 'DELETE' THEN CAST(d.booking_id AS VARCHAR)
            ELSE CAST(i.booking_id AS VARCHAR)
        END
    FROM INSERTED i
    FULL OUTER JOIN DELETED d ON i.booking_id = d.booking_id;
END;


-- Trigger for Payment table
CREATE OR ALTER TRIGGER tr_Payment_Audit
ON Payment
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @action_type VARCHAR(10)
    
    IF EXISTS (SELECT * FROM INSERTED) AND EXISTS (SELECT * FROM DELETED)
        SET @action_type = 'UPDATE'
    ELSE IF EXISTS (SELECT * FROM INSERTED)
        SET @action_type = 'INSERT'
    ELSE
        SET @action_type = 'DELETE'
    
    INSERT INTO AuditLog (table_name, action_type, record_id)
    SELECT 
        'Payment',
        @action_type,
        CASE 
            WHEN @action_type = 'DELETE' THEN CAST(d.payment_id AS VARCHAR)
            ELSE CAST(i.payment_id AS VARCHAR)
        END
    FROM INSERTED i
    FULL OUTER JOIN DELETED d ON i.payment_id = d.payment_id;
END;


-- Trigger for TourOperator table
CREATE OR ALTER TRIGGER tr_TourOperator_Audit
ON TourOperator
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @action_type VARCHAR(10)
    
    IF EXISTS (SELECT * FROM INSERTED) AND EXISTS (SELECT * FROM DELETED)
        SET @action_type = 'UPDATE'
    ELSE IF EXISTS (SELECT * FROM INSERTED)
        SET @action_type = 'INSERT'
    ELSE
        SET @action_type = 'DELETE'
    
    INSERT INTO AuditLog (table_name, action_type, record_id)
    SELECT 
        'TourOperator',
        @action_type,
        CASE 
            WHEN @action_type = 'DELETE' THEN CAST(d.operator_id AS VARCHAR)
            ELSE CAST(i.operator_id AS VARCHAR)
        END
    FROM INSERTED i
    FULL OUTER JOIN DELETED d ON i.operator_id = d.operator_id;
END;

-- Trigger for ServiceProvider table
CREATE OR ALTER TRIGGER tr_ServiceProvider_Audit
ON ServiceProvider
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @action_type VARCHAR(10)
    
    IF EXISTS (SELECT * FROM INSERTED) AND EXISTS (SELECT * FROM DELETED)
        SET @action_type = 'UPDATE'
    ELSE IF EXISTS (SELECT * FROM INSERTED)
        SET @action_type = 'INSERT'
    ELSE
        SET @action_type = 'DELETE'
    
    INSERT INTO AuditLog (table_name, action_type, record_id)
    SELECT 
        'ServiceProvider',
        @action_type,
        CASE 
            WHEN @action_type = 'DELETE' THEN CAST(d.provider_id AS VARCHAR)
            ELSE CAST(i.provider_id AS VARCHAR)
        END
    FROM INSERTED i
    FULL OUTER JOIN DELETED d ON i.provider_id = d.provider_id;
END;

-- Trigger for Hotel table
CREATE OR ALTER TRIGGER tr_Hotel_Audit
ON Hotel
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @action_type VARCHAR(10)
    
    IF EXISTS (SELECT * FROM INSERTED) AND EXISTS (SELECT * FROM DELETED)
        SET @action_type = 'UPDATE'
    ELSE IF EXISTS (SELECT * FROM INSERTED)
        SET @action_type = 'INSERT'
    ELSE
        SET @action_type = 'DELETE'
    
    INSERT INTO AuditLog (table_name, action_type, record_id)
    SELECT 
        'Hotel',
        @action_type,
        CASE 
            WHEN @action_type = 'DELETE' THEN CAST(d.hotel_id AS VARCHAR)
            ELSE CAST(i.hotel_id AS VARCHAR)
        END
    FROM INSERTED i
    FULL OUTER JOIN DELETED d ON i.hotel_id = d.hotel_id;
END;

-- Trigger for TransportService table
CREATE OR ALTER TRIGGER tr_TransportService_Audit
ON TransportService
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @action_type VARCHAR(10)
    
    IF EXISTS (SELECT * FROM INSERTED) AND EXISTS (SELECT * FROM DELETED)
        SET @action_type = 'UPDATE'
    ELSE IF EXISTS (SELECT * FROM INSERTED)
        SET @action_type = 'INSERT'
    ELSE
        SET @action_type = 'DELETE'
    
    INSERT INTO AuditLog (table_name, action_type, record_id)
    SELECT 
        'TransportService',
        @action_type,
        CASE 
            WHEN @action_type = 'DELETE' THEN CAST(d.transport_id AS VARCHAR)
            ELSE CAST(i.transport_id AS VARCHAR)
        END
    FROM INSERTED i
    FULL OUTER JOIN DELETED d ON i.transport_id = d.transport_id;
END;

-- Trigger for Guide table
CREATE OR ALTER TRIGGER tr_Guide_Audit
ON Guide
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @action_type VARCHAR(10)
    
    IF EXISTS (SELECT * FROM INSERTED) AND EXISTS (SELECT * FROM DELETED)
        SET @action_type = 'UPDATE'
    ELSE IF EXISTS (SELECT * FROM INSERTED)
        SET @action_type = 'INSERT'
    ELSE
        SET @action_type = 'DELETE'
    
    INSERT INTO AuditLog (table_name, action_type, record_id)
    SELECT 
        'Guide',
        @action_type,
        CASE 
            WHEN @action_type = 'DELETE' THEN CAST(d.guide_id AS VARCHAR)
            ELSE CAST(i.guide_id AS VARCHAR)
        END
    FROM INSERTED i
    FULL OUTER JOIN DELETED d ON i.guide_id = d.guide_id;
END;

-- Trigger for Trip table
CREATE OR ALTER TRIGGER tr_Trip_Audit
ON Trip
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @action_type VARCHAR(10)
    
    IF EXISTS (SELECT * FROM INSERTED) AND EXISTS (SELECT * FROM DELETED)
        SET @action_type = 'UPDATE'
    ELSE IF EXISTS (SELECT * FROM INSERTED)
        SET @action_type = 'INSERT'
    ELSE
        SET @action_type = 'DELETE'
    
    INSERT INTO AuditLog (table_name, action_type, record_id)
    SELECT 
        'Trip',
        @action_type,
        CASE 
            WHEN @action_type = 'DELETE' THEN CAST(d.trip_id AS VARCHAR)
            ELSE CAST(i.trip_id AS VARCHAR)
        END
    FROM INSERTED i
    FULL OUTER JOIN DELETED d ON i.trip_id = d.trip_id;
END;

-- Trigger for ServiceAssignment table
CREATE OR ALTER TRIGGER tr_ServiceAssignment_Audit
ON ServiceAssignment
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @action_type VARCHAR(10)
    
    IF EXISTS (SELECT * FROM INSERTED) AND EXISTS (SELECT * FROM DELETED)
        SET @action_type = 'UPDATE'
    ELSE IF EXISTS (SELECT * FROM INSERTED)
        SET @action_type = 'INSERT'
    ELSE
        SET @action_type = 'DELETE'
    
    INSERT INTO AuditLog (table_name, action_type, record_id)
    SELECT 
        'ServiceAssignment',
        @action_type,
        CASE 
            WHEN @action_type = 'DELETE' THEN CAST(d.assignment_id AS VARCHAR)
            ELSE CAST(i.assignment_id AS VARCHAR)
        END
    FROM INSERTED i
    FULL OUTER JOIN DELETED d ON i.assignment_id = d.assignment_id;
END;

-- Trigger for Review table
CREATE OR ALTER TRIGGER tr_Review_Audit
ON Review
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @action_type VARCHAR(10)
    
    IF EXISTS (SELECT * FROM INSERTED) AND EXISTS (SELECT * FROM DELETED)
        SET @action_type = 'UPDATE'
    ELSE IF EXISTS (SELECT * FROM INSERTED)
        SET @action_type = 'INSERT'
    ELSE
        SET @action_type = 'DELETE'
    
    INSERT INTO AuditLog (table_name, action_type, record_id)
    SELECT 
        'Review',
        @action_type,
        CASE 
            WHEN @action_type = 'DELETE' THEN CAST(d.review_id AS VARCHAR)
            ELSE CAST(i.review_id AS VARCHAR)
        END
    FROM INSERTED i
    FULL OUTER JOIN DELETED d ON i.review_id = d.review_id;
END;

-- Trigger for AbandonedBooking table
CREATE OR ALTER TRIGGER tr_AbandonedBooking_Audit
ON AbandonedBooking
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @action_type VARCHAR(10)
    
    IF EXISTS (SELECT * FROM INSERTED) AND EXISTS (SELECT * FROM DELETED)
        SET @action_type = 'UPDATE'
    ELSE IF EXISTS (SELECT * FROM INSERTED)
        SET @action_type = 'INSERT'
    ELSE
        SET @action_type = 'DELETE'
    
    INSERT INTO AuditLog (table_name, action_type, record_id)
    SELECT 
        'AbandonedBooking',
        @action_type,
        CASE 
            WHEN @action_type = 'DELETE' THEN CAST(d.abandoned_id AS VARCHAR)
            ELSE CAST(i.abandoned_id AS VARCHAR)
        END
    FROM INSERTED i
    FULL OUTER JOIN DELETED d ON i.abandoned_id = d.abandoned_id;
END;