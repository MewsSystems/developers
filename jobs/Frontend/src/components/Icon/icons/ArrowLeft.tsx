import { SVGProps } from "../types";

export function ArrowLeft({
  width = 24,
  height = 24,
  color = "black",
}: SVGProps) {
  return (
    <svg
      aria-hidden="true"
      fill={color}
      width={width}
      height={height}
      viewBox="0 0 24 24"
    >
      <path d="M20 11H6.414l3.95-3.95A1 1 0 0 0 8.95 5.636l-5.657 5.657a1 1 0 0 0 0 1.414l5.657 5.657a1 1 0 0 0 1.414-1.414L6.414 13H20a1 1 0 0 0 0-2z" />
    </svg>
  );
}
