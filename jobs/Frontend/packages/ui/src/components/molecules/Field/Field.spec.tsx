/*
 |-----------------------------------------------------------------------------
 | src/components/molecules/Field/Field.spec.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import { type MountResult, expect, test } from '@repo/testing/components';

import Field from './Field';
import { props } from './mocks';

test.describe('Field', () => {
	let component: MountResult;

	test.beforeEach(async ({ mount }) => {
		component = await mount(<Field {...props} classNames="class" />);
	});

	test('renders', async () => {
		await expect(component).toHaveAttribute('data-testid', 'field');
	});

	test('props', async () => {
		await expect(component).toContainClass('class');
	});

	test.describe('with', () => {
		test('error', async ({ page }) => {
			await component.update(<Field {...props} error="Error" />);

			const error = page.getByTestId('error');
			await expect(error).toBeVisible();
		});

		test('hint', async ({ page }) => {
			await component.update(<Field {...props} hint="Hint" />);

			const hint = page.getByTestId('hint');
			await expect(hint).toBeVisible();
		});
	});
});
