import { useEffect, useRef, useState } from "react";

export function useDebounce<T>(value: T, delay = 500) {
    const [debounced, setDebounced] = useState(value);
    const timer = useRef<number | null>(null);

    useEffect(() => {
        if (timer.current) window.clearTimeout(timer.current);
        timer.current = window.setTimeout(() => setDebounced(value), delay);
        return () => {
            if (timer.current) window.clearTimeout(timer.current);
        };
    }, [value, delay]);

    const isDebouncing = debounced !== value;
    return { debounced, isDebouncing };
}
