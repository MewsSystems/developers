import { test, expect } from '@playwright/test';

test('Search avengers movies', async ({ page }) => {
  await page.goto('http://localhost:5173/');
  await page.locator('#searchInput').fill('avengers');
  await expect(page.getByRole('link', { name: 'Avengers: Infinity War' })).toBeVisible();
});

test('No movies result', async ({ page }) => {
  await page.goto('http://localhost:5173/');
  await page.locator('#searchInput').fill('random movie with no results');
  await expect(page.getByText('No movies found')).toBeVisible();
});

test('Load more results', async ({ page }) => {
  await page.goto('http://localhost:5173/');
  await page.locator('#searchInput').fill('Lion');
  const firstResultsCount = await page.getByTestId('movieCard').count();
  await page.getByRole('button', { name: 'Load more' }).click();
  const secondResultsCount = await page.getByTestId('movieCard').count();

  await expect(firstResultsCount).toBeLessThan(secondResultsCount);
});

test('Go to details page', async ({ page }) => {
  const movieName = 'Mufasa: The Lion King';

  await page.goto('http://localhost:5173/');
  await page.locator('#searchInput').fill('Lion');
  await page.getByRole('link', { name: movieName }).click();
  await expect(page.getByRole('heading', { name: movieName })).toBeVisible();
});
