import { useEffect } from 'react';
import LocalStoreUtil from '../utils/LocalStorageUtil';

const useResetLocalStorageOnRefresh = () => {
    useEffect(() => {
        window.addEventListener('beforeunload', () =>
            LocalStoreUtil.clearAll()
        );
        return () => {
            window.removeEventListener('beforeunload', () =>
                LocalStoreUtil.clearAll()
            );
        };
    }, []);
};

export default useResetLocalStorageOnRefresh;
