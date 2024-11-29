# MEWS Front-End Tech Test

## Error & Performance Monitoring

Monitoring performance and error metrics in web applications is essential for ensuring a great user experience, maintaining reliability, and supporting business goals. It helps diagnose issues, optimize performance, and proactively resolve problems before they impact users. Key metrics like load times, error rates, and crash reports provide actionable insights, enabling scalability, reducing downtime, and enhancing maintainability.

This is where [Sentry](https://sentry.io/welcome/) comes in help and I have configured it to capture errors and performance metrics for the frontend application.
The configuration is located in the `sentryConfig.ts` file, while alerts for web performance metrics within the tool alerts panel as shown below (the deprecated [FID](https://web.dev/articles/fid) metric was used instead of the new [INP](https://web.dev/articles/inp) because somehow Sentry has not updated the selectable alert options yet).

![Sentry alerts screenshot](./src/assets/images/sentry-alerts.png)
