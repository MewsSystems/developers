/*
 |-----------------------------------------------------------------------------
 | src/components/molecules/Alert/hooks.ts
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import { useEffect, useState } from 'react';

const useAlertState = () => {
	const [visible, setVisible] = useState<boolean>(true);

	useEffect(() => {
		const timeout = setTimeout(() => {
			setVisible(false);
		}, 6000);

		return () => {
			clearTimeout(timeout);
		};
	}, []);

	return { setVisible, visible };
};

export { useAlertState };
