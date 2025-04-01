# DateTime Utilities

This directory contains refactored classes that were previously part of the `Utils::DateTimeHelper` module. Each class has a single responsibility, following the Single Responsibility Principle.

## Classes

### TimeConstructor

Responsible for creating Time objects with specific components.

```ruby
Utils::DateTime::TimeConstructor.create_with_components(year, month, day, hour, min, sec, timezone)
```

### BusinessDayCalculator

Handles calculations related to business days, including determining next/previous business days and adjusting for weekends.

```ruby
Utils::DateTime::BusinessDayCalculator.next_business_day(time, working_days_only)
Utils::DateTime::BusinessDayCalculator.previous_business_day(date, working_days_only)
Utils::DateTime::BusinessDayCalculator.adjust_for_working_days(time, working_days_only)
```

### PublicationScheduler

Responsible for calculating publication times based on different frequencies.

```ruby
Utils::DateTime::PublicationScheduler.calculate_next_publication(update_frequency, publication_time, current_time)
Utils::DateTime::PublicationScheduler.calculate_next_daily_publication(publication_time, current_time)
Utils::DateTime::PublicationScheduler.calculate_next_hourly_publication(publication_time, current_time)
Utils::DateTime::PublicationScheduler.publication_time_for_date(date, hour, minute, timezone)
```

### CacheTTLCalculator

Handles TTL (Time To Live) calculations for caching.

```ruby
Utils::DateTime::CacheTTLCalculator.calculate_ttl_until_next_publication(update_frequency, publication_time, current_time, default_ttl)
Utils::DateTime::CacheTTLCalculator.get_default_ttls
```

### FetchDateDeterminator

Determines the appropriate date to fetch data based on metadata.

```ruby
Utils::DateTime::FetchDateDeterminator.determine_fetch_date(metadata)
```

## Backward Compatibility

For backward compatibility, the original `Utils::DateTimeHelper` module is maintained in `compatibility.rb`. This module delegates all calls to the appropriate new classes.

```ruby
Utils::DateTimeHelper.method_name(...) # Calls the appropriate method in the new structure
```

## Benefits of this Structure

1. **Single Responsibility**: Each class has a clear, specific purpose
2. **Improved Testability**: Easier to test individual components
3. **Better Maintainability**: Changes to one aspect don't affect others
4. **Reduced Complexity**: Smaller classes with focused methods
5. **Clear Dependencies**: Dependencies between components are explicitly defined 