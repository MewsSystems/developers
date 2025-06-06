import {expect, test} from '@playwright/test';

const SEARCH_INPUT_PLACEHOLDER = 'Start typing to discover';
const MOVIE_PATH = '/movie/603';
const SEARCH_QUERY = '?search=Matrix&page=2';

test.describe('Direct URL Navigation', () => {
  test('loads search results page with query parameters', async ({page}) => {
    await page.goto(`/${SEARCH_QUERY}`);

    const searchInput = page.getByPlaceholder(SEARCH_INPUT_PLACEHOLDER);
    await expect(searchInput).toHaveValue('Matrix');

    await page.waitForSelector('h2');

    const movieTitles = await page.locator('h2').all();
    expect(movieTitles.length).toBeGreaterThan(0);

    const titles = await Promise.all(movieTitles.map((title) => title.textContent()));
    expect(titles.some((title) => title?.toLowerCase().includes('matrix'))).toBeTruthy();
  });
  test('loads movie details page with movie ID', async ({page}) => {
    await page.goto(MOVIE_PATH);
    await page.waitForSelector('h1');

    const title = await page.locator('h1').first().textContent();
    expect(title).toContain('The Matrix');

    await expect(page.locator('p').first()).toBeVisible();
    await expect(page.getByText('Release Date')).toBeVisible();
    await expect(page.getByText('★')).toBeVisible();
  });
});
