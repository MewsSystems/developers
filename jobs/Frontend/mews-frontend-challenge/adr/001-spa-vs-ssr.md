# SPA vs SSR Approach

**Date: 2024-04-21**

## Context

We are building a simple search application for movies. This app is internal to our team, so it doesn't require search engine optimization (SEO) considerations. The user base is small, and there's no expectation for high traffic or complex server-side processing. The current technology stack primarily uses front-end frameworks and doesn't involve server-side rendering (SSR).

## Decision

With no SEO or complex data-processing needs, we've decided to use a Single Page Application (SPA) architecture to develop this app. The reasons for this decision are as follows:

1. **Consistency with Existing Stack**: Our team's expertise and current technology stack focus on front-end frameworks. Using an SPA aligns with our team's skillset and the tools we already use. Additionally, some UI libraries don't work with SSR, making SPA a more flexible option.

2. **Reduced Infrastructure Complexity**: SPAs can be deployed to static hosts, reducing the need for a production Node.js server for SSR. This reduces infrastructure complexity, providing a simpler setup that's easier to manage and scale. With an SPA, we avoid the overhead of server-side rendering (SSR) requirements.

3. **Decreased Backend Workload**: By relying on client-side rendering, we can offload much of the work from the backend. This approach allows the server to focus on API endpoints and data handling, resulting in less stress on server resources.

Overall, these factors point to SPA as the ideal choice for our application, given its simplicity, flexibility, and reduced infrastructure needs.

## Additional Links

- [SPA Basics](https://developer.mozilla.org/en-US/docs/Glossary/SPA)
- [Client-Side Rendering vs. Server-Side Rendering](https://vite-plugin-ssr.com/SPA-vs-SSR)
