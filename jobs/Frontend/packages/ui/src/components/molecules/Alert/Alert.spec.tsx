/*
 |-----------------------------------------------------------------------------
 | src/components/atoms/Alert/Alert.spec.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import { type MountResult, expect, test } from '@repo/testing/components';

import Alert, { type IAlertProps } from './index';
import { props } from './mocks';

const VARIANTS = {
	error: {
		classes: 'bg-error-50 border-error-500 text-error-500',
		icon: 'Error',
	},
	info: {
		classes: 'bg-info-50 border-info-500 text-info-500',
		icon: 'Info',
	},
	success: {
		classes: 'bg-success-50 border-success-500 text-success-500',
		icon: 'CheckCircle',
	},
	warning: {
		classes: 'bg-warning-50 border-warning-500 text-warning-500',
		icon: 'Warning',
	},
} as const;

test.describe('Alert', () => {
	let component: MountResult;

	const commonEvents = async (component: MountResult) => {
		test.describe('events', () => {
			test('onClick', async ({ page }) => {
				await page.getByTestId('close').click();

				// TODO: Require a test to check if component is in dom/visible
				// await expect(component).toBeHidden();
			});

			test('timeout', async ({ page }) => {
				await page.waitForTimeout(6000);

				// TODO: Require a test to check if component is in dom/visible
				// await expect(component).toBeHidden();
			});
		});
	};

	const commonProps = async (component: MountResult) => {
		await expect(component).toContainClass('class');
		await expect(component).toContainText(props.text);
	};

	const variants = async (
		component: MountResult,
		variant: keyof typeof VARIANTS,
	) => {
		const classes = VARIANTS[variant].classes;

		await expect(component).toHaveAttribute('data-testid', 'alert');
		await expect(component).toContainClass(classes);
	};

	Object.keys(VARIANTS).forEach((variant) => {
		test.describe(`variant ${variant}`, () => {
			test.beforeEach(async ({ mount }) => {
				component = await mount(
					<Alert
						{...props}
						classNames="class"
						variant={variant as IAlertProps['variant']}
					/>,
				);
			});

			test('renders', async () => {
				await variants(component, variant as keyof typeof VARIANTS);
			});

			test('props', async () => {
				await commonProps(component);
			});

			commonEvents(component);
		});
	});
});
