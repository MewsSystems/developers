/*
 |-----------------------------------------------------------------------------
 | components/Notifications/Notifications.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { ReactElement } from 'react';

import Alert, { type IAlertProps } from '@repo/ui/components/molecules/Alert';

import type { INotificationsProps } from './types';

const Notifications = ({
	errors,
	isError,
	isInvalid,
	isSuccess,
	message,
}: INotificationsProps): ReactElement => {
	return (
		<>
			{isError && (
				<Alert text={message as IAlertProps['text']} variant="error" />
			)}

			{isInvalid && (
				<Alert
					text={`Failed to complete because ${Object.keys(errors).length} field${Object.keys(errors).length > 1 ? 's are' : ' is'} invalid`}
					variant="error"
				/>
			)}

			{isSuccess && (
				<Alert
					text={message as IAlertProps['text']}
					variant="success"
				/>
			)}
		</>
	);
};

export default Notifications;
