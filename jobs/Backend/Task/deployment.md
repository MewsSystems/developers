# Deployment Guide: ExchangeRateUpdater

This document explains how to deploy and run the Exchange Rate Updater console app in production.

---

## Build & Publish

Publish as a **self-contained single file executable** for your target OS:

```bash
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o ./publish
