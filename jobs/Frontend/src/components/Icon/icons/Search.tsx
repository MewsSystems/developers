import { SVGProps } from "../types";

export function Search({ width = 24, height = 24, color = "black" }: SVGProps) {
  return (
    <svg
      aria-hidden="true"
      fill={color}
      width={width}
      height={height}
      viewBox="0 0 24 24"
    >
      <path d="M20.207 18.793L16.6 15.184a7.027 7.027 0 1 0-1.416 1.416l3.609 3.609a1 1 0 0 0 1.414-1.416zM6 11a5 5 0 1 1 5 5 5.006 5.006 0 0 1-5-5z" />
    </svg>
  );
}
