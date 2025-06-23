import { createMiddleware } from '@tanstack/start';

export const moviesMiddleware = createMiddleware().server(({ next }) => {
  return next({
    context: {
      moviesUrl: process.env.MOVIES_URL,
      apiKey: process.env.MOVIES_API_KEY,
    },
  });
});
