# Logging in Exchange Rate Updater

This document describes the logging setup for the Exchange Rate Updater application.

## Overview

The Exchange Rate Updater uses Serilog for structured logging with different configurations for development and production environments:

- **Development**: Human-readable console output with verbose debugging information
- **Production**: JSON-formatted logs for easier machine parsing and integration with log management systems

## Configuration

Logging configuration is stored in:

- `appsettings.json` - Base configuration
- `appsettings.Development.json` - Development-specific overrides

## Log Levels

| Environment | Default Level | Microsoft/System Level |
|-------------|---------------|------------------------|
| Development | Debug         | Information            |
| Production  | Information   | Warning                |

## Log Outputs

The following log outputs are configured:

1. **Console**
    - Development: Human-readable format with timestamp, level, and message
    - Production: Compact JSON format

2. **File**
    - Path: `logs/exchange-rate-updater-.log` (with date suffix)
    - Format: Text in development, JSON in production
    - Rolling: Daily