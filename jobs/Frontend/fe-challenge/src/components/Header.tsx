import { Link } from 'react-router-dom';
import Logo from '@/components/Logo';
import usePreviousLocation from '@/hooks/usePreviousLocation';

const Header = () => {
  const previousLocation = usePreviousLocation();

  return (
    <header className="h-16 bg-white/75 z-10 border-b border-slate-900/10 flex items-center sticky top-0 px-4 backdrop-blur-xl">
      <Link to={previousLocation} aria-label="Home">
        <Logo className="w-[120px]" />
      </Link>
    </header>
  );
};

export default Header;
