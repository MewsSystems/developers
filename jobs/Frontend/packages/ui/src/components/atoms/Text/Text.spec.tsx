/*
 |-----------------------------------------------------------------------------
 | src/components/atoms/Text/Text.spec.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import { type MountResult, expect, test } from '@repo/testing/components';

import {
	colours,
	heights,
	lines,
	sizes,
	transforms,
	weights,
} from './constants';
import Text from './Text';
import { props } from './mocks';

test.describe('Text', () => {
	let component: MountResult;

	test.beforeEach(async ({ mount }) => {
		component = await mount(
			<Text
				{...props}
				classNames="class"
				clamp={2}
				leading="normal"
				schema="description"
				transform="capitalize"
			/>,
		);
	});

	test('renders', async () => {
		await expect(component).toHaveAttribute('data-testid', 'text');
	});

	test('props', async () => {
		await expect(component).toContainClass(lines[2] as string);
		await expect(component).toContainClass('class');
		await expect(component).toContainClass(
			colours[props.colour as keyof typeof colours],
		);
		await expect(component).toContainClass(heights['normal']);
		await expect(component).toHaveAttribute('itemprop', 'description');
		await expect(component).toContainClass(
			sizes[props.size as keyof typeof sizes],
		);
		await expect(component).toHaveText(props.text as string);
		await expect(component).toContainClass(transforms['capitalize']);
		await expect(component).toContainClass(
			weights[props.weight as keyof typeof weights],
		);
	});

	test.describe('is', () => {
		test('italic', async () => {
			await component.update(<Text {...props} isItalic />);

			await expect(component).toContainClass('italic');
		});
	});
});
