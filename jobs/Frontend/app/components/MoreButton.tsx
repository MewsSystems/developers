import { AiFillDownCircle } from 'react-icons/ai';

export interface MoreButtonProps {
  readonly onClick: () => void;
}

export const MoreButton = (props: MoreButtonProps) => {
  return (
    <div className="col-span-full flex items-center justify-center">
      <button className="group" onClick={props.onClick}>
        <AiFillDownCircle
          className="fill-gray-500 transition-colors group-hover:fill-gray-800 dark:group-hover:fill-gray-300"
          size={60}
        />
      </button>
    </div>
  );
};
