# MBMRTT company - Currency Rates

### Local Setup

1. Clone the repo, and navigate into the newly created folder.
2. Run `npm i` in your terminal of choice

See documentation for `sf` commands [here](https://developer.salesforce.com/docs/atlas.en-us.sfdx_cli_reference.meta/sfdx_cli_reference/cli_reference_unified.htm).

### Scratch orgs

To spin up new scratch orgs use the command `sf org create scratch --edition developer --alias scratch-org3 -v mbmrtt` .
In this example `scratch-org3` reference the given name (alias) of the newly created scratch org, while `mbmrtt` reference your DevHub.

`sf org open --target-org scratch-org3`
`sf config set target-org=scratch-org3`

To capture changes make directly on the scratch org, use the below

`sf project retrieve preview`
`sf project retrieve start`

To generate the package run
`sf package create --name mbmrttPack --description "Mbmrtt - Exchange Rates" --package-type Unlocked --path force-app --target-dev-hub mbmrtt`

NAME VALUE  
 ────────── ──────────────────
Package Id 0Ho8d000000CaqZCAS

To generate versions for the package run

`sf package version create --package 0Ho8d000000CaqZCAS --installation-key mbmrttPassword123 --target-dev-hub mbmrtt`

To install the package in a new scratch org run

`sf package install --package 04t8d000000u8EcAAI --installation-key mbmrttPassword123 --target-org scratch-org3`

```
  Name                          Value
 ───────────────────────────── ─────────────────────────────────────────────────────────────────────────────────
 Package Id                    0Ho8d000000CaqZCAS
 Subscriber Package Version Id 04t8d000000u8EcAAI
 Installation URL              https://login.salesforce.com/packaging/installPackage.apexp?p0=04t8d000000u8EcAAI
```
