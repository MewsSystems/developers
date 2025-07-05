interface ScoreProps {
  score: string;
}

export function Score({ score }: ScoreProps) {
  return (
    <p className="text-stone-800 text-sm mt-1">
      <span className="text-cyan-700">Score:</span> <output>{score}</output>
    </p>
  );
}
