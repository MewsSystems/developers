/*
 |-----------------------------------------------------------------------------
 | src/components/molecules/Alert/types.ts
 |-----------------------------------------------------------------------------
 */

interface IAlertProps {
	classNames?: string;
	text: string;
	variant?: 'error' | 'info' | 'success' | 'warning';
}

type TAlertVariants = {
	error: {
		classes: string;
		icon: 'Error';
	};
	info: {
		classes: string;
		icon: 'Info';
	};
	success: {
		classes: string;
		icon: 'CheckCircle';
	};
	warning: {
		classes: string;
		icon: 'Warning';
	};
};

export type { IAlertProps, TAlertVariants };
