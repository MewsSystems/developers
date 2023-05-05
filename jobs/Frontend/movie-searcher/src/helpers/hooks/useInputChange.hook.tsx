import { ChangeEvent, useState } from "react";

const useInputChange = () => {
  const [inputValue, setInputValue] = useState<string | undefined>();

  const inputValueChangeHandler = (e: ChangeEvent<HTMLInputElement>) => {
    setInputValue(e.target.value);
  };

  return { inputValue, inputValueChangeHandler };
};

export { useInputChange };
