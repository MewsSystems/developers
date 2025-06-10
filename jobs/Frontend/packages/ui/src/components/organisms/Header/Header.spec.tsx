/*
 |-----------------------------------------------------------------------------
 | src/components/atoms/Header/Header.spec.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import { type MountResult, expect, test } from '@repo/testing/components';

import Header from './Header';
import { props } from './mocks';

test.describe('Header', () => {
	let component: MountResult;

	test.beforeEach(async ({ mount }) => {
		component = await mount(<Header {...props} classNames="class" />);
	});

	test('renders', async () => {
		await expect(component).toHaveAttribute('data-testid', 'header');
	});

	test('props', async () => {
		await expect(component).toContainClass('class');
		await expect(component).toHaveText(props.title);
	});
});
