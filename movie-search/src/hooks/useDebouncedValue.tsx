import { useState, useEffect } from "react";

export const useDebouncedValue = (value: string, delay: number = 400) => {
    const [debounced, setDebounced] = useState(value);

    useEffect(() => {
        const timeout = setTimeout(() => {
            setDebounced(value);
        }, delay);

        return () => clearTimeout(timeout);
    }, [value, delay]);

    return debounced;
};