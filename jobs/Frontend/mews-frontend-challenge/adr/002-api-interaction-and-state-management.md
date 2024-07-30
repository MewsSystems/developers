# API Interaction and State Management

**Date: 2024-04-21**

## Context

Our main source of data comes from an external API.

## Decision

We've decided to adopt `react-query` as our preferred library for API interaction for the following reasons:

1. **Retry System for API Errors:** `react-query` provides a robust [retry system](https://react-query.tanstack.com/guides/queries#error-handling) that can automatically handle API errors and retries requests when the API encounters transient issues. `react-query` ensures our application gracefully handles these errors and retries requests when necessary, improving overall data reliability.

2. **Cache as State Management:** `react-query` leverages an efficient cache system for data management. This cache acts as a source of truth for the application's data, reducing redundant API calls and enhancing performance. By using the cache as our primary state management solution, we can reduce the complexity of managing application state through Redux or other state management libraries.

### Comparison to SWR (Stale-While-Revalidate)

While SWR is another popular data-fetching library, `react-query` offers distinct advantages, especially in our use case:

2. **Consistency and Flexibility:** `react-query` offers a more comprehensive set of tools for handling various data-fetching scenarios, including pagination, optimistic updates, and background data synchronization. It provides greater consistency and flexibility in managing data-related logic within our application.

## Consequences

Implementing `react-query` as our primary data-fetching and state management solution, along with the use of the Suspense API from React.

Since the use the cache of `react-query` as a state management solution as well we can make direct API calls from the components themselves.

This decision aligns with our goal of addressing API error issues, improving the reliability and performance of our application, and maintaining a clean and maintainable codebase.

## Additional Links:

- [React query Documentation](https://react-query.tanstack.com/)
- [React Suspense API Documentation](https://reactjs.org/docs/react-api.html#reactsuspense)
- [SWR Documentation](https://swr.vercel.app/)
- [Tao of React - State management](https://alexkondov.com/tao-of-react/#state-management-libraries)
- [Tao of React - Data fetching](https://alexkondov.com/tao-of-react/#use-data-fetching-libraries)
