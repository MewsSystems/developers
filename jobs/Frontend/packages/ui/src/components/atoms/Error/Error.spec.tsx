/*
 |-----------------------------------------------------------------------------
 | src/components/atoms/Error/Error.spec.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import { type MountResult, expect, test } from '@repo/testing/components';

import Error from './Error';
import { props } from './mocks';

test.describe('Error', () => {
	let component: MountResult;

	test.beforeEach(async ({ mount }) => {
		component = await mount(<Error {...props} classNames="class" />);
	});

	test('renders', async () => {
		await expect(component).toHaveAttribute('data-testid', 'error');
	});

	test('props', async () => {
		await expect(component).toHaveText(props.text);
	});
});
