/*
 |-----------------------------------------------------------------------------
 | src/components/atoms/Label/Label.spec.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import { type MountResult, expect, test } from '@repo/testing/components';

import { sizes, weights } from './constants';
import Label from './Label';
import { props } from './mocks';

test.describe('Label', () => {
	let component: MountResult;

	test.beforeEach(async ({ mount }) => {
		component = await mount(<Label {...props} classNames="class" />);
	});

	test('renders', async () => {
		await expect(component).toHaveAttribute('data-testid', 'label');
	});

	test('props', async () => {
		await expect(component).toContainClass('class');
		await expect(component).toHaveAttribute('for', props.id);
		await expect(component).toContainClass(
			sizes[props.size as keyof typeof sizes],
		);
		await expect(component).toHaveText(props.text);
		await expect(component).toContainClass(
			weights[props.weight as keyof typeof weights],
		);
	});

	test.describe('is', () => {
		test('disabled', async () => {
			await component.update(<Label {...props} isDisabled />);

			await expect(component).toContainClass('pointer-events-none');
		});

		test('hidden', async () => {
			await component.update(<Label {...props} isHidden />);

			await expect(component).toContainClass('sr-only');
		});
	});
});
