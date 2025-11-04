#!/usr/bin/env python3
"""
Booking Event Simulator
Generate streaming events from historical booking data
"""

import pandas as pd
import json
import time
import random
import argparse
from datetime import datetime, timedelta

class BookingEventSimulator:
    """Simulates streaming booking events at a specified rate"""
    
    def __init__(self, bookings_file, events_per_second=10):
        """
        Initialize simulator
        
        Args:
            bookings_file: Path to bookings.csv
            events_per_second: Rate of event generation
        """
        self.bookings = pd.read_csv(bookings_file)
        self.bookings['booking_date'] = pd.to_datetime(self.bookings['booking_date'])
        self.bookings['check_in_date'] = pd.to_datetime(self.bookings['check_in_date'])
        self.bookings['check_out_date'] = pd.to_datetime(self.bookings['check_out_date'])
        
        # Sort by booking date for time-ordered replay
        self.bookings = self.bookings.sort_values('booking_date').reset_index(drop=True)
        
        self.eps = events_per_second
        self.event_counter = 0
        
        print(f"Loaded {len(self.bookings)} bookings")
        print(f"Date range: {self.bookings['booking_date'].min()} to {self.bookings['booking_date'].max()}")
    
    def generate_insert_event(self, booking_row):
        """Generate INSERT event for new booking"""
        self.event_counter += 1
        
        processing_delay = timedelta(milliseconds=random.uniform(50, 500))
        booking_time = booking_row['booking_date']
        
        return {
            "event_id": f"evt_{self.event_counter:08d}",
            "event_type": "booking.created",
            "event_timestamp": booking_time.isoformat() + 'Z',
            "processing_timestamp": (booking_time + processing_delay).isoformat() + 'Z',
            "operation": "INSERT",
            "source_system": random.choice(["web", "mobile", "api", "phone"]),
            "data": {
                "booking_id": booking_row['booking_id'],
                "guest_id": booking_row['guest_id'],
                "property_id": "PROP001",
                "room_type": booking_row['room_type'],
                "check_in_date": booking_row['check_in_date'].strftime('%Y-%m-%d'),
                "check_out_date": booking_row['check_out_date'].strftime('%Y-%m-%d'),
                "booking_date": booking_row['booking_date'].strftime('%Y-%m-%d'),
                "total_amount": float(booking_row['total_amount']) if pd.notna(booking_row['total_amount']) else None,
                "currency": "USD",
                "booking_status": booking_row['booking_status'] if booking_row['booking_status'] else "Confirmed",
                "number_of_guests": random.randint(1, 4)
            }
        }
    
    def generate_update_event(self, booking_id, changes, event_type="booking.updated"):
        """Generate UPDATE event"""
        self.event_counter += 1
        
        return {
            "event_id": f"evt_{self.event_counter:08d}",
            "event_type": event_type,
            "event_timestamp": datetime.utcnow().isoformat() + 'Z',
            "processing_timestamp": datetime.utcnow().isoformat() + 'Z',
            "operation": "UPDATE",
            "source_system": "booking-api",
            "data": {
                "booking_id": booking_id,
                **changes
            },
            "changed_fields": list(changes.keys())
        }
    
    def stream_events(self, with_updates=True, update_probability=0.3):
        """
        Generate streaming events
        
        Args:
            with_updates: Whether to generate UPDATE events
            update_probability: Chance of generating update for each booking
        """
        for _, booking in self.bookings.iterrows():
            # Generate INSERT event
            event = self.generate_insert_event(booking)
            yield json.dumps(event)
            
            # Simulate streaming rate
            time.sleep(1.0 / self.eps)
            
            # Randomly generate updates
            if with_updates and random.random() < update_probability:
                time.sleep(random.uniform(1, 10))  # Wait 1-10 seconds
                
                # Random update type
                update_type = random.choice(['cancel', 'modify'])
                
                if update_type == 'cancel':
                    update = self.generate_update_event(
                        booking['booking_id'],
                        {
                            "booking_status": "Cancelled",
                            "cancellation_reason": "guest_request",
                            "cancelled_at": datetime.utcnow().isoformat() + 'Z'
                        },
                        event_type="booking.cancelled"
                    )
                else:  # modify
                    update = self.generate_update_event(
                        booking['booking_id'],
                        {
                            "check_in_date": (booking['check_in_date'] + timedelta(days=1)).strftime('%Y-%m-%d'),
                            "total_amount": float(booking['total_amount']) * 1.1 if pd.notna(booking['total_amount']) else None
                        }
                    )
                
                yield json.dumps(update)
                time.sleep(1.0 / self.eps)
    
    def generate_burst(self, normal_eps=10, burst_eps=100, burst_after_sec=60, burst_duration_sec=30):
        """
        Generate events with burst pattern
        
        Args:
            normal_eps: Normal events per second
            burst_eps: Events per second during burst
            burst_after_sec: When to start burst
            burst_duration_sec: How long burst lasts
        """
        start_time = time.time()
        
        for _, booking in self.bookings.iterrows():
            elapsed = time.time() - start_time
            
            # Determine current rate
            if burst_after_sec < elapsed < burst_after_sec + burst_duration_sec:
                current_eps = burst_eps
                print(f"\r[BURST MODE] {current_eps} eps", end='', flush=True)
            else:
                current_eps = normal_eps
                print(f"\r[NORMAL] {current_eps} eps    ", end='', flush=True)
            
            event = self.generate_insert_event(booking)
            yield json.dumps(event)
            
            time.sleep(1.0 / current_eps)

def main():
    parser = argparse.ArgumentParser(description='Simulate streaming booking events')
    parser.add_argument('bookings_file', help='Path to bookings.csv')
    parser.add_argument('--eps', type=int, default=10, help='Events per second')
    parser.add_argument('--output', help='Output file (default: stdout)')
    parser.add_argument('--with-updates', action='store_true', help='Generate UPDATE events')
    parser.add_argument('--burst', action='store_true', help='Generate burst traffic pattern')
    parser.add_argument('--limit', type=int, help='Limit number of events')
    
    args = parser.parse_args()
    
    simulator = BookingEventSimulator(args.bookings_file, args.eps)
    
    # Open output
    output_file = open(args.output, 'w') if args.output else None
    
    try:
        count = 0
        if args.burst:
            events = simulator.generate_burst()
        else:
            events = simulator.stream_events(with_updates=args.with_updates)
        
        for event in events:
            if output_file:
                output_file.write(event + '\n')
            else:
                print(event)
            
            count += 1
            if args.limit and count >= args.limit:
                break
                
    except KeyboardInterrupt:
        print("\nStopped by user")
    finally:
        if output_file:
            output_file.close()
            print(f"\nWrote {count} events to {args.output}")

if __name__ == "__main__":
    main()
