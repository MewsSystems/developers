import { ClassValue, clsx } from "clsx";
import { twMerge } from "tailwind-merge";

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}

export function times<T>(n: number, fn: (index: number) => T) {
  return Array(n)
    .fill(null)
    .map((_, index) => fn(index));
}

export function isEmpty<T>(arr: T[]) {
  return arr.length === 0;
}
