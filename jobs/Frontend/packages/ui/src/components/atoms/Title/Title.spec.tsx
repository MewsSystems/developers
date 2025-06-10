/*
 |-----------------------------------------------------------------------------
 | src/components/atoms/Title/Title.spec.tsx
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
import Title from './Title';
import { props } from './mocks';

test.describe('Title', () => {
	let component: MountResult;

	test.beforeEach(async ({ mount }) => {
		component = await mount(
			<Title
				{...props}
				classNames="class"
				clamp={2}
				leading="normal"
				schema="title"
				transform="capitalize"
			/>,
		);
	});

	test('renders', async () => {
		await expect(component).toHaveAttribute('data-testid', 'title');
	});

	test('props', async () => {
		await expect(component).toContainClass(lines[2] as string);
		await expect(component).toContainClass('class');
		await expect(component).toContainClass(
			colours[props.colour as keyof typeof colours],
		);
		await expect(component).toContainClass(heights['normal']);
		expect(
			component.getByRole('heading', { level: props.level }),
		).toBeTruthy();
		await expect(component).toHaveAttribute('itemprop', 'title');
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
		test('screen reader only', async () => {
			await component.update(<Title {...props} isSrOnly />);

			await expect(component).toContainClass('sr-only');
		});
	});
});
