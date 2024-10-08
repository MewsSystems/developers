## The Deployed Application

The deployed application can be found at [https://movie-search-nextjs.vercel.app/](https://movie-search-nextjs.vercel.app/)

## Project Structure

This project is a Next.js application that follows the Bulletproof React structure with some Next.js flavoured things. For more information on the structure of the project, please refer to the offical [Bulletproof React](https://github.com/alan2207/bulletproof-react) docs.

## Testing

I've gone with favouring playwright E2E tests as they give a closer representation of how users interact with the application and a MOTO style of mocks. Where we mock outside using mock service worker and test as far outside as possible not caring about the implementation details of the app.

## Production

- This application uses Next's [opentelemetry](https://nextjs.org/docs/app/building-your-application/optimizing/open-telemetry) to collect performance metrics and traces. This is a great way to monitor the performance of the application in production. These traces can be sent to a collector like [Jaeger](https://www.jaegertracing.io/) or [Lightstep](https://lightstep.com/) so that we can monitor the application in production.

## Future Ideas

- We could add caching to the nexts backend for frontend [(BFF)](https://medium.com/mobilepeople/backend-for-frontend-pattern-why-you-need-to-know-it-46f94ce420b0) reducing the amount of requests we make to the MovieDB API. If the service worked on credits or had a rate limit this would be a good idea.

- Rather than requiring the user to click to load more movies, we could implement infinite scrolling similar to how TikTok or Instagram work where the user scrolls to the bottom of the page and more movies are loaded.

- We could shift the state of search to the url so that users can share their saved searches with others or bookmark them for later.

- Currently we are only tracing out events that happen on the server with the default node instrumentation. We could add more instrumentation to the client side so we get a full timeline of events that users are experiencing.

- Performance considerations could be made for some of the high traffic movies. We could statically generate these pages rather than generating them on the server for every user improving the speed the pages render.

## Getting Started

These instructions will guide you on setting up this repo on your local machine.

### Prerequisites

- Node.js
- npm
- Docker (if you want to run the OpenTelemetry Collector)

## Setting Up The Project

1. Navigate to the project directory:

```bash
cd ...
```

2. Install Dependencies:

```bash
npm install
```

3. Install Playwright:

```bash
npx playwright install --with-deps
```

4. Setup environment variables:

**copy the `.env.example` file to a new file called `.env.local`.**

```bash
copy .env.example .env.local
```

Replace the **`MOVIE_DB_API_ACCESS_TOKEN`** variable with your own Access Token from [The Movie Database](https://www.themoviedb.org/settings/api) or ask and I can provide you with one.

## Run Development Server

Finally, run the development server:

```bash
npm run dev
```

Now you can open [http://localhost:3000](http://localhost:3000) with your browser to see the application.

## Running Tests

### Playwright

To run playwright tests, run the following command:

```bash
npm run test:e2e
```

This will build the application and run the playwrigh tests.

Note: You don't need to repeatedly build the application once you've done it once. Running the above command at the start will set you up to then simply run:

```bash
npm run playwright
```

For subsequent test runs or `npx playwright test` if you prefer playwrights cli.

### Vitest

To run vitest tests, run the following command:

```bash
npm run test
```

## Running OpenTelemetry Collector

1. Start the Jaeger and OpenTelemetry Collector services using Docker Compose:

```sh
docker-compose -f infra/docker-compose.yaml up -d
```

This will start the necessary tracing infrastructure in the background.

## Viewing Jaeger Trace UI

To view the Jaeger Trace UI, open your browser and navigate to:

```
http://localhost:16686
```
