/*
 |-----------------------------------------------------------------------------
 | components/Notifications/types.ts
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

interface INotificationsProps {
	errors?: any; // eslint-disable-line @typescript-eslint/no-explicit-any
	isError?: boolean;
	isInvalid?: boolean;
	isSuccess?: boolean;
	message?: string;
}

export type { INotificationsProps };
