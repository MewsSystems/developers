import {useRef, useEffect} from 'react';

// source: https://overreacted.io/making-setinterval-declarative-with-react-hooks/

export default (callback, delay = 1000, watch = []) => {
  const savedCallback = useRef ();
  useEffect (
    () => {
      if (!savedCallback.current) callback ();
      savedCallback.current = callback;
    },
    [callback, ...watch]
  );
  useEffect (
    () => {
      const tick = () => savedCallback.current ();
      if (delay !== null) {
        let id = setInterval (tick, delay);
        return () => clearInterval (id);
      }
    },
    [delay, ...watch]
  );
};
