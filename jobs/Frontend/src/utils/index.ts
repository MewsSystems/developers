import { ReactEventHandler } from "react";

export function debounce<F extends (...params: any[]) => void>(
  fn: F,
  delay: number
) {
  let timeoutID: number | null = null;
  return function (this: object, ...args: []) {
    if (timeoutID) {
      clearTimeout(timeoutID);
    }
    timeoutID = window.setTimeout(() => fn.apply(this, args), delay);
  } as F;
}

export const handleImageLoadingError: ReactEventHandler<HTMLImageElement> = (
  ev
) => {
  ev.currentTarget.src =
    "https://raw.githubusercontent.com/koehlersimon/fallback/master/Resources/Public/Images/placeholder.jpg";
};
