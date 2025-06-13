import {test, expect, Page} from '@playwright/test';

test.describe('Movie search and details', () => {
    test.beforeEach(async ({page}) => {
        await page.goto('http://localhost:5173/');
    });

    async function searchForMovie(page: Page, query: string) {
        const searchInput = page.getByRole('textbox', {name: /search movies/i});
        await expect(searchInput).toBeVisible();
        await searchInput.fill(query);
    }

    async function getLsTheme(page: Page) {
        return await page.evaluate(() => localStorage.getItem('preferred-theme'))
    }

    test('displays search results and loads more', async ({page}) => {
        await searchForMovie(page, 'mickey');

        await expect(page.getByText('Mickey 17', {exact: true})).toBeVisible();

        const loadMoreButton = page.getByRole('button', {name: /load more/i});
        await expect(loadMoreButton).toBeVisible();
        await loadMoreButton.click();

        await expect(page.getByText('Mickey Saves Christmas', {exact: true})).toBeVisible();
    });

    test('shows movie detail page correctly after search', async ({page}) => {
        await searchForMovie(page, 'mickey');

        const moviePoster = page.getByRole('img', {name: 'Mickey 17'});
        await expect(moviePoster).toBeVisible();
        await moviePoster.click();

        await expect(page.getByRole('heading', {name: 'Mickey 17'})).toBeVisible();
        await expect(page.getByText('Science Fiction')).toBeVisible();
        await expect(page.getByText('Comedy')).toBeVisible();
        await expect(page.getByText('Adventure')).toBeVisible();
        await expect(page.getByText('Runtime: 137 min')).toBeVisible();
        await expect(page.getByText('Origin: United States of America')).toBeVisible();
        await expect(page.getByText('Release Date: 28. 2. 2025')).toBeVisible();

        await expect(page.getByRole('img', {name: 'Mickey 17'})).toBeVisible();
    });

    test('shows no results message', async ({page}) => {
        await searchForMovie(page, 'mewmew');

        await expect(page.getByText('😞 No movies found for "mewmew".')).toBeVisible();
    });

    test('toggles theme on search view and movie detail', async ({page}) => {
        const html = page.locator('html');
        await expect(html).toHaveAttribute('data-theme', 'light');
        expect(await getLsTheme(page)).toBe('light');

        await page.getByRole('button', {name: 'Switch to dark mode'}).click();
        await expect(html).toHaveAttribute('data-theme', 'dark');
        expect(await getLsTheme(page)).toBe('dark');

        const firstMovie = page.locator('div[role="list"] > div').first();
        await expect(firstMovie).toBeVisible();
        await firstMovie.click();

        await page.getByRole('button', { name: 'Switch to light mode' }).click();
        await expect(html).toHaveAttribute('data-theme', 'light');
        expect(await getLsTheme(page)).toBe('light');
    });
});