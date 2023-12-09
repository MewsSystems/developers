import { TypedUseSelectorHook, useDispatch, useSelector } from 'react-redux';
import type { RootState, AppDispatch } from './store';

// Use throughout your app instead of plain `useDispatch` and `useSelector`
export const useAppDispatch = () => useDispatch<AppDispatch>();
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;

export const useDebounce = (ms: number) => {
    let timeout: NodeJS.Timeout | null = null;

    const debounce = (fn: (...args: any[]) => void) => {
        return (...args: any[]) => {
            if (timeout) clearTimeout(timeout);
            timeout = setTimeout(()=>fn(...args), ms);
        }
    }

    return debounce;
}