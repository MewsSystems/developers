import LoadingSpinner from '@/components/LoadingSpinner';

const Loading = () => {
  return (
    <div className="w-full flex justify-center mt-20">
      <LoadingSpinner className="w-14 h-14" />
    </div>
  );
};

export default Loading;
