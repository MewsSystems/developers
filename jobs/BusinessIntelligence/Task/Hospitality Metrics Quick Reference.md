# Hospitality Metrics Quick Reference

## Essential Hotel KPIs

### 1. Average Daily Rate (ADR)

**What it measures**: Average revenue earned per occupied room

**Formula**:

```
ADR = Total Room Revenue / Number of Rooms Sold
```

**SQL Example**:

```sql
SELECT 
    AVG(total_amount / DATEDIFF(check_out_date, check_in_date)) as ADR
FROM bookings
WHERE booking_status = 'Checked-Out'
```

---

### 2. Revenue Per Available Room (RevPAR)

**What it measures**: Revenue efficiency considering all available rooms

**Formula**:

```
RevPAR = Total Room Revenue / Total Available Room Nights
OR
RevPAR = ADR × Occupancy Rate
```

**SQL Example**:

```sql
SELECT 
    SUM(total_amount) / (145 * 365) as RevPAR
FROM bookings
WHERE booking_status = 'Checked-Out'
AND check_in_date BETWEEN '2024-01-01' AND '2024-12-31'
```

**Notes**: 145 = total rooms, 365 = days in year

---

### 3. Occupancy Rate

**What it measures**: Percentage of available rooms that are occupied

**Formula**:

```
Occupancy Rate = (Rooms Sold / Total Available Rooms) × 100
```

**For this dataset**:

* Total rooms: 145
* Days in 2024: 366 (leap year)
* Total room-nights available: 53,070

**SQL Example**:

```sql
SELECT 
    DATE(check_in_date) as date,
    COUNT(*) as rooms_sold,
    (COUNT(*) * 100.0 / 145) as occupancy_rate
FROM bookings
WHERE booking_status IN ('Checked-Out', 'Checked-In')
GROUP BY DATE(check_in_date)
ORDER BY date
```

---

### 4. Booking Lead Time

**What it measures**: Days between booking and check-in

**SQL Example**:

```sql
SELECT 
    AVG(DATEDIFF(check_in_date, booking_date)) as avg_lead_time,
    MIN(DATEDIFF(check_in_date, booking_date)) as min_lead_time,
    MAX(DATEDIFF(check_in_date, booking_date)) as max_lead_time
FROM bookings
WHERE booking_status != 'Cancelled'
```

---

### 5. Length of Stay (LOS)

**What it measures**: Number of nights per booking

**SQL Example**:

```sql
SELECT 
    AVG(DATEDIFF(check_out_date, check_in_date)) as avg_los,
    room_type,
    COUNT(*) as bookings
FROM bookings
WHERE booking_status = 'Checked-Out'
GROUP BY room_type
ORDER BY avg_los DESC
```

---

### 6. Cancellation Rate

**What it measures**: Percentage of bookings that don't convert to stays

**Formula**:

```
Cancellation Rate = ((Cancelled + No-Show) / Total Bookings) × 100
```

**SQL Example**:

```sql
SELECT 
    COUNT(CASE WHEN booking_status IN ('Cancelled', 'No-Show') THEN 1 END) * 100.0 / 
        COUNT(*) as cancellation_rate,
    room_type
FROM bookings
GROUP BY room_type
```

---

## Guest Segmentation Queries

### Revenue by Country

```sql
SELECT 
    g.country,
    COUNT(DISTINCT b.booking_id) as total_bookings,
    SUM(b.total_amount) as total_revenue,
    AVG(b.total_amount) as avg_booking_value
FROM bookings b
JOIN guests g ON b.guest_id = g.guest_id
WHERE b.booking_status = 'Checked-Out'
GROUP BY g.country
ORDER BY total_revenue DESC
LIMIT 10
```

### Performance by Loyalty Tier

```sql
SELECT 
    g.loyalty_tier,
    COUNT(DISTINCT b.guest_id) as unique_guests,
    COUNT(b.booking_id) as total_bookings,
    COUNT(b.booking_id) * 1.0 / COUNT(DISTINCT b.guest_id) as bookings_per_guest,
    SUM(b.total_amount) as total_revenue,
    AVG(b.total_amount) as avg_booking_value
FROM bookings b
JOIN guests g ON b.guest_id = g.guest_id
WHERE b.booking_status = 'Checked-Out'
GROUP BY g.loyalty_tier
ORDER BY total_revenue DESC
```

---

## Data Quality Checks

### Check for Missing Revenue

```sql
SELECT COUNT(*) as missing_revenue_count
FROM bookings 
WHERE total_amount IS NULL OR total_amount = 0;
```

### Check for Invalid Dates

```sql
SELECT COUNT(*) as invalid_date_count
FROM bookings 
WHERE check_out_date <= check_in_date;
```

### Check for Duplicate Bookings

```sql
SELECT guest_id, check_in_date, COUNT(*) as duplicate_count
FROM bookings
GROUP BY guest_id, check_in_date
HAVING COUNT(*) > 1;
```

### Revenue Reasonableness Check

```sql
SELECT 
    booking_id,
    room_type,
    total_amount,
    DATEDIFF(check_out_date, check_in_date) as nights,
    total_amount / DATEDIFF(check_out_date, check_in_date) as rate_per_night
FROM bookings
WHERE total_amount / DATEDIFF(check_out_date, check_in_date) < 50 
   OR total_amount / DATEDIFF(check_out_date, check_in_date) > 1000;
```

---

## Recommended Data Model

### Fact Table: fact_bookings

```sql
CREATE TABLE fact_bookings AS
SELECT 
    b.booking_id,
    b.guest_id,
    b.room_type,
    b.check_in_date,
    b.check_out_date,
    b.booking_date,
    DATEDIFF(b.check_in_date, b.booking_date) as lead_time_days,
    DATEDIFF(b.check_out_date, b.check_in_date) as length_of_stay,
    b.total_amount,
    b.total_amount / DATEDIFF(b.check_out_date, b.check_in_date) as daily_rate,
    b.booking_status,
    CASE 
        WHEN b.booking_status IN ('Cancelled', 'No-Show') THEN 1 
        ELSE 0 
    END as is_cancelled,
    g.country,
    g.loyalty_tier,
    r.nightly_rate as base_rate
FROM bookings b
LEFT JOIN guests g ON b.guest_id = g.guest_id
LEFT JOIN room_inventory r ON b.room_type = r.room_type
```

---

These metrics align with hospitality industry standards and Mews platform analytics.

---