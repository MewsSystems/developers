/*
 |-----------------------------------------------------------------------------
 | src/components/atoms/Input/Input.spec.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import { type MountResult, expect, test } from '@repo/testing/components';

import Input from './Input';
import { props } from './mocks';

test.describe('Input', () => {
	let component: MountResult;

	test.beforeEach(async ({ mount }) => {
		component = await mount(
			<Input
				{...props}
				classNames="class"
				autocomplete="off"
				defaultValue="default"
			/>,
		);
	});

	test('renders', async () => {
		await expect(component).toHaveAttribute('data-testid', 'input');
	});

	test('props', async () => {
		await expect(component).toContainClass('class');
		await expect(component).toHaveAttribute('autocomplete', 'off');
		await expect(component).toHaveAttribute('value', 'default');
		await expect(component).toHaveAttribute('id', props.id);
		await expect(component).toHaveAttribute('name', props.name);
	});

	test.describe('is', () => {
		test('disabled', async () => {
			await component.update(<Input {...props} isDisabled />);

			await expect(component).toHaveAttribute('disabled');
		});

		test('invalid', async () => {
			await component.update(<Input {...props} isInvalid />);

			await expect(component).toContainClass('border-error-500');
		});

		test('optional', async () => {
			await component.update(<Input {...props} isOptional />);

			await expect(component).not.toHaveAttribute('required');
		});

		test('readonly', async () => {
			await component.update(<Input {...props} isReadonly />);

			await expect(component).toHaveAttribute('readonly');
		});
	});

	test.describe('with', () => {
		test('placeholder', async () => {
			await component.update(
				<Input {...props} placeholder="placeholder" />,
			);

			await expect(component).toHaveAttribute(
				'placeholder',
				'placeholder',
			);
		});
	});

	test.describe('events', () => {
		test('onBlur', async () => {
			let event: boolean = false;

			await component.update(
				<Input
					{...props}
					onBlur={() => {
						event = true;
					}}
				/>,
			);

			await component.fill('Input');
			await component.blur();

			expect(event).toBeTruthy();
		});

		test('onChange', async () => {
			let event: boolean = false;

			await component.update(
				<Input
					{...props}
					onChange={() => {
						event = true;
					}}
				/>,
			);

			await component.fill('Input');

			expect(event).toBeTruthy();
		});
	});
});
