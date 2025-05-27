import { test, expect } from '@playwright/test';

test('User can search and navigate movies', async ({ page }) => {
  await page.goto('http://localhost:5173/');

  await expect(page.getByText(/Lilo & Stitch/i)).toBeVisible();

  await page.getByRole('button', { name: 'Next' }).click();
  await expect(page.getByText(/Captain America/i)).toBeVisible();

  const searchInput = page.getByPlaceholder('Search movies...');
  await searchInput.fill('Moana');
  await expect(page.getByRole('heading', { name: /Moana/i })).toBeVisible();

  await page.getByRole('heading', { name: /Moana/i }).click();
  await expect(page.getByRole('heading', { name: /Moana/i })).toBeVisible();

  await page.getByRole('button', { name: '← Back to Movies' }).click();
  await expect(page.getByText(/Moana/i)).toBeVisible();
});
