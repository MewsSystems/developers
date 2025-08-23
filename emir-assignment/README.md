# 🎬 CinEmir — TMDB Movie Explorer

A sleek, fast movie browser built with React + TypeScript + Vite, styled with Tailwind, and powered by TMDB. It features a dynamic, cinematic UI, smooth navigation, rich details, and a polished mobile experience.

> “Find your next favorite film faster — with style.”

---

## ✨ Highlights

-   Smart search with debounce, grid results, and “Load more”
-   Cinematic detail page: hero with backdrop + gradient, ratings, and key facts
-   **Tabbed details** (synced to URL): _Overview_ / _Videos_ / _Photos_
-   Cast grid (with profile placeholders) & **More like this** rail (drag-to-scroll)
-   Robust states: skeletons, graceful errors (with retry), and empty views
-   A11y & UX polish: keyboard focus, CLS‑safe images
-   Sticky header & well-designed footer
-   Document title hook (tab shows the movie title)
-   Scroll behavior: scroll-to-top on route change, but not on “Load more” or tab switch
-   Decent amount of tests (limited due to time constraints): **Vitest** unit tests + **Playwright** E2E

---

## 🧱 Tech Stack

-   **Frontend:** React, TypeScript, Vite
-   **Routing:** React Router
-   **Styles:** Tailwind CSS, lucide-react icons
-   **API:** TMDB (The Movie Database)
-   **Testing:** Vitest + @testing-library/react + Playwright

---

## 📦 Getting Started

```bash
# 1) Install deps
npm install

# 2) Set env (see next section)
cp .env.example .env
# then set VITE_TMDB_API_KEY=your_key_here

# 3) Run dev server
npm run dev

# 4) Build & preview
npm run build
npm run preview
```

---

## 🔐 Environment Variables

Create `.env` from the example and add your TMDB key:

```env
VITE_TMDB_API_KEY=YOUR_TMDB_KEY
```

_This product uses the TMDB API but is not endorsed or certified by TMDB._

---

## 🧪 Testing

### Unit: Vitest + Testing Library

-   Configured JSDOM environment
-   Global test utils (RTL, user-event), JSX transform, and strict TypeScript

**Run:**

```bash
npm run test          # watch
npm run test:run      # single run (CI)
npm run test:ui       # if using the Vitest UI
```

**Folder convention:**

```
tests/
  unit/
    *.test.tsx
  e2e/
    *.spec.ts
```

### E2E: Playwright

-   Headless-stable tests with API mocking (routes for TMDB endpoints)
-   Local dev server bootstrapped via Playwright `webServer`

**Run:**

```bash
npm run e2e        # headless
npm run e2e:open   # headed (debug)
```

> Tip: For headless reliability, don’t use arbitrary timeouts — mock network and assert on locators (we do).

---

## 🌐 Deployment (Vercel)

This is a client-side SPA; add a rewrite so all routes serve `index.html`:

`vercel.json`

```json
{
    "rewrites": [{ "source": "/(.*)", "destination": "/" }]
}
```

Then:

1. Push your branch to GitHub (e.g., `assignment`).
2. In Vercel, import the repo and select the **assignment** branch (Project → Settings → Git → Production Branch) or create a Preview for the branch.
3. Set `VITE_TMDB_API_KEY` in **Project → Settings → Environment Variables**.
4. **Build Command:** `vite build` • **Output:** `dist`

If you saw the Vercel default 404 page, you were missing the rewrite above.

---

## 🔧 Developer Notes

-   Reduced CLS: explicit image width/height; reserved spaces in skeletons; hero has fixed height
-   URL tab sync: `?tab=overview|videos|photos` (resets to overview when navigating to a new movie)
-   Images endpoint & language: the images API can return empty arrays when language filters; we fetch without forcing language there to always get assets.
-   Scroll restoration: customized so pagination/tab switches don’t yank the viewport.

---

## 👏 Credits

-   **Data/API:** TMDB
    -   _This product uses the TMDB API but is not endorsed or certified by TMDB._
-   **Icons:** [lucide.dev](https://lucide.dev)

---

## 📄 License

**MIT** — do what you love. 💚
