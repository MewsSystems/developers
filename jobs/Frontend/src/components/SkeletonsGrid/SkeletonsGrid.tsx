import Skeleton from "@/components/SkeletonsGrid/Skeleton/Skeleton";

interface SkeletonsGridProps {
  amount?: number;
}

const SkeletonsGrid: React.FC<SkeletonsGridProps> = ({ amount = 20 }) => {
  return (
    <>
      {Array.from({ length: amount }).map((_, i) => (
        <Skeleton key={i} />
      ))}
    </>
  );
};

export default SkeletonsGrid;
