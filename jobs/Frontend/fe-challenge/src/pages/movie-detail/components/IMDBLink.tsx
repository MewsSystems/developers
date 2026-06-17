import IMDBIcon from '@/components/IMDBIcon';

interface IMDBLinkProps {
  imdbURL: string;
}

const IMDBLink = ({ imdbURL }: IMDBLinkProps) => {
  return (
    <a
      rel="noopener noreferrer"
      href={imdbURL}
      target="_blank"
      className="bg-[#EFC200] inline-block text-sm px-2 rounded-md mt-3"
    >
      <IMDBIcon className="h-8 text-gray-600" />
    </a>
  );
};

export default IMDBLink;
