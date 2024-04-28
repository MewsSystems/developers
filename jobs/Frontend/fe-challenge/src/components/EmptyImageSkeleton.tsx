import ImageIcon from '@/components/ImageIcon';

const EmptyImageSkeleton = () => {
  return (
    <div className="flex h-full rounded-xl items-center justify-center bg-gray-100">
      <ImageIcon className="w-9 h-9 text-gray-300" />
    </div>
  );
};

export default EmptyImageSkeleton;
