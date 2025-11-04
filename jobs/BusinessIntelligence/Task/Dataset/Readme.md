# Dataset Documentation

## Overview

This package contains three realistic datasets for a fictional hotel property management system. The data simulates one year of booking activity (2024) for a mid-size hotel with 145 rooms across 5 room types.

## Files Included

1. [**bookings.csv**](./bookings.csv) - Transaction-level booking data (1,005 records)
2. [**guests.json**](./guests.json) - Guest profile information (600 records)
3. [**room_inventory.csv**](./room_inventory.csv) - Hotel room configuration (5 room types)
4. [**room_inventory.sql**](./room_inventory.sql) - SQL script to create and populate room inventory table

## Source 1: Bookings ([bookings.csv](./bookings.csv))

**Description**: Transactional data for hotel bookings including confirmed, checked-out, and cancelled reservations.

**Schema**:

* `booking_id` - String - Unique booking identifier (format: B000001)
* `guest_id` - String - Foreign key to guests table (format: G00001)
* `room_type` - String - Type of room booked
* `check_in_date` - Date - Guest check-in date (YYYY-MM-DD)
* `check_out_date` - Date - Guest check-out date (YYYY-MM-DD)
* `booking_date` - Date - Date when booking was made (YYYY-MM-DD)
* `total_amount` - Float - Total booking value in USD
* `booking_status` - String - Current status of booking

**Booking Status Values**:

* `Confirmed` - Future reservation
* `Checked-In` - Guest currently at property
* `Checked-Out` - Completed stay
* `Cancelled` - Cancelled booking
* `No-Show` - Guest didn't arrive
* _(empty string)_ - Missing data (data quality issue)

<!-- **Known Data Quality Issues** (intentional):

* ~5 records with missing `total_amount` values
* ~5 records with missing/empty `booking_status`
* ~5 duplicate bookings (same guest, overlapping dates)
* ~3 records with invalid date logic (check_out_date before check_in_date)

**Dataset Statistics**:

* **Records**: 1,005
* **Date Range**: Jan 2024 - Jun 2025
* **Revenue**: $899,601.91
* **Statuses**: 868 Checked-Out, 86 Cancelled, 46 No-Show -->

## Source 2: Guests ([guests.json](./guests.json))

**Description**: Guest profile data including contact information and loyalty status.

**Schema**:

* `guest_id` - String - Unique guest identifier (format: G00001)
* `name` - String - Guest full name
* `email` - String - Guest email address
* `country` - String - Guest country of origin
* `loyalty_tier` - String - Loyalty program status

**Loyalty Tier Values**:

* `None` - Not enrolled in loyalty program (~50% of guests)
* `Silver` - Basic tier (~30% of guests)
* `Gold` - Mid-tier (~15% of guests)
* `Platinum` - Premium tier (~5% of guests)

<!-- **Dataset Statistics**:

* **Records**: 600 unique guests
* **Countries**: 20 represented
* **Top Countries**: China, Netherlands, Germany, India, Spain -->

## Source 3: Room Inventory ([room_inventory.sql](./room_inventory.sql))

**Description**: Static configuration of hotel room types and pricing.

**Schema**:

* `room_type` - String - Name of room category
* `total_rooms` - Integer - Number of rooms of this type
* `nightly_rate` - Float - Base nightly rate in USD
* `floor` - String - Floor location(s) of room type
* `amenities` - String - Comma-separated list of amenities

**Room Types**:

| Room Type | Total Rooms | Nightly Rate | Floors | Key Amenities |
| --- | --- | --- | --- | --- |
| Standard Double | 40 | $140 | 2-5 | WiFi, TV, Coffee Maker |
| Standard Queen | 50 | $150 | 2-5 | WiFi, TV, Coffee Maker |
| Deluxe King | 30 | $200 | 6-8 | Mini Bar, City View |
| Suite | 15 | $350 | 9-10 | Balcony, Sitting Area |
| Executive Suite | 10 | $500 | 10 | Panoramic View, Jacuzzi |

<!-- **Total Capacity**: 145 rooms -->

## Data Relationships

```
bookings.guest_id  →  guests.guest_id (Many-to-One)
bookings.room_type →  room_inventory.room_type (Many-to-One)
```

## Business Context

### Hotel Profile

* **Type**: Mid-size urban hotel
* **Capacity**: 145 rooms across 5 categories
* **Market Segment**: Mix of business and leisure travelers
* **Geography**: International clientele from 20+ countries
* **Pricing Strategy**: Dynamic pricing with rates varying 80-130% of base rate

### Booking Patterns

* **Lead Time**: Most bookings made 7-60 days in advance
* **Length of Stay**: Typically 2-3 nights
* **Cancellation Rate**: ~8-10%
* **No-Show Rate**: ~4-5%

---