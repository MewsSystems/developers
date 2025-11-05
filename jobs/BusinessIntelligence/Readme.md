# Senior BI Engineer Take-Home Exercise - Hospitality Domain

## 🎯 Overview

Welcome to the Mews Senior BI Engineer take-home exercise! This exercise evaluates your **real-time data engineering skills** through a realistic hospitality data scenario focusing on **streaming ingestion** and **incremental processing strategies**.

**Time Estimate**: 4-6 hours    
**Domain**: Hospitality (Hotel Property Management)    
**Focus Areas**: Real-Time Data Ingestion, Incremental Processing, Stream Processing, Change Data Capture (CDC)

---

## 📚 Documentation Pages

This exercise consists of several pages:

1. [**Dataset Documentation**](./Task/Dataset/Readme.md) - Historical baseline data
2. [**Streaming Events Guide**](./Task/Streaming/Readme.md) - Event structures and simulation
3. [**Hospitality Metrics Reference**](./Task/Hospitality%20Metrics%20Quick%20Reference.md) - SQL examples and KPI formulas

---

## 🚀 What You'll Build

Design and implement a **real-time data pipeline** that handles streaming hotel booking data with incremental processing strategies to support live dashboards and operational analytics.

### Core Components

1. **Real-Time Data Ingestion** - Stream processing from multiple sources
2. **Incremental Loading Strategy** - Efficient CDC and delta processing
3. **State Management** - Handle late-arriving data and out-of-order events
4. **Near Real-Time Analytics** - Sub-minute latency for operational metrics
5. **Scalable Architecture** - Design for high-throughput streaming scenarios

---

## 📊 Scenario Context

You're building a data platform for Mews that processes **live booking events** from hotels worldwide. The system needs to:

* Process bookings as they happen (real-time)
* Handle updates to existing bookings (modifications, cancellations)
* Support incremental batch processing for historical data
* Maintain accurate metrics with minimal latency
* Scale from 1 hotel processing 10 bookings/day to 1,000+ hotels processing 10,000+ bookings/hour

### Data Sources

**Source 1: Booking Events Stream (Real-Time)**

* New bookings created
* Booking modifications (room changes, date changes)
* Cancellations and no-shows
* Check-ins and check-outs
* Format: JSON events with timestamps

**Source 2: Guest Updates Stream (Real-Time)**

* Guest profile creations
* Loyalty tier updates
* Contact information changes
* Format: CDC events (INSERT, UPDATE, DELETE)

**Source 3: Room Inventory (Batch + Incremental)**

* Static configuration (updates infrequent)
* Incremental updates when rooms added/removed
* Price changes (dynamic pricing updates)

**Initial Dataset**: Use the provided [historical data](./Task/Dataset/Readme.md) as the baseline, then simulate streaming events using the [Streaming Events Guide](./Task/Streaming/Readme.md).

---

## 🎯 Requirements

### Part 1: Real-Time Ingestion Architecture (40%)

**1.1 Streaming Data Ingestion**

Design and implement a streaming ingestion pipeline that handles:

* **Event-driven ingestion** from multiple sources
* **Schema evolution** (new fields added over time)
* **Event ordering** and late-arriving data
* **Duplicate detection** and idempotency
* **Backpressure handling** for high-volume scenarios

**Technical Requirements:**

* Process events with <1 minute end-to-end latency
* Handle out-of-order events (up to 1 hour late)
* Support at least 100 events/second throughput
* Implement watermarking for late data
* Ensure exactly-once semantics for critical metrics

**1.2 Incremental Loading Strategy**

Implement an incremental processing approach that includes:

* **Change Data Capture (CDC)** pattern for guest updates
* **Merge/Upsert logic** for handling updates to existing bookings
* **Soft deletes** for cancelled bookings (maintain history)
* **Partitioning strategy** by date and property for efficiency
* **Checkpoint management** for resumability

**Key Scenarios to Handle:**

* Guest books a room → Creates new booking
* Guest modifies dates → Updates existing booking (preserve history)
* Guest cancels → Soft delete (booking_status = 'Cancelled')
* Guest checks in → Status update (booking_status = 'Checked-In')
* Late-arriving event → Process correctly despite delay

**1.3 State Management**

Handle stateful operations:

* Maintain running aggregates (daily revenue, occupancy)
* Track booking lifecycle (created → modified → confirmed → checked-in → checked-out)
* Manage session windows for multi-event bookings
* Implement exactly-once processing guarantees

### Part 2: Near Real-Time Analytics (20%)

Build analytics that update in near real-time:

**Streaming Metrics** (Update every 1-5 minutes):

* Current occupancy rate (who's checked in now)
* Today's revenue (running total)
* Live booking funnel (new bookings last hour/day)
* Cancellation alerts (immediate notifications)

**Micro-Batch Metrics** (Update every 15-30 minutes):

* Rolling 7-day ADR and RevPAR
* Guest segmentation updates
* Trending room types
* Lead time distribution

**Dashboard Requirements:**

* Show "as of [timestamp]" freshness indicator
* Display both real-time and batch-computed metrics
* Handle eventual consistency gracefully

### Part 3: Architecture & Scalability (30%)

Design for production-scale streaming:

**Architecture Diagram Must Show:**

1. **Ingestion Layer**

    * Event sources (Kafka, Kinesis, Event Hub, or similar)
    * Stream processing engine (Spark Streaming, Flink, Kafka Streams)
    * Dead letter queues for failed events
    
2. **Processing Layer**

    * Stream processing jobs
    * Batch jobs for historical data
    * State stores and checkpointing
    * Data quality validation
    
3. **Storage Layer**

    * Hot storage (recent data, fast queries)
    * Warm storage (last 90 days)
    * Cold storage (historical archive)
    * Partitioning and indexing strategy
    
4. **Serving Layer**

    * Query patterns for real-time dashboards
    * Caching strategy
    * API endpoints for live data
    

**Scalability Considerations:**

* How to handle 10x traffic increase
* Multi-region deployment strategy
* Failure recovery and replay mechanisms
* Cost optimization (compute vs storage)

**Document:**

* Technology choices and trade-offs
* Latency vs throughput trade-offs
* Consistency guarantees
* Monitoring and alerting strategy
* Incremental vs full refresh decisions

### Part 4: Implementation & Communication (10%)

**Code/Pseudocode:**

* Streaming job implementation (working or detailed pseudocode)
* Incremental merge logic
* State management code
* Data quality checks

**Documentation:**

* Clear explanation of ingestion strategy
* Incremental loading algorithm
* How you handle edge cases
* Testing approach for streaming systems

---

## 🌊 Streaming Scenarios to Address

Your solution should handle these real-world scenarios:

### Scenario 1: High-Volume Booking Window

* 1,000 bookings arrive in 10 minutes (Black Friday sale)
* System must process without dropping events
* Metrics must stay accurate under load

### Scenario 2: Late-Arriving Cancellation

* Booking created at 10:00 AM
* Cancellation event arrives at 2:00 PM (for 10:05 AM cancellation)
* System must retroactively correct metrics

### Scenario 3: Duplicate Events

* Network retry causes duplicate booking event
* System must deduplicate correctly
* Idempotency key: booking_id + event_timestamp

### Scenario 4: Schema Evolution

* New field added: `booking_source` (web, mobile, phone)
* Existing pipelines must continue working
* Historical data lacks this field

### Scenario 5: Multi-Property Update

* 100 hotels update their room rates simultaneously
* Incremental processing must be efficient
* Avoid full table scans

---

## 📋 Deliverables

1. **Streaming Architecture Design**

    * End-to-end architecture diagram
    * Data flow from ingestion to serving
    * Technology stack justification
    
2. **Incremental Processing Implementation**

    * CDC/Upsert logic (code or detailed pseudocode)
    * Partitioning strategy
    * Checkpoint/state management approach
    
3. **Code/Pseudocode**

    * Streaming job implementation
    * Incremental merge logic
    * Late data handling
    
4. **Scaling Strategy Document**

    * How to scale from 10 to 10,000 events/sec
    * Failure recovery approach
    * Monitoring and observability
    
5. **Presentation (5-10 slides)**

    * Architectural approach
    * Key design decisions
    * Trade-offs made
    * Performance characteristics

---

## 💡 Key Questions to Answer

Your solution should address:

1. **Latency vs Completeness**: How do you balance fast processing with waiting for late data?
2. **Exactly-Once Processing**: How do you guarantee no double-counting in metrics?
3. **Failure Recovery**: What happens if your stream processor crashes?
4. **Backfill Strategy**: How do you reprocess historical data without disrupting live streams?
5. **Schema Evolution**: How do you handle new fields without breaking pipelines?
6. **Cost Optimization**: When to use micro-batching vs true streaming?

---

## 🎁 Bonus Challenges (Optional)

* Implement a **lambda architecture** (streaming + batch) with consistency guarantees
* Design a **Kappa architecture** (streaming-only) alternative
* Add **multi-region active-active** replication
* Implement **complex event processing** (CEP) for fraud detection
* Build a **backpressure handling** mechanism
* Design **blue-green deployment** strategy for zero-downtime updates

---

## 📥 Download Datasets

**Baseline historical data in [Dataset Documentation](./Dataset%20Documentation.md):**
* [bookings.csv](./Task/Dataset/bookings.csv) - Use as initial state
* [guests.json](./Task/Dataset/guests.json) - Use as initial state
* [room_inventory.sql](./Task/Dataset/room_inventory.sql) - Static reference data

**Streaming event examples in [Streaming Events Guide](./Submission%20Guidelines%20&%20Tips.md):**
* Sample event structures (JSON)
* Event simulation scripts (Python)
* CDC patterns and examples

---

**Good luck! We're excited to see how you approach real-time data engineering! 🚀**

_Focus on demonstrating your understanding of streaming concepts and incremental processing strategies. Working code is great, but clear architectural thinking is more important._