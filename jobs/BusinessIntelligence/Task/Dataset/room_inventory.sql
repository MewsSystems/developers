-- Room Inventory Table
CREATE TABLE IF NOT EXISTS room_inventory (
    room_type VARCHAR(50) PRIMARY KEY,
    total_rooms INT,
    nightly_rate DECIMAL(10,2),
    floor VARCHAR(20),
    amenities TEXT
);

INSERT INTO room_inventory (room_type, total_rooms, nightly_rate, floor, amenities) VALUES
('Standard Queen', 50, 150, '2-5', 'WiFi,TV,Coffee Maker'),
('Deluxe King', 30, 200, '6-8', 'WiFi,TV,Coffee Maker,Mini Bar,City View'),
('Suite', 15, 350, '9-10', 'WiFi,TV,Coffee Maker,Mini Bar,City View,Balcony,Sitting Area'),
('Executive Suite', 10, 500, '10', 'WiFi,TV,Coffee Maker,Mini Bar,Panoramic View,Balcony,Sitting Area,Jacuzzi'),
('Standard Double', 40, 140, '2-5', 'WiFi,TV,Coffee Maker');
