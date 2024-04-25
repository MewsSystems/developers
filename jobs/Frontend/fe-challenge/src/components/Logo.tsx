interface LogoProps {
  className?: string;
}

const Logo = ({ className }: LogoProps) => {
  return (
    <svg
      className={className}
      xmlns="http://www.w3.org/2000/svg"
      viewBox="0 0 901.62 113.84"
    >
      <desc>MEWS logo</desc>
      <g>
        <g fill="currentColor">
          <path d="M901,69.73c-3.16-20.93-28.21-22.16-44.79-23,0,0-68.41-1.85-75.63-2.17-6.35-.27-13.73-1.5-14.46-8.59-.75-7.42,5.94-10.86,12-12.15,12.16-2.57,24.3-2.79,36.35-2.79,26.48,0,50.85,2.91,76.58,7.61L893.86,8l0-.15C874.47,4.27,847.63,0,819,0a289.37,289.37,0,0,0-37.49,2.27c-13.54,1.78-28.37,4.89-35.65,14.42-6.18,8.1-6.81,21.78-3.07,31.11,6.44,16.05,29.23,16.67,43.44,17.31,0,0,68.82,2,76.11,2.29,6.35.28,13.74,1.55,14.46,8.85.65,6.53-3.7,11.25-12.68,13.12-11.74,2.45-24.13,2.78-36.17,2.78-29.26,0-55.1-3.06-84.81-7.67l-2.43,20.85a433.24,433.24,0,0,0,82.77,8.51h.41c12.61,0,25.34,0,37.86-1.8,11-1.59,22.61-4,31.3-11.57S902.58,80.19,901,69.73Z" />
          <path d="M97.48,85.81,29.06.77H0V113.12H25.7V35.23l57,70.79c8,10,21.58,10,29.66,0l56.95-70.79v77.89H195V.77H165.9Z" />
          <polygon points="409.96 0.77 260.51 0.77 260.51 113.12 411.21 113.12 414.99 90.4 286.21 90.4 286.21 68.31 404.86 68.31 404.86 45.58 286.21 45.58 286.21 23.49 409.96 23.49 409.96 0.77" />
          <path d="M658,113.09,704.82.77H678.29l-35.88,86L600.13,11a21.31,21.31,0,0,0-36.94,0L520.9,86.74,485,.77H458.5l46.87,112.32h27.27l49-88.62,49,88.62Z" />
        </g>
      </g>
    </svg>
  );
};

export default Logo;
