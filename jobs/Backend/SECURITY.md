- [Security Policy](#security-policy)
  - [Scanning](#scanning) 
  - [Reporting a Vulnerability](#reporting-a-vulnerability)

# Security Policy

We release patches for security vulnerabilities. Which versions are eligible for
receiving such patches depends on the CVSS v3.0 Rating:

| CVSS v3.0 | Supported Versions                        |
| --------- | ----------------------------------------- |
| 9.0-10.0  | Releases within the previous three months |
| 4.0-8.9   | Most recent release                       |

# Scanning

This project makes use of [OWASP dependency check](https://owasp.org/www-project-dependency-check/), and [CodeQL](https://codeql.github.com/) within the CI pipeline. 

## Reporting a Vulnerability

Please report (suspected) security vulnerabilities by opening open a github issue.
