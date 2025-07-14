# Site Structure Overview



## Data API Layer

The app exposes two main API endpoints for the frontend:

- **Search API:**
    - `GET /api/movies?search=cars&page=3`
        - Code: [`src/app/api/movies/route.ts`](../src/app/api/movies/route.ts)
- **Movie Details API:**
    - `GET /api/movies/21192`
        - Code: [`src/app/api/movies/[movieId]/route.ts`](../src/app/api/movies/[movieId]/route.ts)

**How these endpoints work:**
- Both act as a **Backend-for-Frontend (BFF)** layer.
- They aggregate data from [TMDB’s API](https://www.themoviedb.org/documentation/api):
    - **Search:**
        - `https://api.themoviedb.org/3/search/movie?query=car&page=1`
        - `https://api.themoviedb.org/3/configuration`
    - **Movie Details:**
        - `https://api.themoviedb.org/3/movie/11`
        - `https://api.themoviedb.org/3/configuration`
- TMDB configuration is cached and reused to quickly build image URLs.
- Our API endpoints aggregate and reformat the data, so the frontend only needs a single request.

**API Caching:**
- We assume the data doesn't change often, so we use **Next.js in-memory caching** with specific revalidation times:
    - **Search results:** 5 minutes (updated most often)
    - **Movie details:** 1 hour (rarely changes)
    - **Configuration:** 2 hours (almost never changes)
- All cache times are configurable via environment variables.

---

## Client (Frontend) Code

The site uses the Next.js App Router.

- **Search Page:** `/`
    - Code: [`src/app/page.tsx`](../src/app/page.tsx)
    - Example: `http://localhost:3000/?search=Cars&page=9`
- **Movie Details Page:** `/movies/:movieId`
    - Code: [`src/app/movies/[movieId]/page.tsx`](../src/app/movies/[movieId]/page.tsx)
    - Example: `http://localhost:3000/movies/1726-iron-man`
- **Shared Layout:**
    - Code: [`src/app/layout.tsx`](../src/app/layout.tsx)

**How data flows:**
- Both pages are **server-side rendered (SSR)** on initial load.
- Each page fetches data via our API endpoints above.
- Data is then **hydrated** into the frontend—so the client can pick up with the same data instantly.
- Both pages use `export const revalidate = ...` to control how often the server regenerates the HTML (e.g., `revalidate = 300` for 5 minutes).

---

## Caching Strategy Breakdown

We use caching throughout our application to deliver data quickly, reduce unnecessary network requests, and keep the user 
experience smooth and responsive. By storing API responses and page data in memory (both on the server and client, using 
features like Tanstack Query’s cache and Next.js’s in-memory revalidation), we minimize load on external APIs, reduce latency, 
and allow repeated or similar requests to be served instantly from cache. This improves performance for all users, helps control 
costs, and ensures our pages remain fast and up-to-date without sacrificing scalability or flexibility.

### A. API BFF Aggregation Layer
- **In-memory caching** for fetch requests to TMDB, using `fetch` with custom revalidation.
- **Files:**
    - [`src/app/api/movies/route.ts`](../src/app/api/movies/route.ts)
    - [`src/app/api/movies/[movieId]/route.ts`](../src/app/api/movies/[movieId]/route.ts)
- **Revalidation times:**
    - Search: 5 min, Details: 1 hour, Config: 2 hours (all configurable via env).

### B. Server-side Page Load
- **SSR pages** fetch data from the API layer.
- **Files:**
    - [`src/app/page.tsx`](../src/app/page.tsx)
    - [`src/app/movies/[movieId]/page.tsx`](../src/app/movies/[movieId]/page.tsx)
- **HTML revalidation:**
    - Both pages export `revalidate` (e.g., 300 seconds) for Incremental Static Regeneration.
- **Layered with API caching**—the server can deliver fast, up-to-date HTML using cached API responses.

### C. Tanstack Query Cache (Client-Side)
- On the frontend, **Tanstack Query** manages its own cache for search results and details.
- This enables:
    - **Instant updates** when the same data is already in the cache from earlier queries.
    - **Reduced API requests** as the user navigates or paginates.
- **After the initial SSR load**, the client uses and updates its local Tanstack Query cache for all further UI interactions.

---

## Environment Variable Control

- **All main cache durations** (API fetches, page revalidation, configuration) are configurable in the environment (`.env`) file.
- This allows for easy tuning of cache/freshness for different environments or requirements.

---

## Multi-Level Cache Timing Caveats

With **multiple levels of cache** (API layer, server-side page, Tanstack Query client cache), keep in mind:
- **Data may be cached in multiple places**—if cache durations are set too long, you may serve stale data.
- **Cache layers can get out of sync:** for example, if the API BFF layer's cache is newer than the page cache, or vice versa.
- **Best practices:**
    - Align cache times to match user expectations for data freshness.
    - Remember: The fastest responses come from the client (Tanstack Query cache), then the API layer, then the external TMDB API.
    - For dynamic data, use shorter cache durations.
    - For static data, longer cache improves speed and lowers load on external APIs.

---

## CDN Caching as a possible additional improvement

While Next.js’s built-in fetch revalidation provides a layer of in-memory caching, it is limited to each serverless function 
instance and is not shared globally across all requests. For even better performance and scalability, you can leverage CDN 
caching by setting HTTP cache headers on your API responses using `NextResponse`. This allows upstream CDNs (such as Vercel 
Edge Network, Cloudflare, or Fastly) to cache API responses at the edge, serving repeated requests instantly to users 
worldwide and reducing backend load.

For example, to instruct a CDN to cache movie search results for 5 minutes, you can add the following headers in your API route:

```ts
// src/app/api/movies/route.ts

import { NextResponse } from 'next/server';

// ...after you have your response data
const response = NextResponse.json(result, { status: 200 });
response.headers.set('Cache-Control', 'public, max-age=300, stale-while-revalidate=59');

// For more aggressive edge caching, you might use:
response.headers.set('CDN-Cache-Control', 'public, max-age=300, stale-while-revalidate=59');

return response;
```

### What’s the Difference Between `Cache-Control` and `CDN-Cache-Control`?

#### Cache-Control
- The **standard HTTP header** used to control caching for browsers, proxies, and CDNs.
- Tells all caches (browser, CDN, reverse proxy, etc.) how to treat the response: how long to keep it, when to revalidate, etc.
- **Example:**

```ts
Cache-Control: public, max-age=300, stale-while-revalidate=59
```

This means any cache can keep this response for 5 minutes, then can serve a stale copy for up to 59 seconds while revalidating.

#### CDN-Cache-Control
- A **non-standard, vendor-specific header** recognized by some CDNs (such as [Vercel Edge Network](https://vercel.com/docs/edge-network/caching#cdn-cache-control)), but **not all**.
- Used **only by the CDN**, and is ignored by browsers.
- If present, allows you to specify different cache behavior for the CDN versus the browser.
- **Example use case:**
- You want the CDN to cache a response for 10 minutes, but browsers only for 1 minute:
  ```
  Cache-Control: public, max-age=60
  CDN-Cache-Control: public, max-age=600
  ```
- **If you only use `Cache-Control`, both browser and CDN will follow the same rules.**
- **If you use both, CDN follows `CDN-Cache-Control`, browser follows `Cache-Control`.**

---

#### Summary Table

| Header               | Who obeys it?           | Standard?   | Example Use                         |
|----------------------|------------------------|-------------|-------------------------------------|
| `Cache-Control`      | Browsers, CDNs, proxies| Yes         | Default caching everywhere          |
| `CDN-Cache-Control`  | Some CDNs only         | No          | Override for CDN caching behavior   |

---

**In most cases, just using `Cache-Control` is enough,** unless you want the browser and CDN to have different cache times or you 
are using a CDN that supports this header.

This approach enables CDNs to serve cached responses quickly, reduces latency for users, and further decreases the number 
of times your API (or the upstream TMDB API) needs to be called. By combining serverless function caching, Tanstack Query 
client caching, and CDN edge caching, you can achieve robust performance and scalability for your app.
