import { useEffect } from "react";
import useNavigateBack from "./useNavigateBack";

export default function useNavigateBackByBackspacePress() {
  const navigateBack = useNavigateBack();

  useEffect(() => {
    const handleKeyDown = (event: KeyboardEvent) => {
      if (event.key === "Backspace") {
        navigateBack();
      }
    };

    window.addEventListener("keydown", handleKeyDown);

    return () => {
      window.removeEventListener("keydown", handleKeyDown);
    };
  }, [navigateBack]);
}
