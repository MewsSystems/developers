/*
 |-----------------------------------------------------------------------------
 | src/components/atoms/Hint/Hint.spec.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import { type MountResult, expect, test } from '@repo/testing/components';

import Hint from './Hint';
import { props } from './mocks';

test.describe('Hint', () => {
	let component: MountResult;

	test.beforeEach(async ({ mount }) => {
		component = await mount(<Hint {...props} classNames="class" />);
	});

	test('renders', async () => {
		await expect(component).toHaveAttribute('data-testid', 'hint');
	});

	test('props', async () => {
		await expect(component).toContainClass('class');
		await expect(component).toHaveText(props.text);
	});
});
