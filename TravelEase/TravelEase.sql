-- Create the database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'TravelEase')
BEGIN
    CREATE DATABASE TravelEase;
END
GO

USE TravelEase;
GO

-- Create User table
CREATE TABLE [User] ( --user is reserverd so []
    user_id INT IDENTITY(1,1) PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE,
    phone_number VARCHAR(20),
    registration_date DATE NOT NULL DEFAULT GETDATE(),
    is_active BIT NOT NULL DEFAULT 1,
    role VARCHAR(20) NOT NULL
);

-- Create Preferences table
CREATE TABLE Preference (
    preference_id INT IDENTITY(1,1) PRIMARY KEY,
    preference_name VARCHAR(50) NOT NULL UNIQUE,
    description VARCHAR(255)
);

-- Create Traveler table
CREATE TABLE Traveler (
    traveler_id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT NOT NULL,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    date_of_birth DATE,
    nationality VARCHAR(50),
    address VARCHAR(255),
    FOREIGN KEY (user_id) REFERENCES [User](user_id)
);

-- Create TravelerPreference table for many-to-many relationship
CREATE TABLE TravelerPreference (
    traveler_id INT NOT NULL,
    preference_id INT NOT NULL,
    PRIMARY KEY (traveler_id, preference_id),
    FOREIGN KEY (traveler_id) REFERENCES Traveler(traveler_id),
    FOREIGN KEY (preference_id) REFERENCES Preference(preference_id)
);

-- Create TourOperator table
CREATE TABLE TourOperator (
    operator_id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT NOT NULL,
    company_name VARCHAR(100) NOT NULL,
    description VARCHAR(MAX),
    address VARCHAR(255),
    FOREIGN KEY (user_id) REFERENCES [User](user_id)
);

-- Create Admin table
CREATE TABLE Admin (
    admin_id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT NOT NULL,
    FOREIGN KEY (user_id) REFERENCES [User](user_id)
);

-- Create ServiceProvider table
CREATE TABLE ServiceProvider (
    provider_id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT NOT NULL,
    provider_type VARCHAR(50) NOT NULL,
    FOREIGN KEY (user_id) REFERENCES [User](user_id)
);

-- Create Hotel table
CREATE TABLE Hotel (
    hotel_id INT IDENTITY(1,1) PRIMARY KEY,
    provider_id INT NOT NULL,
    hotel_name VARCHAR(100) NOT NULL,
    location VARCHAR(255) NOT NULL,
    total_rooms INT,
    FOREIGN KEY (provider_id) REFERENCES ServiceProvider(provider_id)
);

-- Create TransportService table
CREATE TABLE TransportService (
    transport_id INT IDENTITY(1,1) PRIMARY KEY,
    provider_id INT NOT NULL,
    transport_type VARCHAR(50) NOT NULL,
    vehicle_details VARCHAR(MAX),
    capacity INT,
    FOREIGN KEY (provider_id) REFERENCES ServiceProvider(provider_id)
);

-- Create GuideService table
CREATE TABLE Guide (
    guide_id INT IDENTITY(1,1) PRIMARY KEY,
    provider_id INT NOT NULL,
    guide_name VARCHAR(100) NOT NULL,
    specialization VARCHAR(100),
    FOREIGN KEY (provider_id) REFERENCES ServiceProvider(provider_id)
);

-- Create Language table
CREATE TABLE Language (
    language_id INT IDENTITY(1,1) PRIMARY KEY,
    language_name VARCHAR(50) NOT NULL UNIQUE
);

-- Create GuideLanguage table for many-to-many relationship
CREATE TABLE GuideLanguage (
    guide_id INT NOT NULL,
    language_id INT NOT NULL,
    PRIMARY KEY (guide_id, language_id),
    FOREIGN KEY (guide_id) REFERENCES Guide(guide_id),
    FOREIGN KEY (language_id) REFERENCES Language(language_id)
);

-- Create TripCategory table
CREATE TABLE TripCategory (
    category_id INT IDENTITY(1,1) PRIMARY KEY,
    category_name VARCHAR(50) NOT NULL UNIQUE,
    description VARCHAR(MAX)
);

-- Create Destination table
CREATE TABLE Destination (
    destination_id INT IDENTITY(1,1) PRIMARY KEY,
    destination_name VARCHAR(100) NOT NULL,
    country VARCHAR(50) NOT NULL,
    region VARCHAR(50),
    description VARCHAR(MAX)
);

-- Create Activity table
CREATE TABLE Activity (
    activity_id INT IDENTITY(1,1) PRIMARY KEY,
    activity_name VARCHAR(100) NOT NULL,
    description VARCHAR(MAX)
);

-- Create Itinerary table
CREATE TABLE Itinerary (
    itinerary_id INT IDENTITY(1,1) PRIMARY KEY,
    description VARCHAR(MAX)
);

-- Create ItineraryActivity table for many-to-many relationship
CREATE TABLE ItineraryActivity (
    itinerary_id INT NOT NULL,
    activity_id INT NOT NULL,
    PRIMARY KEY (itinerary_id, activity_id),
    FOREIGN KEY (itinerary_id) REFERENCES Itinerary(itinerary_id),
    FOREIGN KEY (activity_id) REFERENCES Activity(activity_id)
);

-- Create Trip table
CREATE TABLE Trip (
    trip_id INT IDENTITY(1,1) PRIMARY KEY,
    operator_id INT NOT NULL,
    category_id INT NOT NULL,
    destination_id INT NOT NULL,
    itinerary_id INT NOT NULL,
    trip_name VARCHAR(100) NOT NULL,
    description VARCHAR(MAX),
    start_date DATE NOT NULL,
    end_date DATE NOT NULL,
    duration INT NOT NULL, -- in days
    price DECIMAL(10, 2) NOT NULL,
    capacity INT NOT NULL,
    sustainability_score VARCHAR(10),
    wheelchair_access BIT DEFAULT 0,
    FOREIGN KEY (operator_id) REFERENCES TourOperator(operator_id),
    FOREIGN KEY (category_id) REFERENCES TripCategory(category_id),
    FOREIGN KEY (destination_id) REFERENCES Destination(destination_id),
    FOREIGN KEY (itinerary_id) REFERENCES Itinerary(itinerary_id)
);

-- Create Booking table
CREATE TABLE Booking (
    booking_id INT IDENTITY(1,1) PRIMARY KEY,
    traveler_id INT NOT NULL,
    trip_id INT NOT NULL,
    booking_date DATE NOT NULL DEFAULT GETDATE(),
    status VARCHAR(20) NOT NULL, -- 'Confirmed', 'Pending', 'Cancelled', etc.
    total_amount DECIMAL(10, 2) NOT NULL,
    is_cancelled BIT DEFAULT 0,
    cancellation_reason VARCHAR(MAX),
    FOREIGN KEY (traveler_id) REFERENCES Traveler(traveler_id),
    FOREIGN KEY (trip_id) REFERENCES Trip(trip_id)
);

-- Create ServiceAssignment table
CREATE TABLE ServiceAssignment (
    assignment_id INT IDENTITY(1,1) PRIMARY KEY,
    booking_id INT NOT NULL,
    provider_id INT NOT NULL,
    assignment_date DATE NOT NULL DEFAULT GETDATE(),
    status VARCHAR(20) NOT NULL, -- 'Assigned', 'Confirmed', 'Rejected', etc.
    FOREIGN KEY (booking_id) REFERENCES Booking(booking_id),
    FOREIGN KEY (provider_id) REFERENCES ServiceProvider(provider_id)
);

-- Create Payment table
CREATE TABLE Payment (
    payment_id INT IDENTITY(1,1) PRIMARY KEY,
    booking_id INT NOT NULL,
    amount DECIMAL(10, 2) NOT NULL,
    payment_date DATE NOT NULL DEFAULT GETDATE(),
    payment_method VARCHAR(50) NOT NULL,
    status VARCHAR(20) NOT NULL, -- 'Completed', 'Pending', 'Failed', etc.
    is_refunded BIT DEFAULT 0,
    FOREIGN KEY (booking_id) REFERENCES Booking(booking_id)
);

-- Create Review table
CREATE TABLE Review (
    review_id INT IDENTITY(1,1) PRIMARY KEY,
    traveler_id INT NOT NULL,
    trip_id INT,
    provider_id INT,
    rating FLOAT NOT NULL,
    comment VARCHAR(MAX),
    review_date DATE NOT NULL DEFAULT GETDATE(),
    is_flagged BIT DEFAULT 0,
    FOREIGN KEY (traveler_id) REFERENCES Traveler(traveler_id),
    FOREIGN KEY (trip_id) REFERENCES Trip(trip_id),
    FOREIGN KEY (provider_id) REFERENCES ServiceProvider(provider_id),
    -- Ensure that either trip_id or provider_id is set, but not necessarily both
    CONSTRAINT CK_Review_Target CHECK (
        (trip_id IS NOT NULL AND provider_id IS NULL) OR
        (trip_id IS NULL AND provider_id IS NOT NULL) OR
        (trip_id IS NOT NULL AND provider_id IS NOT NULL)
    )
);

-- Create WishList table
CREATE TABLE WishList (
    wishlist_id INT IDENTITY(1,1) PRIMARY KEY,
    traveler_id INT NOT NULL,
    date_created DATE NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (traveler_id) REFERENCES Traveler(traveler_id)
);

-- Create WishListTrip table for many-to-many relationship
CREATE TABLE WishListTrip (
    wishlist_id INT NOT NULL,
    trip_id INT NOT NULL,
    added_date DATE NOT NULL DEFAULT GETDATE(),
    PRIMARY KEY (wishlist_id, trip_id),
    FOREIGN KEY (wishlist_id) REFERENCES WishList(wishlist_id),
    FOREIGN KEY (trip_id) REFERENCES Trip(trip_id)
);

-- Create DigitalPass table
CREATE TABLE DigitalPass (
    pass_id INT IDENTITY(1,1) PRIMARY KEY,
    booking_id INT NOT NULL,
    pass_type VARCHAR(50) NOT NULL,
    document_link VARCHAR(MAX),
    qr_code VARCHAR(MAX),
    expiry_date DATE NOT NULL,
    FOREIGN KEY (booking_id) REFERENCES Booking(booking_id)
);

-- Create AbandonedBooking table
CREATE TABLE AbandonedBooking (
    abandoned_id INT IDENTITY(1,1) PRIMARY KEY,
    traveler_id INT NOT NULL,
    trip_id INT NOT NULL,
    timestamp DATETIME NOT NULL DEFAULT GETDATE(),
    stage_abandoned VARCHAR(50) NOT NULL, -- 'Payment', 'Information', 'Review', etc.
    FOREIGN KEY (traveler_id) REFERENCES Traveler(traveler_id),
    FOREIGN KEY (trip_id) REFERENCES Trip(trip_id)
);

-- Import User data
BULK INSERT [User]
FROM 'C:\Users\PC\Desktop\TravelEase\new_data\user.csv'
WITH (
    FORMAT = 'CSV',
    FIRSTROW = 2, -- Skip header row
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    TABLOCK
);

-- Import Preference data
BULK INSERT Preference
FROM 'C:\Users\PC\Desktop\TravelEase\new_data\preference.csv'
WITH (
    FORMAT = 'CSV',
    FIRSTROW = 2,
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    TABLOCK
);

-- Import Traveler data
BULK INSERT Traveler
FROM 'C:\Users\PC\Desktop\TravelEase\new_data\traveler.csv'
WITH (
    FORMAT = 'CSV',
    FIRSTROW = 2,
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    TABLOCK
);

-- Import TravelerPreference data
BULK INSERT TravelerPreference
FROM 'C:\Users\PC\Desktop\TravelEase\new_data\traveler_preference.csv'
WITH (
    FORMAT = 'CSV',
    FIRSTROW = 2,
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    TABLOCK
);

-- Import TourOperator data
BULK INSERT TourOperator
FROM 'C:\Users\PC\Desktop\TravelEase\new_data\tour_operator.csv'
WITH (
    FORMAT = 'CSV',
    FIRSTROW = 2,
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    TABLOCK
);

-- Import Admin data
BULK INSERT Admin
FROM 'C:\Users\PC\Desktop\TravelEase\new_data\admin.csv'
WITH (
    FORMAT = 'CSV',
    FIRSTROW = 2,
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    TABLOCK
);

-- Import ServiceProvider data
BULK INSERT ServiceProvider
FROM 'C:\Users\PC\Desktop\TravelEase\new_data\service_provider.csv'
WITH (
    FORMAT = 'CSV',
    FIRSTROW = 2,
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    TABLOCK
);

-- Import Hotel data
BULK INSERT Hotel
FROM 'C:\Users\PC\Desktop\TravelEase\new_data\hotel.csv'
WITH (
    FORMAT = 'CSV',
    FIRSTROW = 2,
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    TABLOCK
);

-- Import TransportService data
BULK INSERT TransportService
FROM 'C:\Users\PC\Desktop\TravelEase\new_data\transport_service.csv'
WITH (
    FORMAT = 'CSV',
    FIRSTROW = 2,
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    TABLOCK
);

-- Import Guide data
BULK INSERT Guide
FROM 'C:\Users\PC\Desktop\TravelEase\new_data\guide.csv'
WITH (
    FORMAT = 'CSV',
    FIRSTROW = 2,
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    TABLOCK
);

-- Import Language data
BULK INSERT Language
FROM 'C:\Users\PC\Desktop\TravelEase\new_data\language.csv'
WITH (
    FORMAT = 'CSV',
    FIRSTROW = 2,
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    TABLOCK
);

-- Import GuideLanguage data
BULK INSERT GuideLanguage
FROM 'C:\Users\PC\Desktop\TravelEase\new_data\guide_language.csv'
WITH (
    FORMAT = 'CSV',
    FIRSTROW = 2,
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    TABLOCK
);

-- Import TripCategory data
BULK INSERT TripCategory
FROM 'C:\Users\PC\Desktop\TravelEase\new_data\trip_category.csv'
WITH (
    FORMAT = 'CSV',
    FIRSTROW = 2,
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    TABLOCK
);

-- Import Destination data
BULK INSERT Destination
FROM 'C:\Users\PC\Desktop\TravelEase\new_data\destination.csv'
WITH (
    FORMAT = 'CSV',
    FIRSTROW = 2,
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    TABLOCK
);

-- Import Activity data
BULK INSERT Activity
FROM 'C:\Users\PC\Desktop\TravelEase\new_data\activity.csv'
WITH (
    FORMAT = 'CSV',
    FIRSTROW = 2,
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    TABLOCK
);

-- Import Itinerary data
BULK INSERT Itinerary
FROM 'C:\Users\PC\Desktop\TravelEase\new_data\itinerary.csv'
WITH (
    FORMAT = 'CSV',
    FIRSTROW = 2,
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    TABLOCK
);

-- Import ItineraryActivity data
BULK INSERT ItineraryActivity
FROM 'C:\Users\PC\Desktop\TravelEase\new_data\itinerary_activity.csv'
WITH (
    FORMAT = 'CSV',
    FIRSTROW = 2,
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    TABLOCK
);

-- Import Trip data
BULK INSERT Trip
FROM 'C:\Users\PC\Desktop\TravelEase\new_data\trip.csv'
WITH (
    FORMAT = 'CSV',
    FIRSTROW = 2,
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    TABLOCK
);

-- Import Booking data
BULK INSERT Booking
FROM 'C:\Users\PC\Desktop\TravelEase\new_data\booking.csv'
WITH (
    FORMAT = 'CSV',
    FIRSTROW = 2,
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    TABLOCK
);

-- Import ServiceAssignment data
BULK INSERT ServiceAssignment
FROM 'C:\Users\PC\Desktop\TravelEase\new_data\service_assignment.csv'
WITH (
    FORMAT = 'CSV',
    FIRSTROW = 2,
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    TABLOCK
);

-- Import Payment data
BULK INSERT Payment
FROM 'C:\Users\PC\Desktop\TravelEase\new_data\payment.csv'
WITH (
    FORMAT = 'CSV',
    FIRSTROW = 2,
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    TABLOCK
);

-- Import Review data
BULK INSERT Review
FROM 'C:\Users\PC\Desktop\TravelEase\new_data\review.csv'
WITH (
    FORMAT = 'CSV',
    FIRSTROW = 2,
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    TABLOCK
);

-- Import WishList data
BULK INSERT WishList
FROM 'C:\Users\PC\Desktop\TravelEase\new_data\wishlist.csv'
WITH (
    FORMAT = 'CSV',
    FIRSTROW = 2,
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    TABLOCK
);

-- Import WishListTrip data
BULK INSERT WishListTrip
FROM 'C:\Users\PC\Desktop\TravelEase\new_data\wishlist_trip.csv'
WITH (
    FORMAT = 'CSV',
    FIRSTROW = 2,
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    TABLOCK
);

-- Import DigitalPass data
BULK INSERT DigitalPass
FROM 'C:\Users\PC\Desktop\TravelEase\new_data\digital_pass.csv'
WITH (
    FORMAT = 'CSV',
    FIRSTROW = 2,
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    TABLOCK
);

-- Import AbandonedBooking data
BULK INSERT AbandonedBooking
FROM 'C:\Users\PC\Desktop\TravelEase\new_data\abandoned_booking.csv'
WITH (
    FORMAT = 'CSV',
    FIRSTROW = 2,
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    TABLOCK
);

-- Verify data import
SELECT 'User' AS TableName, COUNT(*) AS RecordCount FROM [User]
UNION ALL
SELECT 'Traveler', COUNT(*) FROM Traveler
UNION ALL
SELECT 'Preference', COUNT(*) FROM Preference
UNION ALL
SELECT 'TravelerPreference', COUNT(*) FROM TravelerPreference
UNION ALL
SELECT 'TourOperator', COUNT(*) FROM TourOperator
UNION ALL
SELECT 'Admin', COUNT(*) FROM Admin
UNION ALL
SELECT 'ServiceProvider', COUNT(*) FROM ServiceProvider
UNION ALL
SELECT 'Hotel', COUNT(*) FROM Hotel
UNION ALL
SELECT 'TransportService', COUNT(*) FROM TransportService
UNION ALL
SELECT 'Guide', COUNT(*) FROM Guide
UNION ALL
SELECT 'Language', COUNT(*) FROM Language
UNION ALL
SELECT 'GuideLanguage', COUNT(*) FROM GuideLanguage
UNION ALL
SELECT 'TripCategory', COUNT(*) FROM TripCategory
UNION ALL
SELECT 'Destination', COUNT(*) FROM Destination
UNION ALL
SELECT 'Activity', COUNT(*) FROM Activity
UNION ALL
SELECT 'Itinerary', COUNT(*) FROM Itinerary
UNION ALL
SELECT 'ItineraryActivity', COUNT(*) FROM ItineraryActivity
UNION ALL
SELECT 'Trip', COUNT(*) FROM Trip
UNION ALL
SELECT 'Booking', COUNT(*) FROM Booking
UNION ALL
SELECT 'ServiceAssignment', COUNT(*) FROM ServiceAssignment
UNION ALL
SELECT 'Payment', COUNT(*) FROM Payment
UNION ALL
SELECT 'Review', COUNT(*) FROM Review
UNION ALL
SELECT 'WishList', COUNT(*) FROM WishList
UNION ALL
SELECT 'DigitalPass', COUNT(*) FROM DigitalPass
UNION ALL
SELECT 'AbandonedBooking', COUNT(*) FROM AbandonedBooking;

PRINT 'Data import completed successfully.';