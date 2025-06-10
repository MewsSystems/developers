/*
 |-----------------------------------------------------------------------------
 | src/components/atoms/Loader/Loader.spec.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import { type MountResult, expect, test } from '@repo/testing/components';

import Loader from './Loader';

test.describe('Loader', () => {
	let component: MountResult;

	test.beforeEach(async ({ mount }) => {
		component = await mount(<Loader />);
	});

	test('renders', async () => {
		await expect(component).toHaveAttribute('data-testid', 'loader');
	});
});
