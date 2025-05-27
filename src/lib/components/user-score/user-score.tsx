interface UserScoreProps {
  percent: number;
  size?: number;
  strokeWidth?: number;
}

const scoreColors = {
  green: '#21d07a',
  yellow: '#d2d531',
  red: '#db2360',
};

export const UserScore = ({ percent, size = 36, strokeWidth = 4 }: UserScoreProps) => {
  const radius = (size - strokeWidth) / 2;
  const circumference = 2 * Math.PI * radius;
  const offset = circumference * (1 - percent / 100);

  const getColor = (p: number) => {
    if (p >= 70) return scoreColors.green;
    if (p >= 40) return scoreColors.yellow;
    return scoreColors.red;
  };

  return (
    <svg width={size} height={size}>
      <circle
        stroke="#423d0f"
        fill="transparent"
        strokeWidth={strokeWidth}
        r={radius}
        cx={size / 2}
        cy={size / 2}
      />
      <circle
        stroke={getColor(percent)}
        fill="transparent"
        strokeWidth={strokeWidth}
        strokeDasharray={circumference}
        strokeDashoffset={offset}
        strokeLinecap="round"
        r={radius}
        cx={size / 2}
        cy={size / 2}
      />
      <text
        x="50%"
        y="50%"
        dy="0.3em"
        textAnchor="middle"
        fontSize="0.70rem"
        fill="#fff"
        fontWeight="light"
      >
        {percent}%
      </text>
    </svg>
  );
};
