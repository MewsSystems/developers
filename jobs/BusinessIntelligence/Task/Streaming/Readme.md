# Streaming Events Examples & Simulation Guide

## Overview

This page provides sample streaming events and guidance on simulating real-time data flows for the take-home exercise.

---

## 📊 Sample Event Structures

### Booking Event Stream

**Event Type: New Booking (INSERT)**

```json
{
  "event_id": "evt_001",
  "event_type": "booking.created",
  "event_timestamp": "2024-11-03T10:30:00.000Z",
  "processing_timestamp": "2024-11-03T10:30:00.125Z",
  "operation": "INSERT",
  "source_system": "booking-api",
  "data": {
    "booking_id": "B001234",
    "guest_id": "G00567",
    "property_id": "PROP001",
    "room_type": "Deluxe King",
    "check_in_date": "2024-11-15",
    "check_out_date": "2024-11-18",
    "booking_date": "2024-11-03",
    "total_amount": 680.00,
    "currency": "USD",
    "booking_status": "Confirmed",
    "booking_source": "web",
    "number_of_guests": 2
  }
}
```

**Event Type: Booking Modification (UPDATE)**

```json
{
  "event_id": "evt_002",
  "event_type": "booking.updated",
  "event_timestamp": "2024-11-03T14:45:00.000Z",
  "processing_timestamp": "2024-11-03T14:45:00.089Z",
  "operation": "UPDATE",
  "source_system": "booking-api",
  "data": {
    "booking_id": "B001234",
    "check_in_date": "2024-11-16",
    "check_out_date": "2024-11-19",
    "total_amount": 720.00,
    "updated_at": "2024-11-03T14:45:00.000Z"
  },
  "changed_fields": ["check_in_date", "check_out_date", "total_amount"]
}
```

**Event Type: Cancellation (UPDATE)**

```json
{
  "event_id": "evt_003",
  "event_type": "booking.cancelled",
  "event_timestamp": "2024-11-03T16:20:00.000Z",
  "processing_timestamp": "2024-11-03T16:20:00.045Z",
  "operation": "UPDATE",
  "source_system": "booking-api",
  "data": {
    "booking_id": "B001234",
    "booking_status": "Cancelled",
    "cancellation_reason": "guest_request",
    "cancelled_at": "2024-11-03T16:20:00.000Z",
    "refund_amount": 720.00
  },
  "changed_fields": ["booking_status"]
}
```

**Event Type: Check-In (UPDATE)**

```json
{
  "event_id": "evt_004",
  "event_type": "booking.checked_in",
  "event_timestamp": "2024-11-16T15:00:00.000Z",
  "processing_timestamp": "2024-11-16T15:00:00.112Z",
  "operation": "UPDATE",
  "source_system": "pms-frontend",
  "data": {
    "booking_id": "B001234",
    "booking_status": "Checked-In",
    "actual_check_in": "2024-11-16T15:00:00.000Z",
    "room_number": "612"
  },
  "changed_fields": ["booking_status", "actual_check_in"]
}
```

---

### Guest Event Stream (CDC Pattern)

**Event Type: New Guest Profile (INSERT)**

```json
{
  "event_id": "evt_101",
  "event_type": "guest.created",
  "event_timestamp": "2024-11-03T09:15:00.000Z",
  "operation": "INSERT",
  "table": "guests",
  "data": {
    "guest_id": "G00890",
    "name": "Sarah Martinez",
    "email": "sarah.martinez@email.com",
    "phone": "+1-555-0123",
    "country": "USA",
    "loyalty_tier": "None",
    "created_at": "2024-11-03T09:15:00.000Z",
    "updated_at": "2024-11-03T09:15:00.000Z"
  }
}
```

**Event Type: Loyalty Tier Update (UPDATE)**

```json
{
  "event_id": "evt_102",
  "event_type": "guest.updated",
  "event_timestamp": "2024-11-03T18:30:00.000Z",
  "operation": "UPDATE",
  "table": "guests",
  "data": {
    "guest_id": "G00890",
    "loyalty_tier": "Silver",
    "updated_at": "2024-11-03T18:30:00.000Z"
  },
  "before": {
    "loyalty_tier": "None"
  },
  "changed_fields": ["loyalty_tier"]
}
```

---

### Room Inventory Updates (Incremental Batch)

**Event Type: Price Update**

```json
{
  "event_id": "evt_201",
  "event_type": "room.price_updated",
  "event_timestamp": "2024-11-03T00:05:00.000Z",
  "operation": "UPDATE",
  "source": "revenue-management-system",
  "data": {
    "property_id": "PROP001",
    "room_type": "Deluxe King",
    "nightly_rate": 220.00,
    "effective_date": "2024-11-03",
    "previous_rate": 200.00,
    "reason": "demand_surge"
  }
}
```

---

## 🎬 Simulation Strategies

### Option 1: Timestamp-Based Replay

Convert static data into streaming events:

```python
import pandas as pd
import json
from datetime import datetime, timedelta

# Load historical bookings
bookings = pd.read_csv('bookings.csv')
bookings['booking_date'] = pd.to_datetime(bookings['booking_date'])
bookings = bookings.sort_values('booking_date')

# Generate INSERT events
for idx, row in bookings.iterrows():
    event = {
        "event_id": f"evt_{idx:06d}",
        "event_type": "booking.created",
        "event_timestamp": row['booking_date'].isoformat() + 'Z',
        "operation": "INSERT",
        "data": row.to_dict()
    }
    # Simulate processing delay
    processing_delay = random.uniform(0.05, 0.5)  # 50-500ms
    
    yield event
```

### Option 2: Introduce Modifications

Create UPDATE events for existing bookings:

```python
# Simulate cancellations (10% of bookings)
cancellation_candidates = bookings.sample(frac=0.1)

for idx, row in cancellation_candidates.iterrows():
    # Cancellation happens 1-5 days after booking
    cancel_delay = timedelta(days=random.randint(1, 5))
    cancel_time = pd.to_datetime(row['booking_date']) + cancel_delay
    
    event = {
        "event_id": f"evt_cancel_{idx:06d}",
        "event_type": "booking.cancelled",
        "event_timestamp": cancel_time.isoformat() + 'Z',
        "operation": "UPDATE",
        "data": {
            "booking_id": row['booking_id'],
            "booking_status": "Cancelled"
        },
        "changed_fields": ["booking_status"]
    }
    
    yield event
```

### Option 3: Late-Arriving Events

Simulate out-of-order processing:

```python
# Introduce 5% late arrivals (1-2 hours late)
late_events = random.sample(all_events, int(len(all_events) * 0.05))

for event in late_events:
    original_time = datetime.fromisoformat(event['event_timestamp'].rstrip('Z'))
    delay = timedelta(hours=random.uniform(1, 2))
    
    # Event timestamp stays the same (when it actually happened)
    # But processing timestamp is delayed
    event['processing_timestamp'] = (original_time + delay).isoformat() + 'Z'
```

---

## 🔄 CDC Event Patterns

### Full CDC Event Structure

```json
{
  "schema": {
    "type": "struct",
    "fields": [
      {"field": "guest_id", "type": "string"},
      {"field": "name", "type": "string"},
      {"field": "email", "type": "string"},
      {"field": "loyalty_tier", "type": "string"}
    ]
  },
  "payload": {
    "before": {
      "guest_id": "G00123",
      "name": "John Doe",
      "email": "john@email.com",
      "loyalty_tier": "Silver"
    },
    "after": {
      "guest_id": "G00123",
      "name": "John Doe",
      "email": "john.doe@email.com",
      "loyalty_tier": "Gold"
    },
    "source": {
      "version": "1.0",
      "connector": "debezium",
      "name": "mews-guests-db",
      "ts_ms": 1699014000000,
      "snapshot": "false",
      "db": "production",
      "table": "guests"
    },
    "op": "u",
    "ts_ms": 1699014000123
  }
}
```

---

## 📈 Event Generation Script

### Simple Event Simulator

```python
import json
import time
from datetime import datetime, timedelta
import random

class BookingEventSimulator:
    def __init__(self, bookings_df, events_per_second=10):
        self.bookings = bookings_df
        self.eps = events_per_second
        self.event_counter = 0
        
    def generate_insert_event(self, booking_row):
        self.event_counter += 1
        return {
            "event_id": f"evt_{self.event_counter:08d}",
            "event_type": "booking.created",
            "event_timestamp": booking_row['booking_date'].isoformat() + 'Z',
            "processing_timestamp": datetime.utcnow().isoformat() + 'Z',
            "operation": "INSERT",
            "source_system": random.choice(["web", "mobile", "api"]),
            "data": booking_row.to_dict()
        }
    
    def generate_update_event(self, booking_id, changes):
        self.event_counter += 1
        return {
            "event_id": f"evt_{self.event_counter:08d}",
            "event_type": "booking.updated",
            "event_timestamp": datetime.utcnow().isoformat() + 'Z',
            "operation": "UPDATE",
            "data": {
                "booking_id": booking_id,
                **changes
            },
            "changed_fields": list(changes.keys())
        }
    
    def stream_events(self):
        """Generator that yields events at specified rate"""
        for _, booking in self.bookings.iterrows():
            event = self.generate_insert_event(booking)
            yield json.dumps(event)
            
            # Simulate streaming rate
            time.sleep(1.0 / self.eps)
            
            # Randomly generate updates (30% chance)
            if random.random() < 0.3:
                time.sleep(random.uniform(10, 300))  # 10s to 5min later
                update = self.generate_update_event(
                    booking['booking_id'],
                    {"booking_status": random.choice(["Confirmed", "Cancelled"])}
                )
                yield json.dumps(update)

# Usage
simulator = BookingEventSimulator(bookings_df, events_per_second=10)
for event in simulator.stream_events():
    print(event)
    # Or send to Kafka, write to file, etc.
```

---

## 🧪 Testing Scenarios

### Scenario 1: Burst Traffic

```python
# Simulate Black Friday spike
def generate_burst(normal_eps=10, burst_eps=100, burst_duration_sec=60):
    start_time = time.time()
    current_eps = normal_eps
    
    while True:
        elapsed = time.time() - start_time
        
        # Switch to burst mode
        if 300 < elapsed < 360:  # Burst for 1 minute after 5 minutes
            current_eps = burst_eps
        else:
            current_eps = normal_eps
        
        yield generate_event()
        time.sleep(1.0 / current_eps)
```

### Scenario 2: Late Arrivals

```python
def introduce_late_events(events, late_percentage=0.05, max_delay_hours=2):
    sorted_events = sorted(events, key=lambda e: e['event_timestamp'])
    late_indices = random.sample(range(len(events)), int(len(events) * late_percentage))
    
    for idx in late_indices:
        event = sorted_events[idx]
        delay = timedelta(hours=random.uniform(0.5, max_delay_hours))
        event['processing_timestamp'] = (
            datetime.fromisoformat(event['event_timestamp'].rstrip('Z')) + delay
        ).isoformat() + 'Z'
    
    return sorted_events
```

### Scenario 3: Duplicates

```python
def introduce_duplicates(events, duplicate_percentage=0.02):
    duplicate_candidates = random.sample(events, int(len(events) * duplicate_percentage))
    
    for event in duplicate_candidates:
        # Same event_id, slightly different processing_timestamp
        duplicate = event.copy()
        duplicate['processing_timestamp'] = (
            datetime.fromisoformat(event['processing_timestamp'].rstrip('Z')) 
            + timedelta(milliseconds=random.randint(100, 500))
        ).isoformat() + 'Z'
        
        events.append(duplicate)
    
    return events
```

---

## 📦 Sample Event Files

### Download Sample Events

Files attached to this page:

* [sample_booking_events.jsonl](./sample_booking_events.jsonl) - 1,000 booking events
* [sample_guest_events.jsonl](./sample_guest_events.jsonl) - 500 CDC events
* [sample_late_events.jsonl](./sample_late_events.jsonl) - 50 late-arriving events
* [event_simulator.py](./event_simulator.py) - Python script to generate events

---

## 💡 Tips for Simulating Streams

### Use Kafka Locally

```shell
# Start Kafka with Docker
docker-compose up -d

# Create topic
kafka-topics --create --topic bookings --bootstrap-server localhost:9092

# Produce events
python event_simulator.py | kafka-console-producer \
  --topic bookings --bootstrap-server localhost:9092
```

### Use Files with Timestamps

```python
# Write events to file with delays
with open('events.jsonl', 'w') as f:
    for event in generate_events():
        f.write(json.dumps(event) + '\n')
        time.sleep(0.1)  # 10 events per second
```

### Use Cloud Services

* **Azure Event Hubs**: Create free tier namespace
* **AWS Kinesis**: Use Kinesis Data Generator
* **Confluent Cloud**: Free trial available

---

## 🔍 Validation Checklist

Your streaming simulation should include:

- [ ] At least 1,000 events total
- [ ] Mix of INSERT, UPDATE operations
- [ ] 5-10% late-arriving events
- [ ] 2-3% duplicate events
- [ ] Varying event rates (simulate bursts)
- [ ] Event timestamps in chronological order
- [ ] Processing timestamps may be out of order
- [ ] Schema evolution (some events have extra fields)

---

**Use this guide to create realistic streaming scenarios for your solution!**