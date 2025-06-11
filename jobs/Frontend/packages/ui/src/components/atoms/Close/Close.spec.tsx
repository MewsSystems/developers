/*
 |-----------------------------------------------------------------------------
 | src/components/atoms/Close/Close.spec.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import { type MountResult, expect, test } from '@repo/testing/components';

import { colours, sizes, thicknesses } from './constants';
import Close from './Close';
import { props } from './mocks';

test.describe('Close', () => {
	let component: MountResult;

	test.beforeEach(async ({ mount }) => {
		component = await mount(<Close {...props} classNames="class" />);
	});

	test('renders', async () => {
		await expect(component).toHaveAttribute('data-testid', 'close');
	});

	test('props', async () => {
		await expect(component).toContainClass('class');
		await expect(component).toContainClass(
			// @ts-expect-error TODO:
			colours[props.colour as keyof typeof colours],
		);
		await expect(component).toContainClass(
			// @ts-expect-error TODO:
			colours.hover[props.hover as keyof typeof colours],
		);
		await expect(component).toHaveText(props.label);
		await expect(component).toContainClass(
			sizes[props.size as keyof typeof sizes],
		);
		await expect(component).toContainClass(
			thicknesses[props.thickness as keyof typeof thicknesses],
		);
	});

	test.describe('Is', () => {
		test('disabled', async () => {
			await component.update(<Close {...props} isDisabled />);

			await expect(component).toHaveAttribute('disabled');
		});
	});
});
