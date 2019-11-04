// credit to https://reactjs.org/docs/hooks-faq.html#how-to-get-the-previous-props-or-state
import { useEffect, useRef } from 'react';

export default function (value) {
    const ref = useRef();
    useEffect(() => {
        ref.current = value;
    });
    return ref.current;

}
