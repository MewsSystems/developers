# SSR Hydration Pattern with Tanstack Query

**SSR (Server-Side Rendering)** means your page’s HTML (and the initial data) are built on the server, not in the browser.

- The server fetches the data (using your own API or external APIs), and builds a full HTML page with that data already in place.
- This HTML is sent to the browser—so users see the page instantly, with real data (not just a loading spinner).

**Hydration** happens next:
- The browser loads React and your Tanstack Query setup.
- Tanstack Query “hydrates” (restores) the data that was fetched on the server—so it’s available in the client-side cache, ready for instant use by your React components.
- React becomes interactive, using the already-fetched data (no extra fetch needed after first load).

**Pattern summary (with Tanstack Query):**
- **Server:** Fetches the data, builds the HTML, and puts the data in Tanstack Query’s cache state.
- **Client:** Loads the React app, rehydrates the cache with the same data, and continues as a fast, interactive app—without fetching the same data again.

---

## Why is this good?

- **Fast first load:** HTML and data are there immediately.
- **No duplicate requests:** Tanstack Query knows it already has the data!
- **SEO-friendly:** The page is fully readable by search engines.
- **Smooth React experience after hydration:** Client-side cache is ready to go.
- Gives you the best parts of SSR and SPA-style applications.

---

## SSR/SSG Hydration Pattern Used on the Search Page

There are **three main ways data can be updated or loaded** on our search page:

### 1. Initial Page Load (HTML)
- **What:**  
  The very first time you visit a page (or reload), your browser gets a full HTML document from the server.
- **How to see it:**  
  Open Chrome DevTools, go to the Network tab, and filter by "Document". You’ll see a request that loads the base HTML for the page.
- **What happens:**
    - Next.js does server-side rendering (SSR): it fetches the needed data on the server, builds the HTML, and sends it to the browser.
    - This ensures the page is fast to display, SEO-friendly, and has all the initial data already in place.
- **Content-Type:** `text/html`
- **Result:**  
  You see the whole page rendered, even before React becomes interactive.

---

### 2. React Component Load (Flight/Partial Hydration)
- **What:**  
  As you navigate between pages (client-side navigation), Next.js sends React "fragments" instead of the whole document.
- **How to see it:**  
  In the Network tab, filter by "Fetch/XHR". Look for requests with content-type: `text/x-component` or similar.
- **What happens:**
    - Next.js streams just the bits of React you need to update the UI (not a full page reload!).
    - This is called the React Server Components protocol (Flight).
    - It’s more efficient: only sends the necessary HTML/JS for the component that changed.
- **Content-Type:** `text/x-component`
- **Result:**  
  Navigation feels instant. Only new/changed parts of the page are sent and hydrated.

---

### 3. JSON Loads via Tanstack Query (Client Data Fetch)
- **What:**  
  When your React components (client-side) need to fetch more data (like a new search, pagination, or background update), they use Tanstack Query to fetch JSON from your own API routes.
- **How to see it:**  
  In the Network tab, filter by "Fetch/XHR". Look for requests with content-type: `application/json; charset=UTF-8`.
- **What happens:**
    - The browser requests just the JSON data needed for the update (not a new HTML page).
    - This is how you get fresh results when you type a new search, click “next page”, etc.
    - Your page stays responsive and fast, and only updates the bits that need new data.
- **Content-Type:** `application/json; charset=UTF-8`
- **Result:**  
  The UI updates responsively as you interact, with no full page reloads.

---

## Summary Table

| Method                      | When/Why it Happens                | What You See in DevTools           | Content-Type                        |
|-----------------------------|------------------------------------|------------------------------------|-------------------------------------|
| Initial Page Load (HTML)    | First load, hard reload, SSR       | "Document"                         | `text/html`                         |
| React Component (Flight)    | Client-side navigation, RSC hydration | "Fetch/XHR" (`text/x-component`) | `text/x-component`                  |
| JSON Load (Tanstack Query)  | Client data updates, search, pagination | "Fetch/XHR" (`application/json`) | `application/json; charset=UTF-8`   |

---

## How This Relates to Our Code

### [`src/app/page.tsx`](../src/app/page.tsx) (Search Page - SSR and Hydration)
- On the **initial page load**, this file runs on the server.  
  It fetches the search data and sets up the Tanstack Query cache for hydration.
- If you visit the site or reload, you get a fully populated HTML page and hydrated cache (SSR).
- If a fetch error occurs server-side, a friendly message can be displayed.

### [`src/features/home/HomeSearchSection/HomeSearchSection.tsx`](../src/features/home/HomeSearchSection/HomeSearchSection.tsx) (Client-Side Data Updates)
- Handles **user interaction**: typing in the search box, pagination, etc.
- Uses Tanstack Query on the **client** to fetch and cache JSON data from the API routes when needed (e.g., new searches or pages).
- Handles loading, error, and success states to keep the interface responsive and efficient.
- This is where you see **JSON loads** and dynamic UI updates in DevTools.

---

### **When do the different updates apply?**

- **Initial Page Load:**  
  When you visit the page for the first time or reload.  
  (SSR/SSG, HTML document, fully hydrated by Tanstack Query.)

- **Client-Side Navigation (Flight):**  
  Navigating to other pages that aren't the search page (not a hard reload).
  (React Server Components protocol, sends just what changed.)

- **Client Data Updates (JSON):**  
  When you interact with the UI (e.g., search or paginate), triggering new API calls for fresh data.  
  (Handled by Tanstack Query, updating just the relevant UI.)

---

**This pattern means your app is always fast, up-to-date, and provides a responsive, seamless user experience—with no wasted data fetching or awkward blank/loading screens!**

## Caveats: Navigation, SSR Hydration, and Tanstack Query

Using SSR hydration with Tanstack Query in Next.js gives you fast loads and instant data on first visit. However, **the way you handle navigation and URL updates in Next.js has a direct effect on how well Tanstack Query’s caching and hydration features work.**

### The Problem

- **Initial Page Load:**  
  On the first load or hard refresh, SSR/SSG and Tanstack Query hydration work perfectly—data is fetched on the server, cached, and ready on the client.

- **Client-Side Navigation (Flight):**  
  When navigating between pages or updating search params using Next.js's built-in methods (such as `<Link>`, `router.push`, or `router.replace`), Next.js performs a "Flight" navigation, sending only the changed React components and their data.  
  However, **this navigation method circumvents how Tanstack Query utilizes cache on the client and may even potentially trigger additional requests for data**, even if the data has already been fetched. This can result in unnecessary loading states or network traffic that could otherwise be avoided.

- **Loss of Cache Benefits:**  
  The main advantage of Tanstack Query is avoiding unnecessary data requests—if a user has already loaded some data, it stays in cache and doesn't need to be fetched again.  
  However, using Next.js’s router methods for navigation and updating search params can cause you to lose this benefit, making the UI slower and less efficient.

### Known Next.js Limitation

At this point in time, **you can't easily configure Next.js's router methods or the `<Link>` component to favor SSR hydration or force a full reload when only updating search params or shallow routing.**  
This is a known limitation—see [vercel/next.js#72383](https://github.com/vercel/next.js/issues/72383).

- As a result, you can't guarantee preserved cache or SSR hydration with just the built-in router/navigation tools.

### Our Workaround

**To avoid these issues, we've created a custom hook [`useSsrHydratedUrlState`](../src/hooks/useSsrHydratedUrlState.ts) that:**

- Synchronizes your app state with the URL's search params, so that bookmarking, sharing, and reloads always “just work.”
- Updates the browser’s URL using the [History API](https://developer.mozilla.org/en-US/docs/Web/API/History_API) instead of triggering a full page reload or losing cache.
- Keeps Tanstack Query’s cache intact during client-side navigation or param updates, preserving fast and efficient data loading.

#### [`src/hooks/useSsrHydratedUrlState.ts`](../src/hooks/useSsrHydratedUrlState.ts) — What does it do?

This hook enables "URL-synced state with SSR hydration and seamless SPA navigation":

- On **initial page load** (SSR/SSG), it initializes state from the URL, so hydration works as intended.
- During **client-side navigation** or param updates, it updates the URL using `pushState` instead of Next.js router methods, avoiding unnecessary reloads and keeping Tanstack Query’s cache intact.
- When the user interacts (search, paginate), only the relevant data is fetched and the URL is updated, but the page is never fully reloaded—preserving both UI experience and cache.

> *Monitor Next.js updates: If router.push or Link become more configurable (allowing control over navigation mode), consider switching to the native API in the future.*

---
